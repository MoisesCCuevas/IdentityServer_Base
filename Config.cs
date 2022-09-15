// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationSystem
{
    public class Config
    {
        public static List<TestUser> SetUsers()
        {
            return new List<TestUser>
            {
                new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "AliceSmith@email.com")
                }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var api1 = new IdentityResource(
                name: "api1",
                displayName: "Identity Server",
                claimTypes: new[] { "api1" });

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                api1
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "",
                    Description = "",
                    ApiSecrets = {
                        new Secret("secret".Sha256())
                    },
                    Scopes = {
                        new Scope()
                        {
                            Name = "api1.scope",
                            Description = "",
                            DisplayName = "",
                            Required = true
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "1",
                    ClientName = "",
                    RedirectUris = { "http://localhost:3000/#/login/" },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 32400,
                    PostLogoutRedirectUris = { "http://localhost:3000/#/" },
                    AllowedCorsOrigins = { "http://localhost:3000" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.scope"
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}