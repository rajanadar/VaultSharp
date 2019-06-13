using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V1
{
    /// <summary>
    /// V1 of Key Value Secrets Engine
    /// </summary>
    public interface IKeyValueSecretsEngineV1
    {
        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<T>> ReadSecretAsync<T>(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null);
        
        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null);

        /// <summary>
        /// Stores a secret at the specified location. If the value does not yet exist, the calling token must have an ACL policy granting the create capability. 
        /// If the value already exists, the calling token must have an ACL policy granting the update capability.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The value to be written.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <remarks>
        /// Unlike other secrets engines, the KV secrets engine does not enforce TTLs for expiration. 
        /// Instead, the lease_duration is a hint for how often consumers should check back for a new value. 
        /// This is commonly displayed as refresh_interval instead of lease_duration to clarify this in output.
        /// If provided a key of ttl, the KV secrets engine will utilize this value as the lease duration:
        /// Even with a ttl set, the secrets engine never removes data on its own.The ttl key is merely advisory.
        /// When reading a value with a ttl, both the ttl key and the refresh interval will reflect the value:
        /// </remarks>
        Task WriteSecretAsync(string path, IDictionary<string, object> values, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1);

        /// <summary>
        /// Stores a secret at the specified location. If the value does not yet exist, the calling token must have an ACL policy granting the create capability. 
        /// If the value already exists, the calling token must have an ACL policy granting the update capability.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The value to be written.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <remarks>
        /// Unlike other secrets engines, the KV secrets engine does not enforce TTLs for expiration. 
        /// Instead, the lease_duration is a hint for how often consumers should check back for a new value. 
        /// This is commonly displayed as refresh_interval instead of lease_duration to clarify this in output.
        /// If provided a key of ttl, the KV secrets engine will utilize this value as the lease duration:
        /// Even with a ttl set, the secrets engine never removes data on its own.The ttl key is merely advisory.
        /// When reading a value with a ttl, both the ttl key and the refresh interval will reflect the value:
        /// </remarks>
        Task WriteSecretAsync<T>(string path, T values, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1);
        
        /// <summary>
        /// Deletes the value at the specified path in Vault.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV1" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1);
    }
}