using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.ActiveDirectory.Models;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    /// <summary>
    /// The Active Directory Secrets Engine.
    /// </summary>
    public interface IActiveDirectorySecretsEngine
    {
        IActiveDirectoryLibrary Library { get;}

        /// <summary>
        /// The config endpoint configures the LDAP connection and binding parameters, 
        /// as well as the password rotation configuration.
        /// At present, this endpoint does not confirm that the provided AD credentials are 
        /// valid AD credentials with proper permissions.
        /// </summary>
        /// <param name="createConnectionConfigModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task ConfigureConnectionAsync(CreateConnectionConfigModel createConnectionConfigModel, string mountPoint = null);

        /// <summary>
        /// Reads the LDAP Connection details.
        /// </summary>       
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The config</returns>
        Task<Secret<ConnectionConfigModel>> ReadConnectionAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes the LDAP Config
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteConnectionAsync(string mountPoint = null);

        /// <summary>
        /// Writes a role for Vault to manage the passwords for individual service accounts.
        /// When adding a role, Vault verifies its associated service account exists.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="createActiveDirectoryRoleModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task WriteRoleAsync(string roleName, CreateActiveDirectoryRoleModel createActiveDirectoryRoleModel, string mountPoint = null);

        /// <summary>
        /// This endpoint queries an existing role by the given name.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the  mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<ActiveDirectoryRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint lists all existing roles in the secrets engine.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
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
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Offers the credential information for a given role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="ActiveDirectoryCredentials" /> as the data.
        /// </returns>
        Task<Secret<ActiveDirectoryCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Rotate the Bind Password to a new one known only to Vault.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>
        /// Generally, rotate-root returns a 204. 
        /// However, if rotate-root is already in progress, it may return a 200 with a 
        /// warning that root credential rotation is already in progress.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> RotateRootCredentialsAsync(string mountPoint = null);

        /// <summary>
        /// Read the status of Rotate action.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>
        /// Generally, returns a 204. 
        /// However, if rotate-root is already in progress, it may return a 200 with a 
        /// warning that root credential rotation is already in progress.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadRotateRootCredentialsStatusAsync(string mountPoint = null);

        /// <summary>
        /// Manually rotate the password of a managed Active Directory service account.
        /// </summary>
        /// <param name="roleName">
        /// [required]
        /// The role of the service account.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>
        /// Generally, rotate returns a 204. 
        /// However, if rotate is already in progress, it may return a 200 with a 
        /// warning that rotation is already in progress.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> RotateRoleCredentialsAsync(string roleName, string mountPoint = null);
    }
}