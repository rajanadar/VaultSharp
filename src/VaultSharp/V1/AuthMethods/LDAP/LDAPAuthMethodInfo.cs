using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.LDAP
{
    /// <summary>
    /// Represents the login information for the LDAP Authentication backend.
    /// </summary>
    public class LDAPAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.LDAP;

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
        /// Initializes a new instance of the <see cref="LDAPAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public LDAPAuthMethodInfo(string username, string password) : this(AuthMethodType.LDAP.Type, username, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LDAPAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public LDAPAuthMethodInfo(string mountPoint, string username, string password)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");
            Checker.NotNull(password, "password");

            MountPoint = mountPoint;
            Username = username;
            Password = password;
        }
    }
}