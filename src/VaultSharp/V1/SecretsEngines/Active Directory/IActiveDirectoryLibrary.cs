using System.Collections.Generic;
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

        /// <summary>
        /// Returns a credential if available.
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the service account set.</param>
        /// <param name="timeToLive">
        /// The maximum amount of time a check-out lasts before Vault automatically checks it back in. 
        /// Setting it to zero reflects an unlimited lending period. 
        /// Defaults to the set's ttl. 
        /// If the requested ttl is higher than the set's, the set's will be used.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="CheckedOutCredentials" /> as the data.
        /// </returns>
        Task<Secret<CheckedOutCredentials>> CheckoutCredentialsAsync(string setName, long? timeToLive = null, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Checks in a checked out credential.
        /// By default, check-in must be called by the same entity or client token used for check-out. 
        /// If a caller attempts to check in a service account they're not authorized to check in, 
        /// they will receive an error response. If they attempt to check in a service account 
        /// they are authorized to check in, but it's already checked in, they will receive a 
        /// successful response but the account will not be included in the check_ins listed. 
        /// check_ins shows which service accounts were checked in by this particular call.
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the service account set.</param>
        /// <param name="serviceAccountNames">
        /// The names of all the service accounts to be checked in. May be omitted if only one is checked out.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="CheckedInAccounts" /> as the data.
        /// </returns>
        Task<Secret<CheckedInAccounts>> CheckinCredentialsAsync(string setName, List<string> serviceAccountNames = null, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Force checks in a checked out credential.
        /// Access to this endpoint should only be granted to highly privileged Vault users, 
        /// like Vault operators.
        /// If they attempt to check in a service account 
        /// they are authorized to check in, but it's already checked in, they will receive a 
        /// successful response but the account will not be included in the check_ins listed. 
        /// check_ins shows which service accounts were checked in by this particular call.
        /// </summary>
        /// <param name="setName"><para>[required]</para>
        /// Name of the service account set.</param>
        /// <param name="serviceAccountNames">
        /// The names of all the service accounts to be checked in. May be omitted if only one is checked out.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="CheckedInAccounts" /> as the data.
        /// </returns>
        Task<Secret<CheckedInAccounts>> ForceCheckinCredentialsAsync(string setName, List<string> serviceAccountNames = null, string mountPoint = null, string wrapTimeToLive = null);
    }
}