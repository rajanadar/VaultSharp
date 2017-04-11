using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.GitHub
{
    /// <summary>
    /// Represents the login information for the GitHub Authentication backend.
    /// </summary>
    public class GitHubAuthenticationInfo : IAuthenticationInfo
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
                return AuthenticationBackendType.GitHub;
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
        /// Gets the personal access token.
        /// </summary>
        /// <value>
        /// The personal access token.
        /// </value>
        public string PersonalAccessToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAuthenticationInfo" /> class.
        /// </summary>
        /// <param name="personalAccessToken">The personal access token.</param>
        public GitHubAuthenticationInfo(string personalAccessToken) : this(AuthenticationBackendType.GitHub.Type, personalAccessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public GitHubAuthenticationInfo(string mountPoint, string personalAccessToken)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(personalAccessToken, "personalAccessToken");

            MountPoint = mountPoint;
            PersonalAccessToken = personalAccessToken;
        }
    }
}