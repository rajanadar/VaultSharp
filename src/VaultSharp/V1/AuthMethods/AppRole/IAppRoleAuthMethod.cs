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
        /// <param name="createAppRoleRoleModel"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AppRole" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, CreateAppRoleRoleModel createAppRoleRoleModel, string mountPoint = AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Reads the properties of an existing AppRole.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>Metadata of Named AppRole</returns>
        Task<Secret<AppRoleInfo>> ReadRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole);

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
    }
}
