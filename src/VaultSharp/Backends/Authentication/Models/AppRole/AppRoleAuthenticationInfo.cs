using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.AppRole
{
    /// <summary>
    /// Represents the login information for the AppRole Authentication backend.
    /// </summary>
    public class AppRoleAuthenticationInfo : IAuthenticationInfo
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
                return AuthenticationBackendType.AppRole;
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
        /// Gets the role identifier.
        /// RoleID is an identifier that selects the AppRole against which the other credentials are evaluated. 
        /// When authenticating against this backend's login endpoint, the RoleID is a required argument 
        /// at all times. By default, RoleIDs are unique UUIDs, which allow them to serve as secondary 
        /// secrets to the other credential information. 
        /// However, they can be set to particular values to match introspected information by the 
        /// client (for instance, the client's domain name).
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public string RoleId { get; }

        /// <summary>
        /// Gets the secret identifier.
        /// SecretID is a credential that is required by default for any login and is intended to always be secret.
        /// (For advanced usage, requiring a SecretID can be disabled via an AppRole's bind_secret_id parameter, 
        /// allowing machines with only knowledge of the RoleID, or matching other set constraints, 
        /// to fetch a token). 
        /// SecretIDs can be created against an AppRole either via generation of a 
        /// 128-bit purely random UUID by the role itself (Pull mode) or via specific, 
        /// custom values (Push mode). Similarly to tokens, SecretIDs have properties like usage-limit, 
        /// TTLs and expirations.
        /// </summary>
        /// <value>
        /// The secret identifier.
        /// </value>
        public string SecretId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppRoleAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="roleId">
        /// The role identifier.
        /// RoleID is an identifier that selects the AppRole against which the other credentials are evaluated. 
        /// When authenticating against this backend's login endpoint, the RoleID is a required argument 
        /// at all times. By default, RoleIDs are unique UUIDs, which allow them to serve as secondary 
        /// secrets to the other credential information. 
        /// However, they can be set to particular values to match introspected information by the 
        /// client (for instance, the client's domain name).
        /// </param>
        /// <param name="secretId">
        /// The secret identifier.
        /// SecretID is a credential that is required by default for any login and is intended to always be secret.
        /// (For advanced usage, requiring a SecretID can be disabled via an AppRole's bind_secret_id parameter, 
        /// allowing machines with only knowledge of the RoleID, or matching other set constraints, 
        /// to fetch a token). 
        /// SecretIDs can be created against an AppRole either via generation of a 
        /// 128-bit purely random UUID by the role itself (Pull mode) or via specific, 
        /// custom values (Push mode). Similarly to tokens, SecretIDs have properties like usage-limit, 
        /// TTLs and expirations.
        /// </param>
        public AppRoleAuthenticationInfo(string roleId, string secretId = null) : this(AuthenticationBackendType.AppRole.Type, roleId, secretId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppRoleAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleId">
        /// The role identifier.
        /// RoleID is an identifier that selects the AppRole against which the other credentials are evaluated. 
        /// When authenticating against this backend's login endpoint, the RoleID is a required argument 
        /// at all times. By default, RoleIDs are unique UUIDs, which allow them to serve as secondary 
        /// secrets to the other credential information. 
        /// However, they can be set to particular values to match introspected information by the 
        /// client (for instance, the client's domain name).
        /// </param>
        /// <param name="secretId">
        /// The secret identifier.
        /// SecretID is a credential that is required by default for any login and is intended to always be secret.
        /// (For advanced usage, requiring a SecretID can be disabled via an AppRole's bind_secret_id parameter, 
        /// allowing machines with only knowledge of the RoleID, or matching other set constraints, 
        /// to fetch a token). 
        /// SecretIDs can be created against an AppRole either via generation of a 
        /// 128-bit purely random UUID by the role itself (Pull mode) or via specific, 
        /// custom values (Push mode). Similarly to tokens, SecretIDs have properties like usage-limit, 
        /// TTLs and expirations.
        /// </param>
        public AppRoleAuthenticationInfo(string mountPoint, string roleId, string secretId = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleId, "roleId");

            MountPoint = mountPoint;
            RoleId = roleId;
            SecretId = secretId;
        }
    }
}