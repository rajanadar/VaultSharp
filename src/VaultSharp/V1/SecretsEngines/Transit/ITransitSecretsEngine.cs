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
        /// This endpoint generates a new high-entropy key and the value encrypted with the named key. 
        /// Optionally return the plaintext of the key as well.
        /// Whether plaintext is returned depends on the path; as a result, you can use Vault ACL policies to control whether a user is allowed to retrieve the plaintext value of a key. 
        /// This is useful if you want an untrusted user or operation to generate keys that are then made available to trusted users.
        /// </summary>
        /// <param name="keyType"><para>[required]</para>
        ///  Specifies the type of key to generate.
        ///  If plaintext, the plaintext key will be returned along with the ciphertext.
        ///  If wrapped, only the ciphertext value will be returned. 
        /// </param>
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
        Task<Secret<DataKeyResponse>> GenerateDataKeyAsync(string keyType, string keyName, DataKeyRequestOptions dataKeyRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint creates a new named encryption key of the specified type. The values set here cannot be changed after key creation.
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
        Task CreateEncryptionKeyAsync(string keyName, CreateKeyRequestOptions createKeyRequestOptions, string mountPoint = null);

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
        /// <returns>The information about the encryption key.</returns>
        Task<Secret<EncryptionKeyInfo>> ReadEncryptionKeyAsync(string keyName, string mountPoint = null);

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
        /// <returns>The new cyphertext.</returns>
        Task<Secret<EncryptionResponse>> RewrapAsync(string keyName, RewrapRequestOptions rewrapRequestOptions, string mountPoint = null);
    }
}