// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace LoverCloud.Identity
{
    using IdentityModel;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using LoverCloud.Core.Models;
    using System;
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

        public static IEnumerable<LoverCloudUser> Users =>
            new LoverCloudUser[]
            {
                new LoverCloudUser
                {
                    UserName = "陈畏民",
                    Email = "1634205628@qq.com",
                    PhoneNumber = "13814307540",
                    RegisterDate = DateTime.Now
                },
                new LoverCloudUser
                {
                    UserName = "朱容容",
                    Email = "1324323815@qq.com",
                    PhoneNumber = "15751118812",
                    RegisterDate = DateTime.Now
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