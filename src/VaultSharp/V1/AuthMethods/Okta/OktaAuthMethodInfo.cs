using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Okta
{
    /// <summary>
    /// Represents the login information for the Okta Authentication backend.
    /// </summary>
    public class OktaAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.Okta;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; }

        /// <summary>
        /// Gets the TOTP.
        /// </summary>
        /// <value>
        /// The TOTP.
        /// </value>
        public string TOTP { get; }

        /// <summary>
        /// Gets the Provider.
        /// </summary>
        /// <value>
        /// The Provider.
        /// </value>
        public OktaTOTPProvider? TOTPProvider { get; }

        /// <summary>
        /// Gets the Nonce.
        /// </summary>
        /// <value>
        /// The Nonce.
        /// </value>
        public string Nonce { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="totp">The totp</param>
        /// <param name="provider">The provider</param>
        /// <param name="nonce">The nonce</param>
        public OktaAuthMethodInfo(string username, string password, string totp = null, OktaTOTPProvider? provider = null, string nonce = null) 
            : this(AuthMethodType.Okta.Type, username, password, totp, provider, nonce)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="totp">The totp</param>
        /// <param name="provider">The provider</param>
        /// <param name="nonce">The nonce</param>
        public OktaAuthMethodInfo(string mountPoint, string username, string password, string totp = null, OktaTOTPProvider? provider = null, string nonce = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");
            Checker.NotNull(password, "password");

            MountPoint = mountPoint;
            Username = username;
            Password = password;
            TOTP = totp;
            TOTPProvider = provider;
            Nonce = nonce;
        }
    }
}