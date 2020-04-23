using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.CloudFoundry
{
    public class CloudFoundryAuthMethodInfo : AbstractAuthMethodInfo
    {
        public override AuthMethodType AuthMethodType => AuthMethodType.CloudFoundry;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// [required]
        /// Gets the name of the role against which the login is being attempted.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        public string RoleName { get; }

        /// <summary>
        /// [required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </summary>
        public string CFInstanceCertContent { get; }

        /// <summary>
        /// [required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_KEY.
        /// </summary>
        public string CFInstanceKeyContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="instanceCertContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </param>
        /// <param name="instanceKeyContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_KEY.
        /// </param>
        public CloudFoundryAuthMethodInfo(string roleName, string instanceCertContent, string instanceKeyContent)
            : this(AuthMethodType.CloudFoundry.Type, roleName, instanceCertContent, instanceKeyContent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="instanceCertContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </param>
        /// <param name="instanceKeyContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_KEY.
        /// </param>
        public CloudFoundryAuthMethodInfo(string mountPoint, string roleName, string instanceCertContent, string instanceKeyContent)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(instanceCertContent, "instanceCertContent");
            Checker.NotNull(instanceKeyContent, "instanceKeyContent");

            MountPoint = mountPoint;
            RoleName = roleName;
            CFInstanceCertContent = instanceCertContent;
            CFInstanceKeyContent = instanceKeyContent;
        }
    }
}
