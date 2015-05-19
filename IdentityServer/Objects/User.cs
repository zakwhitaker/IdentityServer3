using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Objects
{
    // Summary:
    //     User
    public class User
    {
        // Summary:
        //     Initializes a new instance of the Thinktecture.IdentityServer.Core.Services.InMemory.User
        //     class.
        public User()
        {
            Enabled = true;
            Claims = new List<Claim>();
        }

        // Summary:
        //     Gets or sets the claims.
        public IEnumerable<Claim> Claims { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether this Thinktecture.IdentityServer.Core.Services.InMemory.User
        //     is enabled.
        public bool Enabled { get; set; }
        //
        // Summary:
        //     Gets or sets the password.
        public string Password { get; set; }
        //
        // Summary:
        //     Gets or sets the provider.
        public string Provider { get; set; }
        //
        // Summary:
        //     Gets or sets the provider identifier.
        public string ProviderId { get; set; }
        //
        // Summary:
        //     Gets or sets the subject.
        public string Subject { get; set; }
        //
        // Summary:
        //     Gets or sets the username.
        public string Username { get; set; }
    }
}
