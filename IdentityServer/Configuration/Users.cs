using IdentityServer.Objects;
using IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace IdentityServer
{
    public static class Users
    {
        public static List<User> Get()
        {
            return new List<User> {
                new User {
                    Username = "user",
                    Password = "password",
                    Subject = "1",
                    Claims = new List<Claim>
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Test"),
                        new Claim(Constants.ClaimTypes.FamilyName, "User"),
                        new Claim(Constants.ClaimTypes.Email, "test@test.com")
                    }
                }
            };
        }
    }
}
