using System.Threading.Tasks;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.ActiveDirectory.Models;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    /// <summary>
    /// Provides Active Directory APIs for Library Management
    /// </summary>
    public interface IActiveDirectoryLibrary
    {
        /// <summary>
        /// Writes the sets of service accounts that Vault will offer for check-out.
        /// When adding a service account to the library, Vault verifies it already exists in Active Directory.
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the set.</param>
        /// <param name="createServiceAccountSetModel">The request</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task WriteServiceAccountSetAsync(string setName, CreateServiceAccountSetModel createServiceAccountSetModel, string mountPoint = null);

        /// <summary>
        /// This endpoint queries an existing set
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the set.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the  mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role info.</returns>
        Task<Secret<ServiceAccountSetModel>> ReadServiceAccountSetAsync(string setName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint lists all existing sets in the secrets engine.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of names.</returns>
        Task<Secret<ListInfo>> ReadAllServiceAccountSetsAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a set
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the set.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteServiceAccountSetAsync(string setName, string mountPoint = null);
    }
}