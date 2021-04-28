using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.GoogleCloud
{
    /// <summary>
    /// Represents the login information for the GoogleCloud Authentication backend.
    /// </summary>
    public class GoogleCloudAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        [JsonIgnore]
        public override AuthMethodType AuthMethodType => AuthMethodType.GoogleCloud;

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
        /// Gets the signed JSON Web Token (JWT).
        /// For iam type roles, this is a JWT signed with the signJwt method <see href="https://cloud.google.com/iam/reference/rest/v1/projects.serviceAccounts/signJwt" /> or a self-signed JWT.
        /// For gce type roles, this is an identity metadata token. <see href="https://cloud.google.com/compute/docs/instances/verifying-instance-identity#request_signature"/>
        /// </summary>
        /// <value>
        /// The jwt.
        /// </value>
        [JsonProperty("jwt")]
        public string JWT { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleCloudAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT).
        /// For iam type roles, this is a JWT signed with the signJwt method <see href="https://cloud.google.com/iam/reference/rest/v1/projects.serviceAccounts/signJwt" /> or a self-signed JWT.
        /// For gce type roles, this is an identity metadata token. <see href="https://cloud.google.com/compute/docs/instances/verifying-instance-identity#request_signature"/>
        /// </param>
        public GoogleCloudAuthMethodInfo(string roleName, string jwt)
            : this (AuthMethodType.GoogleCloud.Type, roleName, jwt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleCloudAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT).
        /// For iam type roles, this is a JWT signed with the signJwt method <see href="https://cloud.google.com/iam/reference/rest/v1/projects.serviceAccounts/signJwt" /> or a self-signed JWT.
        /// For gce type roles, this is an identity metadata token. <see href="https://cloud.google.com/compute/docs/instances/verifying-instance-identity#request_signature"/>
        /// </param>
        public GoogleCloudAuthMethodInfo(string mountPoint, string roleName, string jwt)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(jwt, "jwt");

            MountPoint = mountPoint;
            RoleName = roleName;
            JWT = jwt;
        }
    }
}
