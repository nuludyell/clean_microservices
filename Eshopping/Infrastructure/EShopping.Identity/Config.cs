// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace EShopping.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogapi"),
                new ApiScope("basketapi"),
                new ApiScope("catalogapi.read"),
                new ApiScope("catalogapi.write"),
                new ApiScope("eshoppinggateway")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                // List of Microservices can go here
                 new ApiResource("Catalog", "Catalog.API")
                 {
                     Scopes =
                     {
                         "catalogapi.read",
                         "catalogapi.write"
                     }
                 },
                 new ApiResource("Basket", "Basket.API")
                 {
                     Scopes = { "basketapi" }
                 },
                 new ApiResource("EShoppingGateway", "EShopping Gateway")
                 {
                     Scopes = { "eshoppinggateway", "basketapi" }
                 }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m flow
                new Client
                {
                    ClientName = "Catalog API Client",
                    ClientId = "CatalogApiClient",
                    ClientSecrets = {new Secret("b957da13-2bae-4f66-a102-0d0f73096d68".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "catalogapi.read",
                        "catalogapi.write"
                    }
                },
                new Client
                {
                    ClientName = "Basket API Client",
                    ClientId = "BasketApiClient",
                    ClientSecrets = {new Secret("4a13112d-8f19-4e39-8cee-349de88d4f56".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"basketapi"}
                },
                new Client
                {
                    ClientName = "EShopping Gateway Client",
                    ClientId = "EShoppingGatewayClient",
                    ClientSecrets = {new Secret("497d7f5d-6553-4dff-ba15-ce86586d7d05".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "eshoppinggateway", "basketapi" }
                }
            };
    }
}