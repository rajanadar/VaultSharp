using System;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Custom
{
    /// <summary>
    /// Represents the login information for the Custom Authentication backend.
    /// </summary>
    public class CustomAuthMethodInfo : AbstractAuthMethodInfo
    {
        private readonly Lazy<AuthMethodType> _lazyAuthMethodType;

        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType
        {
            get { return _lazyAuthMethodType.Value; }
        }

        /// <summary>
        /// Gets the authentication token asynchronous delegate.
        /// </summary>
        /// <value>
        /// The authentication token asynchronous delegate.
        /// </value>
        public Func<Task<string>> AuthenticationTokenAsyncDelegate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the unknown authentication backend type not supported by this library yet. e.g. liveid, facebook etc.</param>
        /// <param name="authenticationTokenAsyncDelegate">The authentication token asynchronous delegate.</param>
        public CustomAuthMethodInfo(string type, Func<Task<string>> authenticationTokenAsyncDelegate)
        {
            Checker.NotNull(type, "type");
            Checker.NotNull(authenticationTokenAsyncDelegate, "authenticationTokenAsyncDelegate");

            _lazyAuthMethodType = new Lazy<AuthMethodType>(() => new AuthMethodType(type));
            AuthenticationTokenAsyncDelegate = authenticationTokenAsyncDelegate;
        }
    }
}