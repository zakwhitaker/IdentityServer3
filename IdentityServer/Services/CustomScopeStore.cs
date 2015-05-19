using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace IdentityServer.Services
{
    /// <summary>
    /// In-memory scope store
    /// </summary>
    public class CustomScopeStore : IScopeStore
    {
        readonly IEnumerable<Scope> _scopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomScopeStore"/> class.
        /// </summary>
        /// <param name="scopes">The scopes.</param>
        public CustomScopeStore(IEnumerable<Scope> scopes)
        {
            _scopes = Scopes.Get();
        }

        /// <summary>
        /// Gets all scopes.
        /// </summary>
        /// <returns>
        /// List of scopes
        /// </returns>
        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException("scopeNames");
            
            var scopes = from s in _scopes
                         where scopeNames.ToList().Contains(s.Name)
                         select s;

            return Task.FromResult<IEnumerable<Scope>>(scopes.ToList());
        }


        /// <summary>
        /// Gets all defined scopes.
        /// </summary>
        /// <param name="publicOnly">if set to <c>true</c> only public scopes are returned.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            if (publicOnly)
            {
                return Task.FromResult(_scopes.Where(s => s.ShowInDiscoveryDocument));
            }

            return Task.FromResult(_scopes);
        }
    }
}