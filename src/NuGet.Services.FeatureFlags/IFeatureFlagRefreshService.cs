// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Services.FeatureFlags
{
    /// <summary>
    /// The service that periodically refreshes the feature flags' state.
    /// </summary>
    public interface IFeatureFlagRefreshService
    {
        /// <summary>
        /// Continuously refresh the feature flags.
        /// </summary>
        /// <param name="cancellationToken">Cancelling this token will complete the returned task.</param>
        /// <returns>A task that completes once the cancellation token is cancelled.</returns>
        Task RunAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Refresh the feature flags once.
        /// </summary>
        /// <param name="cancellationToken">Cancels the refresh.</param>
        /// <returns>A task that completes once the feature flags have been refreshed.</returns>
        Task RefreshAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetch the latest known flags. This should be called after either <see cref="RunAsync(CancellationToken)"/>
        /// or <see cref="RefreshAsync(CancellationToken)"/>.
        /// </summary>
        /// <returns>The latest known flags.</returns>
        LatestFlagsResult GetLatestFlags();
    }
}
