using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    /// <summary>
    /// V2 of Key Value Secrets Engine
    /// </summary>
    public interface IKeyValueSecretsEngineV2
    {
        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="version"><para>[optional]</para>
        /// Specifies the version to return. If not set the latest version is returned.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValue" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<SecretData>> ReadSecretAsync(string path, int? version = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null);
        
        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="version"><para>[optional]</para>
        /// Specifies the version to return. If not set the latest version is returned.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValue" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<SecretData<T>>> ReadSecretAsync<T>(string path, int? version = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret metadata at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValue" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret metadata.
        /// </returns>
        Task<Secret<FullSecretMetadata>> ReadSecretMetadataAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null);

        /// <summary>
        /// Stores a secret at the specified location. If the value does not yet exist, the calling token must have an ACL policy granting the create capability. 
        /// If the value already exists, the calling token must have an ACL policy granting the update capability.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="data"><para>[required]</para>
        /// The value to be written.</param>
        /// <param name="checkAndSet">
        /// <para>[optional]</para>
        /// If not set the write will be allowed. If set to 0 a write will only be allowed if the key doesn’t exist. 
        /// If the index is non-zero the write will only be allowed if the key’s current version matches the version specified in the cas parameter.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
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
        Task WriteSecretAsync(string path, IDictionary<string, object> data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2);
        
        /// <summary>
        /// Stores a secret at the specified location. If the value does not yet exist, the calling token must have an ACL policy granting the create capability. 
        /// If the value already exists, the calling token must have an ACL policy granting the update capability.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="data"><para>[required]</para>
        /// The value to be written.</param>
        /// <param name="checkAndSet">
        /// <para>[optional]</para>
        /// If not set the write will be allowed. If set to 0 a write will only be allowed if the key doesn’t exist. 
        /// If the index is non-zero the write will only be allowed if the key’s current version matches the version specified in the cas parameter.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
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
        Task WriteSecretAsync<T>(string path, T data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2);

        /// <summary>
        /// Deletes the value at the specified path in Vault.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="versions">
        /// <para>[required]</para>
        /// The versions to destroy. Their data will be permanently deleted.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValueV2" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DestroySecretAsync(string path, IList<int> versions, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2);
    }
}