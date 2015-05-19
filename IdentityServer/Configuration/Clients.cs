using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Models;

namespace IdentityServer
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client> {
                new Client {
                    ClientId = "WebApp1",
                    ClientName = "WebApp1",
                    Enabled = true,
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    AllowRememberConsent = true,
                        RedirectUris = new List<string>{"https://localhost:44300/account/signInCallback"},
                        PostLogoutRedirectUris = new List<string>{"https://localhost:44300/"},
                    ScopeRestrictions = new List<string> {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email
                    },
                AccessTokenType = AccessTokenType.Jwt
                },
                new Client {
                    ClientId = "WebApp2",
                    ClientName = "WebApp2",
                    Enabled = true,
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    AllowRememberConsent = true,
                        RedirectUris = new List<string>{"https://localhost:44301/account/signInCallback"},
                        PostLogoutRedirectUris = new List<string>{"https://localhost:44301/"},
                    ScopeRestrictions = new List<string> {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email
                    },
                AccessTokenType = AccessTokenType.Jwt
                }
            };
        }
    }
}
