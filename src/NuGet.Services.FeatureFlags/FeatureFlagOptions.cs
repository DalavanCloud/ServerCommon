// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Services.FeatureFlags
{
    public class FeatureFlagOptions
    {
        /// <summary>
        /// How frequently the feature flags should be refreshed.
        /// </summary>
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// The maximum refresh staleness allowed by the <see cref="IFeatureFlagRefreshService"/>.
        /// If the threshold is reached, the returned feature flags will be labeled as stale.
        /// </summary>
        public TimeSpan? MaximumStaleness { get; set; } = TimeSpan.FromHours(1);
    }
}
