using System;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

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
        /// Gets the CustomAuthInfo asynchronous delegate which includes the token and token info.
        /// </summary>
        public Func<Task<AuthInfo>> CustomAuthInfoAsyncDelegate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the unknown authentication backend type not supported by this library yet. But supported by the Vault Server.</param>
        /// <param name="customAuthInfoAsyncDelegate">The authentication token asynchronous delegate.</param>
        public CustomAuthMethodInfo(string type, Func<Task<AuthInfo>> customAuthInfoAsyncDelegate)
        {
            Checker.NotNull(type, "type");
            Checker.NotNull(customAuthInfoAsyncDelegate, nameof(customAuthInfoAsyncDelegate));

            _lazyAuthMethodType = new Lazy<AuthMethodType>(() => new AuthMethodType(type));
            CustomAuthInfoAsyncDelegate = customAuthInfoAsyncDelegate;
        }
    }
}