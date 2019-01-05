// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace NuGet.Services.FeatureFlags.Tests
{
    public class FeatureFlagRefreshServiceFacts
    {
        [Fact]
        public async Task ReturnsUnitialized()
        {
            await Task.Yield();
        }

        [Fact]
        public async Task ReturnsStale()
        {
            await Task.Yield();
        }

        [Fact]
        public async Task ReturnsOk()
        {
            await Task.Yield();
        }

        [Fact]
        public async Task RefreshesUntilCancelled()
        {
            await Task.Yield();
        }
    }
}
