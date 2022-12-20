using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.AliCloud.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AliCloud
{
    /// <summary>
    /// Non login operations.
    /// </summary>
    public interface IAliCloudAuthMethod
    {
        /// <summary>
        /// Registers a role. 
        /// Only entities using the role registered using this endpoint will 
        /// be able to perform the login operation.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to create. </param>
        /// <param name="createAliCloudRoleModel"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, CreateAliCloudRoleModel createAliCloudRoleModel, string mountPoint = AuthMethodDefaultPaths.AliCloud);

        /// <summary>
        /// Returns the previously registered role configuration.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AliCloud" />
        /// Provide a value only if you have customized the mount point.
        /// </param>        
        /// <returns>The role details</returns>
        Task<Secret<AliCloudRoleModel>> ReadRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AliCloud);

        /// <summary>
        /// This endpoint returns a list of available roles. 
        /// Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AliCloud" />
        /// Provide a value only if you have customized the mount point.
        /// </param>  
        /// <returns>The role names.</returns>
        Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = AuthMethodDefaultPaths.AliCloud);

        /// <summary>
        /// Deletes the previously registered role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Auth backend. Defaults to <see cref="AuthMethodDefaultPaths.AliCloud" />
        /// Provide a value only if you have customized the mount point.
        /// </param>        
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AliCloud);
    }
}
