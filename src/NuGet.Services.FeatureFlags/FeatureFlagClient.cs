// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace NuGet.Services.FeatureFlags
{
    public class FeatureFlagClient : IFeatureFlagClient
    {
        private readonly IFeatureFlagRefreshService _flags;
        private readonly ILogger<FeatureFlagClient> _logger;

        public FeatureFlagClient(IFeatureFlagRefreshService flags, ILogger<FeatureFlagClient> logger)
        {
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsEnabled(string feature, bool @default)
        {
            var result = IsEnabled(feature);

            switch (result)
            {
                case FeatureResult.Enabled:
                    return true;

                case FeatureResult.Disabled:
                    return false;

                case FeatureResult.Unknown:
                default:
                    return @default;

            }
        }

        public FeatureResult IsEnabled(string feature)
        {
            var latest = _flags.GetLatestFlags();
            if (latest.Status != LatestFlagsStatus.Ok)
            {
                _logger.LogWarning(
                    "Couldn't determine status of feature {Feature} as the latest flags have status {LatestFlagsStatus}",
                    feature,
                    latest.Status);

                return FeatureResult.Unknown;
            }

            if (!latest.Flags.Features.TryGetValue(feature, out var featureStatus))
            {
                _logger.LogWarning(
                    "Couldn't determine status of feature {Feature} as it isn't in the latest flags",
                    feature);

                return FeatureResult.Unknown;
            }

            switch (featureStatus)
            {
                case FeatureStatus.Enabled:
                    return FeatureResult.Enabled;

                case FeatureStatus.Disabled:
                    return FeatureResult.Disabled;

                default:
                    _logger.LogWarning(
                        "Unknown feature status {FeatureStatus} for feature {Feature}",
                        feature,
                        featureStatus);

                    return FeatureResult.Unknown;
            }
        }
    }
}
