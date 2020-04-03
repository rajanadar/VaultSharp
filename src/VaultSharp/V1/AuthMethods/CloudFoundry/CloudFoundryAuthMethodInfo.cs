using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.CloudFoundry.Signature;

namespace VaultSharp.V1.AuthMethods.CloudFoundry
{
    public class CloudFoundryAuthMethodInfo : AbstractAuthMethodInfo
    {
        [JsonIgnore]
        public override AuthMethodType AuthMethodType => AuthMethodType.CloudFoundry;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string MountPoint { get; }

        /// <summary>
        /// [required]
        /// Gets the name of the role against which the login is being attempted.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        [JsonProperty("role")]
        public string RoleName { get; }

        /// <summary>
        /// [required]
        /// Gets the Signature for getting a token for a service account.
        /// </summary>
        /// <value>
        /// The Signature.
        /// </value>
        [JsonProperty("signature")]
        public CloudFoundrySignature Signature { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="signature">
        /// [required]
        /// Gets the Signature for getting a token for a service account.
        /// </param>
        public CloudFoundryAuthMethodInfo(string roleName, CloudFoundrySignature signature)
            : this(AuthMethodType.CloudFoundry.Type, roleName, signature)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="signature">
        /// [required]
        /// Gets the Signature for getting a token for a service account.
        /// </param>

        public CloudFoundryAuthMethodInfo(string mountPoint, string roleName, CloudFoundrySignature signature)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(signature, "signature");

            MountPoint = mountPoint;
            RoleName = roleName;
            Signature = signature;
        }
    }
}
