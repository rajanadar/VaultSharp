using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.LDAP
{
    /// <summary>
    /// Represents the login information for the LDAP Authentication backend.
    /// </summary>
    public class LDAPAuthenticationInfo : IAuthenticationInfo
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
                return AuthenticationBackendType.LDAP;
            }
        }

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
        /// Gets the multi factor passcode.
        /// </summary>
        /// <value>
        /// The multi factor passcode.
        /// </value>
        public string MultiFactorPasscode { get; }

        /// <summary>
        /// Gets the multi factor method.
        /// </summary>
        /// <value>
        /// The multi factor method.
        /// </value>
        public string MultiFactorMethod { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LDAPAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public LDAPAuthenticationInfo(string username, string password) : this(AuthenticationBackendType.LDAP.Type, username, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LDAPAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="multiFactorPasscode">The multi factor passcode.</param>
        /// <param name="multiFactorMethod">The multi factor method.</param>
        public LDAPAuthenticationInfo(string mountPoint, string username, string password, string multiFactorPasscode = null,
            string multiFactorMethod = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");
            Checker.NotNull(password, "password");

            MountPoint = mountPoint;
            Username = username;
            Password = password;
            MultiFactorPasscode = multiFactorPasscode;
            MultiFactorMethod = multiFactorMethod;
        }
    }
}