// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using NuGet.Services.Entities;

namespace NuGet.Services.FeatureFlags
{
    public class FlightClient : IFlightClient
    {
        private const string AdminRoleName = "Admins";

        private readonly IFeatureFlagRefreshService _flags;
        private readonly ILogger<FlightClient> _logger;

        public FlightClient(IFeatureFlagRefreshService flags, ILogger<FlightClient> logger)
        {
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsEnabled(string flightName, User user, bool @default)
        {
            var result = IsEnabled(flightName, user);

            switch (result)
            {
                case FlightResult.Enabled:
                    return true;

                case FlightResult.Disabled:
                    return false;

                case FlightResult.Unknown:
                default:
                    return @default;
            }
        }

        public FlightResult IsEnabled(string flightName, User user)
        {
            var latest = _flags.GetLatestFlags();
            if (latest.Status != LatestFlagsStatus.Ok)
            {
                _logger.LogWarning(
                    "Couldn't determine status of flight {Flight} as the latest flags have status {LatestFlagsStatus}",
                    flightName,
                    latest.Status);

                return FlightResult.Unknown;
            }

            if (!latest.Flags.Flights.TryGetValue(flightName, out var flight))
            {
                _logger.LogWarning(
                    "Couldn't determine status of flight {Flight} as it isn't in the latest feature flags",
                    flightName);

                return FlightResult.Unknown;
            }

            if (flight.All)
            {
                return FlightResult.Enabled;
            }

            if (flight.Accounts.Contains(user.Username))
            {
                return FlightResult.Enabled;
            }

            if (TryParseEmailDomain(user.EmailAddress, out var domain) && flight.Domains.Contains(domain))
            {
                return FlightResult.Enabled;
            }

            if (flight.SiteAdmin && user.IsInRole(AdminRoleName))
            {
                return FlightResult.Enabled;
            }

            return FlightResult.Disabled;
        }

        private bool TryParseEmailDomain(string email, out string domain)
        {
            try
            {
                domain = (new MailAddress(email)).Host;

                return true;
            }
            catch (ArgumentNullException) { }
            catch (ArgumentException) { }
            catch (FormatException) { }

            domain = null;
            return false;
        }
    }
}
