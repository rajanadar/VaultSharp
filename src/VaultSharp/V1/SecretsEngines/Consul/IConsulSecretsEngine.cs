using System.Threading.Tasks;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Consul.Models;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    /// <summary>
    /// Consul Secrets Engine.
    /// </summary>
    public interface IConsulSecretsEngine
    {
        /// <summary>
        /// This endpoint configures the access information for Consul. 
        /// This access information is used so that Vault can communicate with Consul and generate Consul tokens.
        /// </summary>
        /// <param name="accessConfigModel">The request object</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <returns>The task</returns>
        Task ConfigureAccesssync(AccessConfigModel accessConfigModel, string mountPoint = null);

        /// <summary>
        /// The role endpoint configures a consul role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="createConsulRoleModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, CreateConsulRoleModel createConsulRoleModel, string mountPoint = null);

        /// <summary>
        /// This endpoint queries an existing role by the given name.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the  mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<ConsulRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint lists all existing roles in the secrets engine.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
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
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Generates a dynamic Consul token based on the role definition.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="ConsulCredentials" /> as the data.
        /// </returns>
        Task<Secret<ConsulCredentials>> GetCredentialsAsync(string consulRoleName, string consulBackendMountPoint = null, string wrapTimeToLive = null);
    }
}