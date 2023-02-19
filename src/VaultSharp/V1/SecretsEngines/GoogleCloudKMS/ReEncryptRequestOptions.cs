using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// ReEncryption input.
    /// </summary>
    public class ReEncryptRequestOptions
    {
        /// <summary>
        /// Integer version of the crypto key version to use for re-encryption. 
        /// If unspecified, this defaults to the latest active crypto key version.
        /// </summary>
        [JsonPropertyName("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        ///  Ciphertext to be re-encrypted to the latest key version. 
        ///  This must be ciphertext that Vault previously generated for this named key.
        /// </summary>
        [JsonPropertyName("ciphertext")]
        public string CipherText { get; set; }

        /// <summary>
        /// Optional data that, if specified, must also be provided during decryption.
        /// </summary>
        [JsonPropertyName("additional_authenticated_data")]
        public string AdditionalAuthenticatedData { get; set; }
    }
}