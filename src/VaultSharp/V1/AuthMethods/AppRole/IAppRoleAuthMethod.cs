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
        /// Reads the properties of an existing AppRole.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>Metadata of Named AppRole</returns>
        Task<Secret<AppRoleInfo>> ReadRoleAsync(string roleName, string mountPoint = "approle");
        /// <summary>
        /// Reads the RoleID of an existing AppRole.
        /// </summary>
        /// <param name="roleName">Name of the Role.</param>
        /// <param name="mountPoint">Mount point of the AppRole Auth method</param>
        /// <returns>RoleId of Named AppRole</returns>
        Task<Secret<RoleIdInfo>> GetRoleIdAsync(string roleName, string mountPoint = "approle");
        /// <summary>
        /// Generates and issues a new SecretID on an existing AppRole. 
        /// Similar to tokens, the response will also contain a 
        /// secret_id_accessor value which can be used to read the properties 
        /// of the SecretID without divulging the SecretID itself, and also to 
        /// delete the SecretID from the AppRole.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="mountPoint"></param>
        /// <param name="secretIdRequestOptions"></param>
        /// <returns></returns>
        Task<Secret<SecretIdInfo>> PullSecretIdAsync(string roleName, string mountPoint = "approle", SecretIdRequestOptions secretIdRequestOptions = null);
    }
}
