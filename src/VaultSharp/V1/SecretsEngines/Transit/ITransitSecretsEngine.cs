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
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineDefaultPaths.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with cipher text.
        /// </returns>
        Task<Secret<EncryptionResponse>> EncryptAsync(string keyName, EncryptRequestOptions encryptRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit, string wrapTimeToLive = null);

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
        /// The mount point for the Transit backend. Defaults to <see cref="SecretsEngineDefaultPaths.Transit" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with plain text.
        /// </returns>
        Task<Secret<DecryptionResponse>> DecryptAsync(string keyName, DecryptRequestOptions decryptRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit, string wrapTimeToLive = null);
    }
}