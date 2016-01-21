using System;
using System.Threading.Tasks;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.Custom
{
    /// <summary>
    /// Represents the login information for the Custom Authentication backend.
    /// </summary>
    public class CustomAuthenticationInfo : IAuthenticationInfo
    {
        private readonly Lazy<AuthenticationBackendType> _backendType; 

        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public AuthenticationBackendType AuthenticationBackendType
        {
            get { return _backendType.Value; }
        }

        /// <summary>
        /// Gets the authentication token asynchronous delegate.
        /// </summary>
        /// <value>
        /// The authentication token asynchronous delegate.
        /// </value>
        public Func<Task<string>> AuthenticationTokenAsyncDelegate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the unknown authentication backend type not supported by this library yet. e.g. liveid, facebook etc.</param>
        /// <param name="authenticationTokenAsyncDelegate">The authentication token asynchronous delegate.</param>
        public CustomAuthenticationInfo(string type, Func<Task<string>> authenticationTokenAsyncDelegate)
        {
            Checker.NotNull(type, "type");
            Checker.NotNull(authenticationTokenAsyncDelegate, "authenticationTokenAsyncDelegate");

            _backendType = new Lazy<AuthenticationBackendType>(() => new AuthenticationBackendType(type));
            AuthenticationTokenAsyncDelegate = authenticationTokenAsyncDelegate;
        }
    }
}