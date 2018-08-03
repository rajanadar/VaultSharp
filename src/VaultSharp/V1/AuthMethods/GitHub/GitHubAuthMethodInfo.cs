using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.GitHub
{
    /// <summary>
    /// Represents the login information for the GitHub Authentication backend.
    /// </summary>
    public class GitHubAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.GitHub;

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
        /// Initializes a new instance of the <see cref="GitHubAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="personalAccessToken">The personal access token.</param>
        public GitHubAuthMethodInfo(string personalAccessToken) : this(AuthMethodType.GitHub.Type, personalAccessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public GitHubAuthMethodInfo(string mountPoint, string personalAccessToken)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(personalAccessToken, "personalAccessToken");

            MountPoint = mountPoint;
            PersonalAccessToken = personalAccessToken;
        }
    }
}