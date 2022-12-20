using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.AppRole.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AppRole
{
    /// <summary>
    /// Non login operations.
    /// </summary>
    public interface IAppRoleAuthMethod
    {
        /// <summary>
        /// This endpoint returns a list of available roles. 
        /// Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.
        /// </param>  
        /// <returns>The role names.</returns>
        Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Creates a new AppRole or updates an existing AppRole. 
        /// This endpoint supports both create and update capabilities. 
        /// There can be one or more constraints enabled on the role. 
        /// It is required to have at least one of them enabled while creating or updating a role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to create. </param>
        /// <param name="appRoleRoleModel"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, AppRoleRoleModel appRoleRoleModel, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Reads the properties of an existing AppRole.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>Metadata of Named AppRole</returns>
        Task<Secret<AppRoleRoleModel>> ReadRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes an existing AppRole from the method.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.
        /// </param>        
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Reads the RoleID of an existing AppRole.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>RoleId of Named AppRole</returns>
        Task<Secret<RoleIdInfo>> GetRoleIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the RoleID of an existing AppRole to a custom value.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to create. </param>
        /// <param name="roleIdInfo"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task<Secret<RoleIdInfo>> WriteRoleIdAsync(string roleName, RoleIdInfo roleIdInfo, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Generates and issues a new SecretID on an existing AppRole. 
        /// Similar to tokens, the response will also contain a 
        /// secret_id_accessor value which can be used to read the properties 
        /// of the SecretID without divulging the SecretID itself, and also to 
        /// delete the SecretID from the AppRole.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="secretIdRequestOptions"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The secret id info</returns>
        Task<Secret<SecretIdInfo>> PullNewSecretIdAsync(string roleName, PullSecretIdRequestOptions secretIdRequestOptions = null, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Lists the accessors of all the SecretIDs issued against the AppRole. 
        /// This includes the accessors for "custom" SecretIDs as well.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>The secret accessors.</returns>
        Task<Secret<ListInfo>> ReadAllSecretIdAccessorsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Reads out the properties of a SecretID.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="secretId">The secret id.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>Secret Id properties</returns>
        Task<Secret<SecretIdInfo>> ReadSecretIdInfoAsync(string roleName, string secretId, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Destroy an AppRole secret ID.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="secretId">The secret id.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>The task</returns>
        Task DestroySecretIdAsync(string roleName, string secretId, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Reads out the properties of a SecretID by accessor.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="secretIdAccessor">The secret id accessor.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>Secret Id properties</returns>
        Task<Secret<SecretIdInfo>> ReadSecretIdInfoByAccessorAsync(string roleName, string secretIdAccessor, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Destroy an AppRole secret id by Accessor.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="secretIdAccessor">The secret id accessor.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>The task</returns>
        Task DestroySecretIdByAccessorAsync(string roleName, string secretIdAccessor, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Assigns a "custom" SecretID against an existing AppRole. 
        /// This is used in the "Push" model of operation.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="secretIdRequestOptions"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The secret id info</returns>
        Task<Secret<SecretIdInfo>> PushNewSecretIdAsync(string roleName, PushSecretIdRequestOptions secretIdRequestOptions = null, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the policies attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The policy value.</returns>
        Task<Secret<List<string>>> ReadRolePoliciesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the policy attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="policies">The policy to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRolePoliciesAsync(string roleName, List<string> policies, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the policy attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRolePoliciesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the secret-id-num-uses attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The policy value.</returns>
        Task<Secret<int>> ReadRoleSecretIdNumberOfUsesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the secret-id-num-uses attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="secretIdNumberOfUses">The policy to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleSecretIdNumberOfUsesAsync(string roleName, int secretIdNumberOfUses, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the secret-id-num-uses attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleSecretIdNumberOfUsesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the secret-id-ttl attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The secret-id-ttl value.</returns>
        Task<Secret<int>> ReadRoleSecretIdTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the secret-id-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="secretIdTimeToLive">The secret-id-ttl to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleSecretIdTimeToLiveAsync(string roleName, int secretIdTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the secret-id-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleSecretIdTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the token-ttl attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The token-ttl value.</returns>
        Task<Secret<int>> ReadRoleTokenTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the token-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="tokenTimeToLive">The token-ttl to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleTokenTimeToLiveAsync(string roleName, int tokenTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the token-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleTokenTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the token-max-ttl attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The token-max-ttl value.</returns>
        Task<Secret<int>> ReadRoleTokenMaximumTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the token-max-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="tokenMaximumTimeToLive">The token-max-ttl to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleTokenMaximumTimeToLiveAsync(string roleName, int tokenMaximumTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the token-max-ttl attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleTokenMaximumTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the bind-secret-id attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The bind-secret-id value.</returns>
        Task<Secret<bool>> ReadRoleBindSecretIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the bind-secret-id attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="bindSecretId">The bind-secret-id to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleBindSecretIdAsync(string roleName, bool bindSecretId, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the bind-secret-id attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleBindSecretIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the secret-id-bound-cidrs attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The secret-id-bound-cidrs value.</returns>
        Task<Secret<List<string>>> ReadRoleSecretIdBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the secret-id-bound-cidrs attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="secretIdBoundCIDRs">The secret-id-bound-cidrs to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleSecretIdBoundCIDRsAsync(string roleName, List<string> secretIdBoundCIDRs, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the secret-id-bound-cidrs attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleSecretIdBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the token-bound-cidrs attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The token-bound-cidrs value.</returns>
        Task<Secret<List<string>>> ReadRoleTokenBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the token-bound-cidrs attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="tokenBoundCIDRs">The token-bound-cidrs to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRoleTokenBoundCIDRsAsync(string roleName, List<string> tokenBoundCIDRs, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the token-bound-cidrs attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRoleTokenBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the period attribute of the Role entity.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The period value.</returns>
        Task<Secret<int>> ReadRolePeriodAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Updates the period attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="period">The period to write.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task WriteRolePeriodAsync(string roleName, int period, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Deletes the period attribute of the Role entity
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        Task DeleteRolePeriodAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Performs some maintenance tasks to clean up invalid entries that may remain in the token store.
        /// Generally, running this is not needed unless upgrade notes or support personnel suggest it. 
        /// This may perform a lot of I/O to the storage method so should be used sparingly.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param> 
        /// <returns>The secret with warning message.</returns>
        Task<Secret<Dictionary<string, object>>> TidyTokensAsync(string mountPoint = AuthMethodDefaultPaths.AppRole);
    }
}
