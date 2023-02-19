using System.Text.Json.Serialization;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.JWT
{
    /// <summary>
    /// Represents the login information for the JWT Authentication backend.
    /// </summary>
    public class JWTAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        [JsonIgnore]
        public override AuthMethodType AuthMethodType => AuthMethodType.JWT;

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
        [JsonPropertyName("role")]
        public string RoleName { get; }

        /// <summary>
        /// [required]
        /// Gets the signed JSON Web Token (JWT).
        /// </summary>
        /// <value>
        /// The jwt.
        /// </value>
        [JsonPropertyName("jwt")]
        public string JWT { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JWTAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT).
        /// </param>
        public JWTAuthMethodInfo(string roleName, string jwt)
            : this (AuthMethodType.JWT.Type, roleName, jwt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JWTAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT).
        /// </param>
        public JWTAuthMethodInfo(string mountPoint, string roleName, string jwt)
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
