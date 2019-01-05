// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Services.FeatureFlags
{
    /// <summary>
    /// The latest known feature flags' state.
    /// </summary>
    public class LatestFlagsResult
    {
        public static readonly LatestFlagsResult Uninitialized = new LatestFlagsResult(LatestFlagsStatus.Uninitialized, flags: null);

        private LatestFlagsResult(LatestFlagsStatus status, FeatureFlagsState flags)
        {
            Flags = flags;
            Status = status;
        }

        public static LatestFlagsResult Stale(FeatureFlagsState flags)
        {
            if (flags == null) throw new ArgumentNullException(nameof(flags));

            return new LatestFlagsResult(LatestFlagsStatus.Stale, flags);
        }

        public static LatestFlagsResult Ok(FeatureFlagsState flags)
        {
            if (flags == null) throw new ArgumentNullException(nameof(flags));

            return new LatestFlagsResult(LatestFlagsStatus.Ok, flags);
        }

        /// <summary>
        /// The status of the feature flags.
        /// </summary>
        public LatestFlagsStatus Status { get; }

        /// <summary>
        /// The latest known feature flags. Null if the status is unitialized.
        /// </summary>
        public FeatureFlagsState Flags { get; }
    }

    /// <summary>
    /// The status of the feature flags. Each service and job refresh their
    /// cache of the feature flags.
    /// </summary>
    public enum LatestFlagsStatus
    {
        /// <summary>
        /// The feature flags' state are unknown as they have never been loaded.
        /// </summary>
        Uninitialized = 0,

        /// <summary>
        /// The feature flags are no longer fresh and may be incorrect.
        /// </summary>
        Stale = 1,

        /// <summary>
        /// The feature flags are ready for use.
        /// </summary>
        Ok = 2,
    }
}
