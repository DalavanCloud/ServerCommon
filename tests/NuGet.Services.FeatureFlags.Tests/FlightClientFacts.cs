// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Services.FeatureFlags.Tests
{
    public class FlightClientFacts
    {
        /// <summary>
        /// Tests <see cref="FlightClient.Enabled(string, User)"/>
        /// </summary>
        public class IsEnabled1 : FactsBase
        {
            [Fact]
            public void WhenLatestFlagsUnitialized_ReturnsUnknown()
            {

            }

            [Fact]
            public void WhenLatestFlagsStale_ReturnsUnknown()
            {

            }

            [Fact]
            public void WhenFlightUnknown_ReturnsUnknown()
            {

            }

            [Fact]
            public void ReturnsDisabled()
            {
            }

            [Fact]
            public void WhenFlightEnabledForAll_ReturnsEnabled()
            {

            }

            [Fact]
            public void WhenAccountIsFlighted_ReturnsEnabled()
            {
            }

            [Fact]
            public void WhenAccountEmailAddressDomainFlighted_ReturnsEnabled()
            {
            }

            [Fact]
            public void WhenAdminsFlighted_ReturnsEnabled()
            {
            }
        }

        /// <summary>
        /// Tests <see cref="FlightClient.Enabled(string, User, bool)"/>
        /// </summary>
        public class IsEnabled2 : FactsBase
        {
            [Fact]
            public void WhenLatestFlagsUnitialized_ReturnsDefault()
            {

            }

            [Fact]
            public void WhenLatestFlagsStale_ReturnsDefault()
            {

            }

            [Fact]
            public void WhenFlightUnknown_ReturnsDefault()
            {

            }

            [Fact]
            public void ReturnsFalse()
            {
            }

            [Fact]
            public void WhenFlightEnabledForAll_ReturnsTrue()
            {

            }

            [Fact]
            public void WhenAccountIsFlighted_ReturnsTrue()
            {
            }

            [Fact]
            public void WhenAccountEmailAddressDomainFlighted_ReturnsTrue()
            {
            }

            [Fact]
            public void WhenAdminsFlighted_ReturnsTrue()
            {
            }
        }

        public class FactsBase
        {

        }
    }
}
