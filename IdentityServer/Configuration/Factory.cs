using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.EntityFramework;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using IdentityServer.Services;
using IdentityServer.Objects;

namespace IdentityServer
{
    class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString)
        {
            var factory = new IdentityServerServiceFactory();

            factory.ScopeStore = new Registration<IScopeStore, CustomScopeStore>();
            factory.ClientStore = new Registration<IClientStore, CustomClientStore>();

            factory.Register(new Registration<List<User>>(Users.Get()));
            factory.UserService = new Registration<IUserService, CustomUserService>();

            return factory;
        }
    }
}
