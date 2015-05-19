using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;

namespace IdentityServer
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope> {
            StandardScopes.OpenId,
            StandardScopes.Profile,
            StandardScopes.Email
        };
        }
    }
}
