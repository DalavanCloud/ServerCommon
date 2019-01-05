// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace NuGet.Services.FeatureFlags.Tests
{
    public class FeatureFlagRefreshServiceFacts
    {
        private readonly Mock<IFeatureFlagStorageService> _storage;
        private readonly FeatureFlagOptions _options;
        private readonly FeatureFlagRefreshService _target;

        public FeatureFlagRefreshServiceFacts()
        {
            _storage = new Mock<IFeatureFlagStorageService>();
            _options = new FeatureFlagOptions();

            _target = new FeatureFlagRefreshService(
                _storage.Object,
                _options,
                Mock.Of<ILogger<FeatureFlagRefreshService>>());
        }

        [Fact]
        public void ReturnsUnitialized()
        {
            var result = _target.GetLatestFlags();

            Assert.Equal(LatestFlagsStatus.Uninitialized, result.Status);
            Assert.Null(result.Flags);
        }

        [Fact]
        public async Task ReturnsStale()
        {
            // Arrange
            var latestFlags = FeatureFlagStateBuilder
                .Create()
                .WithFeature("Foo", FeatureStatus.Enabled)
                .Build();

            _options.MaximumStaleness = TimeSpan.FromDays(-1);
            _storage
                .Setup(s => s.GetAsync())
                .ReturnsAsync(latestFlags);

            await _target.RefreshAsync();

            // Act
            var result = _target.GetLatestFlags();

            // Assert
            Assert.Equal(LatestFlagsStatus.Stale, result.Status);
            Assert.Same(latestFlags, result.Flags);
        }

        [Fact]
        public async Task ReturnsOk()
        {
            // Arrange
            var latestFlags = FeatureFlagStateBuilder
                .Create()
                .WithFeature("Foo", FeatureStatus.Enabled)
                .Build();

            _options.MaximumStaleness = TimeSpan.FromDays(1);
            _storage
                .Setup(s => s.GetAsync())
                .ReturnsAsync(latestFlags);

            await _target.RefreshAsync();

            // Act
            var result = _target.GetLatestFlags();

            // Assert
            Assert.Equal(LatestFlagsStatus.Ok, result.Status);
            Assert.Same(latestFlags, result.Flags);
        }

        [Fact]
        public async Task RefreshesUntilCancelled()
        {
            // Arrange
            LatestFlagsResult result;
            var count = 0;
            var latestFlags = FeatureFlagStateBuilder
                .Create()
                .WithFeature("Foo", FeatureStatus.Enabled)
                .Build();

            _options.MaximumStaleness = TimeSpan.FromDays(1);
            _options.RefreshInterval = TimeSpan.FromMilliseconds(100);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                _storage
                    .Setup(s => s.GetAsync())
                    .Callback(() =>
                    {
                        count++;

                        if (count == 2)
                        {
                            cancellationTokenSource.Cancel();
                        }
                    })
                    .ReturnsAsync(latestFlags);

                // Act
                await _target.RunAsync(cancellationTokenSource.Token);

                result = _target.GetLatestFlags();
            }

            // Assert
            Assert.Equal(LatestFlagsStatus.Ok, result.Status);
            Assert.Same(latestFlags, result.Flags);

            _storage.Verify(s => s.GetAsync(), Times.Exactly(2));
        }
    }
}
