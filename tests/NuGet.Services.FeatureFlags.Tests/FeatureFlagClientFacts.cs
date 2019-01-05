// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

            }

            [Fact]
            public void WhenLatestFlagsStale_ReturnsUnknown()
            {

            }

            [Fact]
            public void WhenFlagUnknown_ReturnsUnknown()
            {

            }

            [Fact]
            public void ReturnsEnabled()
            {

            }

            [Fact]
            public void ReturnsDisabled()
            {
            }
        }

        /// <summary>
        /// Tests <see cref="FeatureFlagClient.IsEnabled(string, bool)"/>
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
            public void WhenFlagUnknown_ReturnsDefault()
            {

            }

            [Fact]
            public void WhenFlagEnabled_ReturnsTrue()
            {

            }

            [Fact]
            public void WhenFlagDisabled_ReturnsFalse()
            {
            }
        }

        public class FactsBase
        {

        }
    }
}
