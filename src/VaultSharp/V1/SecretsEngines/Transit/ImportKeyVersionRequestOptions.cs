using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Options used when importing named encryption key into existing key.
    /// </summary>
    public class ImportKeyVersionRequestOptions
    {
        /// <summary>
        /// A base64-encoded string that contains two values: 
        /// an ephemeral 256-bit AES key wrapped using the wrapping key returned by Vault 
        /// and the encryption of the import key material under the provided AES key. 
        /// The wrapped AES key should be the first 512 bytes of the ciphertext, 
        /// and the encrypted key material should be the remaining bytes. 
        /// </summary>
        [JsonProperty(PropertyName = "ciphertext")]
        public string Base64EncodedCipherText { get; set; }

        /// <summary>
        /// The hash function used for the RSA-OAEP step of creating the ciphertext.
        /// </summary>
        [JsonProperty(PropertyName = "hash_function")]
        public TransitHashFunction HashFunction { get; set; } = TransitHashFunction.SHA256;
    }
}