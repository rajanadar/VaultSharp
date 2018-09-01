
using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.AliCloud
{
    /// <summary>
    /// Represents the login information for the AliCloud Authentication backend.
    /// </summary>
    public class AliCloudAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        [JsonIgnore]
        public override AuthMethodType AuthMethodType => AuthMethodType.AliCloud;

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
        /// Gets the Base64-encoded HTTP URL used in the signed request.
        /// </summary>
        /// <value>
        /// The base 64 encoded url.
        /// </value>
        [JsonProperty("identity_request_url")]
        public string Base64EncodedIdentityRequestUrl { get; }

        /// <summary>
        /// [required]
        /// Gets the Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of 
        /// string values (though the length of that array will probably only be one).
        /// </summary>
        /// <value>
        /// The base 64 encoded headers.
        /// </value>
        [JsonProperty("identity_request_headers")]
        public string Base64EncodedIdentityRequestHeaders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliCloudAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="base64EncodedIdentityRequestUrl">
        /// [required]
        /// The Base64-encoded HTTP URL used in the signed request.
        /// </param>
        /// <param name="base64EncodedIdentityRequestHeaders">
        /// [required]
        /// The Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of 
        /// string values (though the length of that array will probably only be one).
        /// </param>       
        public AliCloudAuthMethodInfo(string roleName, string base64EncodedIdentityRequestUrl, string base64EncodedIdentityRequestHeaders)
            : this (AuthMethodType.AliCloud.Type, roleName, base64EncodedIdentityRequestUrl, base64EncodedIdentityRequestHeaders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliCloudAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="base64EncodedIdentityRequestUrl">
        /// [required]
        /// The Base64-encoded HTTP URL used in the signed request.
        /// </param>
        /// <param name="base64EncodedIdentityRequestHeaders">
        /// [required]
        /// The Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of 
        /// string values (though the length of that array will probably only be one).
        /// </param>  
        public AliCloudAuthMethodInfo(string mountPoint, string roleName, string base64EncodedIdentityRequestUrl, string base64EncodedIdentityRequestHeaders)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(base64EncodedIdentityRequestUrl, "base64EncodedIdentityRequestUrl");
            Checker.NotNull(base64EncodedIdentityRequestHeaders, "base64EncodedIdentityRequestHeaders");

            MountPoint = mountPoint;
            RoleName = roleName;
            Base64EncodedIdentityRequestUrl = base64EncodedIdentityRequestUrl;
            Base64EncodedIdentityRequestHeaders = base64EncodedIdentityRequestHeaders;
        }
    }
}