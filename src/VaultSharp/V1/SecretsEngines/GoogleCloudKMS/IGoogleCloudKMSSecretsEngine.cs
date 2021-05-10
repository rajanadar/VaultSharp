using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// GoogleCloud KMS Secrets Engine.
    /// </summary>
    public interface IGoogleCloudKMSSecretsEngine
    {
        /// <summary>
        /// This endpoint uses the named encryption key to encrypt arbitrary plaintext string data. 
        /// The response will be base64-encoded encrypted ciphertext.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Name of the key in Vault to use for encryption. 
        /// This key must already exist in Vault and must map back to a Google Cloud KMS key. 
        /// </param>
        /// <param name="encryptRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloudKMS" />
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
        /// This endpoint uses the named encryption key to decrypt the ciphertext string. 
        /// For symmetric key types, the provided ciphertext must come from a previous invocation of the /encrypt endpoint. 
        /// For asymmetric key types, the provided ciphertext must be from the encrypt operation 
        /// against the corresponding key version's public key.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Name of the key in Vault to use for decryption. 
        /// This key must already exist in Vault and must map back to a Google Cloud KMS key. 
        /// </param>
        /// <param name="decryptRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloudKMS" />
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
        /// This endpoint uses the named encryption key to re-encrypt the underlying cryptokey to the latest version for this ciphertext without disclosing the original plaintext value to the requestor. 
        /// This is similar to "rewrapping" in Vault's transit secrets engine.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Name of the key in Vault to use for encryption. 
        /// This key must already exist in Vault and must map back to a Google Cloud KMS key. 
        /// </param>
        /// <param name="reEncryptRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloudKMS" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with cipher text.
        /// </returns>
        Task<Secret<ReEncryptionResponse>> ReEncryptAsync(string keyName, ReEncryptRequestOptions reEncryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint uses the named encryption key to sign digest string data. 
        /// The response will include the base64-encoded signature.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Name of the key in Vault to use for signing. 
        /// This key must already exist in Vault and must map back to a Google Cloud KMS key. 
        /// </param>
        /// <param name="signatureOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloudKMS" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with signed text.
        /// </returns>
        Task<Secret<SignatureResponse>> SignAsync(string keyName, SignatureOptions signatureOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint uses the named encryption key to verify a signature and digest string data.
        /// </summary>
        /// <param name="keyName">
        /// [required]
        /// Name of the key in Vault to use for verifying. 
        /// This key must already exist in Vault and must map back to a Google Cloud KMS key. 
        /// </param>
        /// <param name="verificationOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloudKMS" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with verified text.
        /// </returns>
        Task<Secret<VerificationResponse>> VerifyAsync(string keyName, VerificationOptions verificationOptions, string mountPoint = null, string wrapTimeToLive = null);
    }
}