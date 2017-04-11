using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the login information for the Token Authentication backend.
    /// </summary>
    public class TokenAuthenticationInfo : IAuthenticationInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public AuthenticationBackendType AuthenticationBackendType
        {
            get
            {
                return AuthenticationBackendType.Token;
            }
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenAuthenticationInfo" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public TokenAuthenticationInfo(string token)
        {
            Checker.NotNull(token, "token");

            Token = token;
        }
    }
}