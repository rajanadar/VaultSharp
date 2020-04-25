using System.Collections.Generic;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.OCI
{
    /// <summary>
    /// Represents the login information for the OCI Authentication backend.
    /// </summary>
    public class OCIAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.OCI;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// Gets the name of the role against which the login is being attempted.
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public IDictionary<string, object>  RequestHeaders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OCIAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="rolename">The Name of the role against which the login is being attempted..</param>
        /// <param name="requestHeaders">Signed request headers for authenticating. 
        /// For details on signing, see https://docs.cloud.oracle.com/iaas/Content/API/Concepts/signingrequests.htm
        /// </param>
        public OCIAuthMethodInfo(string rolename, IDictionary<string, object> requestHeaders) : this(AuthMethodType.OCI.Type, rolename, requestHeaders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OCIAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="rolename">The Name of the role against which the login is being attempted..</param>
        /// <param name="requestHeaders">Signed request headers for authenticating. 
        /// For details on signing, see https://docs.cloud.oracle.com/iaas/Content/API/Concepts/signingrequests.htm
        /// </param>
        public OCIAuthMethodInfo(string mountPoint, string rolename, IDictionary<string, object> requestHeaders)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(rolename, "rolename");
            Checker.NotNull(requestHeaders, "requestHeaders");

            MountPoint = mountPoint;
            RoleName = rolename;
            RequestHeaders = requestHeaders;
        }
    }
}