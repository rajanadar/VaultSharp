using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Decryption input.
    /// </summary>
    public class DecryptRequestOptions
    {
        /// <summary>
        ///  Integer version of the crypto key version to use for decryption. 
        ///  This is required for asymmetric keys. 
        ///  For symmetric keys, Cloud KMS will choose the correct version automatically.
        /// </summary>
        [JsonProperty("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        ///  Ciphertext to decrypt as previously returned from an encrypt operation. 
        ///  This must be base64-encoded ciphertext as previously returned from an encrypt operation.
        /// </summary>
        [JsonProperty("ciphertext")]
        public string CipherText { get; set; }

        /// <summary>
        /// Optional data that was specified during encryption of this payload.
        /// </summary>
        [JsonProperty("additional_authenticated_data")]
        public string AdditionalAuthenticatedData { get; set; }
    }
}