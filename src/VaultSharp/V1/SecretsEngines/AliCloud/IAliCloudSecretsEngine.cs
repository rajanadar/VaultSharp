using System.Threading.Tasks;

using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.AliCloud.Models;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    /// <summary>
    /// The AliCloud Secrets Engine.
    /// </summary>
    public interface IAliCloudSecretsEngine
    {
        /// <summary>
        /// This endpoint configures the root RAM credentials to communicate with AliCloud. 
        /// To use instance metadata, leave the static credential configuration unset.
        /// </summary>
        /// <param name="createRootCredentialsConfigModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task ConfigureRootCredentialsAsync(CreateRootCredentialsConfigModel createRootCredentialsConfigModel = null, string mountPoint = null);

        /// <summary>
        /// Reads the Connection details.
        /// </summary>       
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The config</returns>
        Task<Secret<RootCredentialsConfigModel>> ReadRootCredentialsConfigAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// The role endpoint configures how Vault will generate credentials for users of each role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="createAliCloudRoleModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, CreateAliCloudRoleModel createAliCloudRoleModel, string mountPoint = null);

        /// <summary>
        /// This endpoint queries an existing role by the given name.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the  mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<AliCloudRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint lists all existing roles in the secrets engine.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of role names.</returns>
        Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Generates a dynamic AliCloud RAM credential based on the named role.
        /// </summary>
        /// <param name="aliCloudRoleName"><para>[required]</para>
        /// Name of the AliCloud role.</param>
        /// <param name="aliCloudMountPoint"><para>[optional]</para>
        /// The mount point for the AliCloud backend. Defaults to <see cref="SecretsEngineMountPoints.AliCloud" />
        /// Provide a value only if you have customized the AliCloud mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AliCloudCredentials" /> as the data.
        /// </returns>
        Task<Secret<AliCloudCredentials>> GetCredentialsAsync(string aliCloudRoleName, string aliCloudMountPoint = null, string wrapTimeToLive = null);
    }
}