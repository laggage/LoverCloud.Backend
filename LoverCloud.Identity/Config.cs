// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace LoverCloud.Identity
{
    using IdentityModel;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("LoverCloud", "情侣云")
            };

        public static IEnumerable<TestUser> TestUsers =>
            new TestUser[]
            {
                new TestUser
                {
                    Username = "yy",
                    Password = "123456789",
                    Claims = new Claim[]
                    {
                        new Claim("role", ""),
                        new Claim(JwtClaimTypes.Scope, "LoverCloud")
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId= "jwt-client",
                    ClientName = "JwtClient",
                    ClientUri = "http://localhost:3089",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    RequireClientSecret = false,
                    AllowOfflineAccess = true,

                    AllowedScopes = new string[]
                    {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        "LoverCloud"
                    },
                    AlwaysSendClientClaims = true,
                }
            };
    }
}