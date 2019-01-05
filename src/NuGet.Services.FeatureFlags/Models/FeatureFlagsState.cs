// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Services.FeatureFlags
{
    /// <summary>
    /// The state of all features and flights.
    /// </summary>
    public class FeatureFlagsState
    {
        public FeatureFlagsState(
            IReadOnlyDictionary<string, FeatureStatus> features,
            IReadOnlyDictionary<string, FlightState> flights)
        {
            Features = features ?? throw new ArgumentNullException(nameof(features));
            Flights = flights ?? throw new ArgumentNullException(nameof(flights));

            if (Flights.Values.Any(f => f == null))
            {
                throw new ArgumentException("Flights cannot be null", nameof(flights));
            }
        }

        /// <summary>
        /// The state of features. Keys are features' unique identifiers, values are features'
        /// statuses (either enabled or disabled).
        /// </summary>
        public IReadOnlyDictionary<string, FeatureStatus> Features { get; }

        /// <summary>
        /// The state of feature flights. Unlike features, flights can have enabled partially
        /// for specific users. Keys are flights' unique identifiers, values are flights
        /// status.
        /// </summary>
        public IReadOnlyDictionary<string, FlightState> Flights { get; }
    }
}
