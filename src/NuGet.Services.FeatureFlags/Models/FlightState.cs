// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Services.FeatureFlags
{
    /// <summary>
    /// The state of a specific flight. A flight can enable features for specific users. For example,
    /// the "index package with a license expression" feature could be enabled for only administrators.
    /// </summary>
    public class FlightState
    {
        public FlightState(bool all, bool siteAdmin, IReadOnlyList<string> accounts, IReadOnlyList<string> domains)
        {
            All = all;
            SiteAdmin = siteAdmin;
            Accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            Domains = domains ?? throw new ArgumentNullException(nameof(domains));
        }

        /// <summary>
        /// Whether this flight is enabled for all users. If true, all other properties are ignored.
        /// </summary>
        public bool All { get; }

        /// <summary>
        /// Whether this flight is enabled for NuGet.org administrators.
        /// </summary>
        public bool SiteAdmin { get; }

        /// <summary>
        /// Specific account usernames that have this flight enabled.
        /// </summary>
        public IReadOnlyList<string> Accounts { get; }

        /// <summary>
        /// Specific email domains that have this flight enabled. Example: "microsoft.com" would
        /// enable the flight for "billy@microsoft.com" but not for "bob@nuget.org". Sorry Bob.
        /// </summary>
        public IReadOnlyList<string> Domains { get; }
    }
}
