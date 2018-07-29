using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.UserPass
{
    /// <summary>
    /// Represents the login information for the UserPass Authentication backend.
    /// </summary>
    public class UserPassAuthMethodInfo : IAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public AuthMethodType AuthMethodType => AuthMethodType.UserPass;

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
        /// Initializes a new instance of the <see cref="UserPassAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public UserPassAuthMethodInfo(string username, string password) : this(AuthMethodType.UserPass.Type, username, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPassAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public UserPassAuthMethodInfo(string mountPoint, string username, string password)
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