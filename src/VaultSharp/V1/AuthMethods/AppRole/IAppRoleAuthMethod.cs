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
        /// <returns></returns>
        Task<Secret<AppRoleInfo>> ReadRoleAsync(string roleName, string mountPoint = "approle");
    }
}
