// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NuGet.Services.FeatureFlags
{
    public class FeatureFlagRefreshService : IFeatureFlagRefreshService
    {
        private readonly IFeatureFlagStorageService _storage;
        private readonly FeatureFlagOptions _options;
        private readonly ILogger<FeatureFlagRefreshService> _logger;

        private FeatureFlagsAndCacheTime _latestFlags;

        public FeatureFlagRefreshService(
            IFeatureFlagStorageService storage,
            FeatureFlagOptions options,
            ILogger<FeatureFlagRefreshService> logger)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _latestFlags = null;
        }

        public LatestFlagsResult GetLatestFlags()
        {
            var latestFlags = _latestFlags;
            if (latestFlags == null)
            {
                return LatestFlagsResult.Uninitialized;
            }

            if (_options.MaximumStaleness.HasValue)
            {
                var staleness = DateTimeOffset.UtcNow - latestFlags.CacheTime;
                if (staleness > _options.MaximumStaleness)
                {
                    return LatestFlagsResult.Stale(latestFlags.FeatureFlags);
                }
            }

            return LatestFlagsResult.Ok(latestFlags.FeatureFlags);
        }

        public async Task RefreshAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            _latestFlags = new FeatureFlagsAndCacheTime(
                await _storage.GetAsync(),
                DateTimeOffset.UtcNow);
        }

        public async Task RunAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Refreshing feature flags...");

                    await RefreshAsync(cancellationToken);

                    _logger.LogInformation("Refreshed feature flags");
                }
                catch (Exception e)
                {
                    _logger.LogError(0, e, "Unable to refresh the feature flags due to exception");
                }

                await Task.Delay(_options.RefreshInterval);
            }
        }

        private class FeatureFlagsAndCacheTime
        {
            public FeatureFlagsAndCacheTime(FeatureFlagsState featureFlags, DateTimeOffset cacheTime)
            {
                FeatureFlags = featureFlags ?? throw new ArgumentNullException(nameof(featureFlags));
                CacheTime = cacheTime;
            }

            public FeatureFlagsState FeatureFlags { get; }
            public DateTimeOffset CacheTime { get; }
        }
    }
}
