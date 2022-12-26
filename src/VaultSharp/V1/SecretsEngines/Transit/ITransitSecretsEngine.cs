
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The Transit Secrets Engine.
    /// </summary>
    public interface ITransitSecretsEngine
    {
        /// <summary>
        /// This endpoint creates a new named encryption key of the specified type. 
        /// The values set here cannot be changed after key creation.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="createKeyRequestOptions"></param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task CreateEncryptionKeyAsync(string keyName, CreateKeyRequestOptions createKeyRequestOptions = null, string mountPoint = null);

        /// <summary>
        /// This endpoint imports existing key material into a new transit-managed encryption key. 
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to create.
        /// </param>
        /// <param name="importKeyRequestOptions"></param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task ImportEncryptionKeyAsync(string keyName, ImportKeyRequestOptions importKeyRequestOptions, string mountPoint = null);

        /// <summary>
        /// This endpoint imports new key material into an existing imported key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to create.
        /// </param>
        /// <param name="importKeyVersionRequestOptions">The request option</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task ImportEncryptionKeyVersionAsync(string keyName, ImportKeyVersionRequestOptions importKeyVersionRequestOptions, string mountPoint = null);

        /// <summary>
        /// This endpoint is used to retrieve the wrapping key to use for importing keys. 
        /// The returned key will be a 4096-bit RSA public key.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>        
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The public key</returns>
        Task<Secret<TransitWrappingKeyModel>> ReadWrappingKeyAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns information about a named encryption key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The information about the encryption key.</returns>
        Task<Secret<EncryptionKeyInfo>> ReadEncryptionKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of keys. Only the key names are returned.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of key names.</returns>
        Task<Secret<ListInfo>> ReadAllEncryptionKeysAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes a named encryption key. It will no longer be possible to decrypt any data encrypted with the named key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task DeleteEncryptionKeyAsync(string keyName, string mountPoint = null);

        /// <summary>
        /// This endpoint allows tuning configuration values for a given key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="updateKeyRequestOptions"><para>[required]</para>
        /// The options for the request.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task UpdateEncryptionKeyConfigAsync(string keyName, UpdateKeyRequestOptions updateKeyRequestOptions, string mountPoint = null);

        /// <summary>
        /// This endpoint rotates the version of the named key. After rotation, new plaintext requests will be encrypted with the new version of the key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task RotateEncryptionKeyAsync(string keyName, string mountPoint = null);

        /// <summary>
        /// This endpoint returns the named key.
        /// The `keys` object shows the value of the key for each version.
        /// If version is specified, the specific version will be returned.
        /// If latest is provided as the version, the current key will be provided.
        /// Depending on the type of key, different information may be returned.
        /// The key must be exportable to support this operation and the version must still be valid.
        /// </summary>
        /// <param name="keyType"><para>[required]</para>
        /// Specifies the type of the key to export.
        /// </param>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to read information about.
        /// </param>
        /// <param name="version"><para>[optional]</para>
        /// Specifies the version of the key to read.
        /// If omitted, all versions of the key will be returned.
        /// This is specified as part of the URL.
        /// If the version is set to latest, the current key will be returned.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task<Secret<ExportedKeyInfo>> ExportKeyAsync(TransitKeyCategory keyType, string keyName, string version = null, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Encrypts the provided plaintext using the named key.
        /// This path supports the create and update policy capabilities as follows:
        /// if the user has the create capability for this endpoint in their policies,
        /// and the key does not exist, it will be upserted with default values
        /// (whether the key requires derivation depends on whether the context parameter is empty or not).
        /// If the user only has update capability and the key does not exist, an error will be returned.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Specifies the name of the encryption key to encrypt against.
        /// </param>
        /// <param name="encryptRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with cipher text.
        /// </returns>
        Task<Secret<EncryptionResponse>> EncryptAsync(string keyName, EncryptRequestOptions encryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Decrypts the provided ciphertext using the named key.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Specifies the name of the encryption key to decrypt against.
        /// </param>
        /// <param name="decryptRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with plain text.
        /// </returns>
        Task<Secret<DecryptionResponse>> DecryptAsync(string keyName, DecryptRequestOptions decryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint rewraps the provided ciphertext using the latest version of the named key.
        /// Because this never returns plaintext, it is possible to delegate this functionality to
        /// untrusted users or scripts.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>vz
        /// <param name="rewrapRequestOptions"><para>[required]</para>
        /// The options for the request.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The new cyphertext.</returns>
        Task<Secret<EncryptionResponse>> RewrapAsync(string keyName, RewrapRequestOptions rewrapRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint generates a new high-entropy key and the value encrypted with the named key.
        /// Optionally return the plaintext of the key as well.
        /// Whether plaintext is returned depends on the path; as a result, you can use Vault ACL policies to control whether a user is allowed to retrieve the plaintext value of a key.
        /// This is useful if you want an untrusted user or operation to generate keys that are then made available to trusted users.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use to encrypt the datakey.
        /// </param>
        /// <param name="dataKeyRequestOptions"></param>
        /// The Options
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with data key response.
        /// </returns>
        Task<Secret<DataKeyResponse>> GenerateDataKeyAsync(string keyName, DataKeyRequestOptions dataKeyRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns high-quality random bytes of the specified length.
        /// </summary>
        /// <param name="randomOptions"><para>[required]</para>
        /// The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>A set of random bytes in the requested output format.</returns>
        Task<Secret<RandomBytesResponse>> GenerateRandomBytesAsync(RandomBytesRequestOptions randomOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns the cryptographic hash of given data using the specified algorithm.
        /// </summary>
        /// <param name="hashOptions"><para>[required]</para>
        /// The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The cryptographic hash(es) of the input data.</returns>
        Task<Secret<HashResponse>> HashDataAsync(HashRequestOptions hashOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns the digest of given data using the specified hash algorithm 
        /// and the named key.
        /// The key can be of any type supported by transit; 
        /// the raw key will be marshaled into bytes to be used for the HMAC function.
        /// If the key is of a type that supports rotation, the latest (current) version will be used.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key.</param>
        /// <param name="hmacOptions"><para>[required]</para>
        /// The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The HMAC digest of the requested data.</returns>
        Task<Secret<HmacResponse>> GenerateHmacAsync(string keyName, HmacRequestOptions hmacOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns the cryptographic signature of the given data using the 
        /// named key and the specified hash algorithm.
        /// The key must be of a type that supports signing.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to backup.</param>
        /// <param name="signOptions"><para>[required]</para>
        /// The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The cryptographic signature of the requested data.</returns>
        Task<Secret<SigningResponse>> SignDataAsync(string keyName, SignRequestOptions signOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns whether the provided signature is valid for the given data.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to backup.</param>
        /// <param name="verifyOptions"><para>[required]</para>
        /// The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>If the given signature is valid for the given data.</returns>
        Task<Secret<VerifyResponse>> VerifySignedDataAsync(string keyName, VerifyRequestOptions verifyOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns a plaintext backup of a named key.
        /// The backup contains all the configuration data and keys of all the versions
        /// along with the HMAC key.
        /// The response from this endpoint can be used with the <see cref="RestoreKeyAsync">restore</see> endpoint to restore the key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to backup.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The backed up key data for secure storage or a subsequent restore operation.</returns>
        Task<Secret<BackupKeyResponse>> BackupKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint restores the backup as a named key.
        /// This will restore the key configurations and all the versions of the named key
        /// along with HMAC keys.
        /// The input to this endpoint should be the output of <see cref="BackupKeyAsync">backup</see> endpoint.
        /// </summary>
        /// <remarks>
        /// For safety, by default the backend will refuse to restore to an existing key.
        /// If you want to reuse a key name, it is recommended you delete the key before restoring.
        /// It is a good idea to attempt restoring to a different key name first to verify that the operation successfully completes.
        /// </remarks>
        /// <param name="backupData"><para>[required]</para>
        /// Backed up key data to be restored.</param>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to backup.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task RestoreKeyAsync(RestoreKeyRequestOptions backupData, string keyName = null, string mountPoint = null);

        /// <summary>
        /// This endpoint trims older key versions setting a minimum version
        /// for the keyring. Once trimmed, previous versions of the key cannot be recovered.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the encryption key to use. This is specified as part of the URL.
        /// </param>
        /// <param name="trimKeyRequestOptions">
        /// The options for the request.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task TrimKeyAsync(string keyName, TrimKeyRequestOptions trimKeyRequestOptions, string mountPoint = null);

        /// <summary>
        /// This endpoint is used to configure the transit engine's cache.
        /// Note that configuration changes will not be applied until the transit plugin
        /// is reloaded which can be achieved using the
        /// <see cref="SystemBackend.Plugin.PluginProvider.ReloadBackendsAsync">/sys/plugins/reload/backend</see> endpoint.
        /// </summary>
        /// <param name="cacheOptions">The options for the request.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <returns>Nothing is returned. No error means the operation was successful.</returns>
        Task SetCacheConfigAsync(CacheConfigRequestOptions cacheOptions, string mountPoint = null);

        /// <summary>
        /// This endpoint retrieves configurations for the transit engine's cache.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineMountPoints.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The current size of the transit engine's cache.</returns>
        Task<Secret<CacheResponse>> ReadCacheConfigAsync(string mountPoint = null, string wrapTimeToLive = null);
    }
}