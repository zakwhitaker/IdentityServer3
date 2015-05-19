using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace IdentityServer.Services
{
    /// <summary>
    /// In-memory client store
    /// </summary>
    public class CustomClientStore : IClientStore
    {
        readonly IEnumerable<Client> _clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomClientStore"/> class.
        /// </summary>
        /// <param name="clients">The clients.</param>
        public CustomClientStore(IEnumerable<Client> clients)
        {
            _clients = Clients.Get();
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>
        /// The client
        /// </returns>
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var query =
                from client in _clients
                where client.ClientId == clientId && client.Enabled
                select client;

            return Task.FromResult(query.SingleOrDefault());
        }
    }
}