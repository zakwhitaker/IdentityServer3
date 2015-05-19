using IdentityServer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Extensions;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace IdentityServer.Services
{
    /// <summary>
    /// In-memory user service
    /// </summary>
    public class CustomUserService : IUserService
    {
        readonly List<User> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public CustomUserService(List<User> users)
        {
            _users = Users.Get();
        }

        /// <summary>
        /// This methods gets called before the login page is shown. This allows you to authenticate the user somehow based on data coming from the host (e.g. client certificates or trusted headers)
        /// </summary>
        /// <param name="message">The signin message.</param>
        /// <returns>
        /// The authentication result or null to continue the flow
        /// </returns>
        public virtual Task<AuthenticateResult> PreAuthenticateAsync(SignInMessage message)
        {
            return Task.FromResult<AuthenticateResult>(null);
        }

        /// <summary>
        /// This methods gets called for local authentication (whenever the user uses the username and password dialog).
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="message">The signin message.</param>
        /// <returns>
        /// The authentication result
        /// </returns>
        public virtual Task<AuthenticateResult> AuthenticateLocalAsync(string username, string password, SignInMessage message)
        {
            var query =
                from u in _users
                where u.Username == username && u.Password == password
                select u;

            var user = query.SingleOrDefault();
            if (user != null)
            {
                return Task.FromResult(new AuthenticateResult(user.Subject, GetDisplayName(user)));
            }

            return Task.FromResult<AuthenticateResult>(null);
        }

        /// <summary>
        /// This method gets called when the user uses an external identity provider to authenticate.
        /// </summary>
        /// <param name="externalUser">The external user.</param>
        /// <param name="message">The signin message.</param>
        /// <returns>
        /// The authentication result.
        /// </returns>
        public virtual Task<AuthenticateResult> AuthenticateExternalAsync(ExternalIdentity externalUser, SignInMessage message)
        {
            var query =
                from u in _users
                where
                    u.Provider == externalUser.Provider &&
                    u.ProviderId == externalUser.ProviderId
                select u;

            var user = query.SingleOrDefault();
            if (user == null)
            {
                string displayName;

                var name = externalUser.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
                if (name == null)
                {
                    displayName = externalUser.ProviderId;
                }
                else
                {
                    displayName = name.Value;
                }

                user = new User
                {
                    Subject = RandomString(16),
                    Provider = externalUser.Provider,
                    ProviderId = externalUser.ProviderId,
                    Username = displayName,
                    Claims = externalUser.Claims
                };
                _users.Add(user);
            }

            //var p = IdentityServerPrincipal.Create(user.Subject, GetDisplayName(user), Constants.AuthenticationMethods.External, user.Provider);
            //var result = new AuthenticateResult(p);
            //return Task.FromResult(result);
            return Task.FromResult(new AuthenticateResult("/core/account/eula", user.Subject, GetDisplayName(user)));
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="requestedClaimTypes">The requested claim types.</param>
        /// <returns>
        /// Claims
        /// </returns>
        public virtual Task<IEnumerable<Claim>> GetProfileDataAsync(ClaimsPrincipal subject, IEnumerable<string> requestedClaimTypes = null)
        {
            var query =
                from u in _users
                where u.Subject == subject.GetSubjectId()
                select u;
            var user = query.Single();

            var claims = new List<Claim>{
                new Claim(Constants.ClaimTypes.Subject, user.Subject),
            };

            claims.AddRange(user.Claims);
            if (requestedClaimTypes != null)
            {
                claims = claims.Where(x => requestedClaimTypes.Contains(x.Type)).ToList();
            }

            return Task.FromResult<IEnumerable<Claim>>(claims);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. during token issuance or validation)
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>
        /// true or false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">subject</exception>
        public virtual Task<bool> IsActiveAsync(ClaimsPrincipal subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            var query =
                from u in _users
                where
                    u.Subject == subject.GetSubjectId()
                select u;

            var user = query.SingleOrDefault();

            if (user == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(user.Enabled);
        }

        /// <summary>
        /// This method gets called when the user signs out (allows to cleanup resources)
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public virtual Task SignOutAsync(ClaimsPrincipal subject)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Retrieves the display name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        protected virtual string GetDisplayName(User user)
        {
            var nameClaim = user.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
            if (nameClaim != null)
            {
                return nameClaim.Value;
            }

            return user.Username;
        }

        string RandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}
