// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace NuGet.Services.FeatureFlags.Tests
{
    public class FeatureFlagClientFacts
    {
        /// <summary>
        /// Tests <see cref="FeatureFlagClient.IsEnabled(string)"/>
        /// </summary>
        public class IsEnabled1 : FactsBase
        {
            [Fact]
            public void WhenLatestFlagsUnitialized_ReturnsUnknown()
            {
                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Uninitialized);

                Assert.Equal(FeatureResult.Unknown, _target.IsEnabled("Feature"));
            }

            [Fact]
            public void WhenLatestFlagsStale_ReturnsUnknown()
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Stale(latestFlags));

                Assert.Equal(FeatureResult.Unknown, _target.IsEnabled("Feature"));
            }

            [Fact]
            public void WhenFlagUnknown_ReturnsUnknown()
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FeatureResult.Unknown, _target.IsEnabled("Feature"));
            }

            [Fact]
            public void ReturnsEnabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFeature("Feature", FeatureStatus.Enabled)
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FeatureResult.Enabled, _target.IsEnabled("Feature"));
            }

            [Fact]
            public void ReturnsDisabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFeature("Feature", FeatureStatus.Disabled)
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FeatureResult.Disabled, _target.IsEnabled("Feature"));
            }
        }

        /// <summary>
        /// Tests <see cref="FeatureFlagClient.IsEnabled(string, bool)"/>
        /// </summary>
        public class IsEnabled2 : FactsBase
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenLatestFlagsUnitialized_ReturnsDefault(bool @default)
            {
                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Uninitialized);

                Assert.Equal(@default, _target.IsEnabled("Feature", @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenLatestFlagsStale_ReturnsDefault(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Stale(latestFlags));

                Assert.Equal(@default, _target.IsEnabled("Feature", @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlagUnknown_ReturnsDefault(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(@default, _target.IsEnabled("Feature", @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlagEnabled_ReturnsTrue(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFeature("Feature", FeatureStatus.Enabled)
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.True(_target.IsEnabled("Feature", @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlagDisabled_ReturnsFalse(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFeature("Feature", FeatureStatus.Disabled)
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.False(_target.IsEnabled("Feature", @default));
            }
        }

        public class FactsBase
        {
            protected readonly Mock<IFeatureFlagRefreshService> _flags;
            protected readonly FeatureFlagClient _target;

            public FactsBase()
            {
                _flags = new Mock<IFeatureFlagRefreshService>();

                _target = new FeatureFlagClient(
                    _flags.Object,
                    Mock.Of<ILogger<FeatureFlagClient>>());
            }
        }
    }
}
