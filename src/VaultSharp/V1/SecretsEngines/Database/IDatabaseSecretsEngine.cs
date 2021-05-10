using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Database Secrets Engine.
    /// </summary>
    public interface IDatabaseSecretsEngine
    {
        /// <summary>
        /// This endpoint creates or updates a role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to create. </param>
        /// <param name="role"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task CreateRoleAsync(string roleName, Role role, string mountPoint = null);

        /// <summary>
        /// This endpoint queries the role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to read. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<Role>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns a list of available roles. 
        /// Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role names.</returns>
        Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes the role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to delete. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <returns>The task.</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role to create credentials against.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint creates or updates a static role definition. 
        /// Static Roles are a 1-to-1 mapping of a Vault Role to a user in a database which are automatically 
        /// rotated based on the configured rotation_period. 
        /// Not all databases support Static Roles, please see the database-specific documentation.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to create. </param>
        /// <param name="staticRole"><para>[required]</para>
        /// Specifies the request options. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>        
        /// <returns>The task</returns>
        Task CreateStaticRoleAsync(string roleName, StaticRole staticRole, string mountPoint = null);

        /// <summary>
        /// This endpoint queries the static role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to read. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<StaticRole>> ReadStaticRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns a list of available static roles. 
        /// Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role names.</returns>
        Task<Secret<ListInfo>> ReadAllStaticRolesAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes the static role definition and revokes the database user.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to delete. </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>   
        /// <returns>The task.</returns>
        Task DeleteStaticRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Generates a new set of STATIC credentials based on the named role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the static role to get credentials for.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="StaticCredentials" /> as the data.
        /// </returns>
        Task<Secret<StaticCredentials>> GetStaticCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint is used to rotate the Static Role credentials stored for a given role name. 
        /// While Static Roles are rotated automatically by Vault at configured rotation periods, 
        /// users can use this endpoint to manually trigger a rotation to change the stored password and 
        /// reset the TTL of the Static Role's password.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the static role to rotate credentials for.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Database backend. Defaults to <see cref="SecretsEngineMountPoints.Database" />
        /// Provide a value only if you have customized the mount point.</param>
        Task RotateStaticCredentialsAsync(string roleName, string mountPoint = null);
    }
}