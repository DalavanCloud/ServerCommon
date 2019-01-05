// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Services.Entities;
using Xunit;

namespace NuGet.Services.FeatureFlags.Tests
{
    public class FlightClientFacts
    {
        /// <summary>
        /// Tests <see cref="FlightClient.IsEnabled(string, User)"/>
        /// </summary>
        public class IsEnabled1 : FactsBase
        {
            [Fact]
            public void WhenLatestFlagsUnitialized_ReturnsUnknown()
            {
                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Uninitialized);

                Assert.Equal(FlightResult.Unknown, _target.IsEnabled("Flight", _user));
            }

            [Fact]
            public void WhenLatestFlagsStale_ReturnsUnknown()
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Stale(latestFlags));

                Assert.Equal(FlightResult.Unknown, _target.IsEnabled("Flight", _user));
            }

            [Fact]
            public void WhenFlightUnknown_ReturnsUnknown()
            {
                var latestFlags = FeatureFlagStateBuilder.Create().Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Unknown, _target.IsEnabled("Flight", _user));
            }

            [Fact]
            public void ReturnsDisabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Disabled, _target.IsEnabled("Flight", _user));
            }

            [Fact]
            public void WhenFlightEnabledForAll_ReturnsEnabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: true, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Enabled, _target.IsEnabled("Flight", _user));
                Assert.Equal(FlightResult.Enabled, _target.IsEnabled("Flight", _admin));
            }

            [Fact]
            public void WhenAccountIsFlighted_ReturnsEnabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string> { "Bob" }, domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Enabled, _target.IsEnabled("Flight", _user));
                Assert.Equal(FlightResult.Disabled, _target.IsEnabled("Flight", _admin));
            }

            [Fact]
            public void WhenAccountEmailAddressDomainFlighted_ReturnsEnabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string> { "bob.org" }))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Enabled, _target.IsEnabled("Flight", _user));
                Assert.Equal(FlightResult.Disabled, _target.IsEnabled("Flight", _admin));
            }

            [Fact]
            public void WhenAdminsFlighted_ReturnsEnabled()
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: true, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(FlightResult.Disabled, _target.IsEnabled("Flight", _user));
                Assert.Equal(FlightResult.Enabled, _target.IsEnabled("Flight", _admin));
            }
        }

        /// <summary>
        /// Tests <see cref="FlightClient.IsEnabled(string, User, bool)"/>
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

                Assert.Equal(@default, _target.IsEnabled("Flight", _user, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenLatestFlagsStale_ReturnsDefault(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Stale(latestFlags));

                Assert.Equal(@default, _target.IsEnabled("Flight", _user, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlightUnknown_ReturnsDefault(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.Equal(@default, _target.IsEnabled("Unknown", _user, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlightDisabled_ReturnsFalse(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.False(_target.IsEnabled("Flight", _user, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenFlightEnabledForAll_ReturnsTrue(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: true, siteAdmin: false, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.True(_target.IsEnabled("Flight", _user, @default));
                Assert.True(_target.IsEnabled("Flight", _admin, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenAccountIsFlighted_ReturnsTrue(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string> { "Bob" }, domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.True(_target.IsEnabled("Flight", _user, @default));
                Assert.False(_target.IsEnabled("Flight", _admin, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenAccountEmailAddressDomainFlighted_ReturnsTrue(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: false, accounts: new List<string>(), domains: new List<string> { "nuget.org" }))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.False(_target.IsEnabled("Flight", _user, @default));
                Assert.True(_target.IsEnabled("Flight", _admin, @default));
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void WhenAdminsFlighted_ReturnsTrue(bool @default)
            {
                var latestFlags = FeatureFlagStateBuilder
                    .Create()
                    .WithFlight("Flight", new FlightState(all: false, siteAdmin: true, accounts: new List<string>(), domains: new List<string>()))
                    .Build();

                _flags
                    .Setup(f => f.GetLatestFlags())
                    .Returns(LatestFlagsResult.Ok(latestFlags));

                Assert.False(_target.IsEnabled("Flight", _user, @default));
                Assert.True(_target.IsEnabled("Flight", _admin, @default));
            }
        }

        public class FactsBase
        {
            protected readonly Mock<IFeatureFlagRefreshService> _flags;
            protected readonly FlightClient _target;

            protected readonly User _user;
            protected readonly User _admin;

            public FactsBase()
            {
                _flags = new Mock<IFeatureFlagRefreshService>();

                _target = new FlightClient(
                    _flags.Object,
                    Mock.Of<ILogger<FlightClient>>());

                _user = new User
                {
                    Username = "Bob",
                    EmailAddress = "hello@bob.org",

                    Roles = new List<Role>()
                };

                _admin = new User
                {
                    Username = "Alice",
                    EmailAddress = "admin@nuget.org",

                    Roles = new List<Role>
                    {
                        new Role { Name = "Admins" }
                    }
                };
            }
        }
    }
}
