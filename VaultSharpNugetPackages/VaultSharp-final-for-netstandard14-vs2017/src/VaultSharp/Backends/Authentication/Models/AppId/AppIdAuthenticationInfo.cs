using System;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.AppId
{
    /// <summary>
    /// Represents the login information for the AppId Authentication backend.
    /// </summary>
    [Obsolete("The AppId Authentication backend in Vault is now deprecated with the addition " +
              "of the new AppRole backend. There are no plans to remove it, but we encourage " +
              "using AppRole whenever possible, as it offers enhanced functionality " +
              "and can accommodate many more types of authentication paradigms.")]
    public class AppIdAuthenticationInfo : IAuthenticationInfo
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
                return AuthenticationBackendType.AppId;
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
        /// Gets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string AppId { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdAuthenticationInfo" /> class.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public AppIdAuthenticationInfo(string appId, string userId) : this(AuthenticationBackendType.AppId.Type, appId, userId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="appId">The application identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public AppIdAuthenticationInfo(string mountPoint, string appId, string userId)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(appId, "appId");
            Checker.NotNull(userId, "userId");

            MountPoint = mountPoint;
            AppId = appId;
            UserId = userId;
        }
    }
}