using System.Net;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents a single Decryption item.
    /// </summary>
    public class DecryptionItem
    {
        /// <summary>
        /// [required]
        /// Specifies cipher text to be decrypted.
        /// </summary>
        [JsonPropertyName("ciphertext")]
        public string CipherText { get; set; }

        /// <summary>
        /// [required]
        ///  Specifies the base64 encoded context for key derivation. 
        ///  This is required if key derivation is enabled for this key.
        /// </summary>
        [JsonPropertyName("context")]
        public string Base64EncodedContext { get; set; }

        /// <summary>
        /// [optional]
        /// Specifies the base64 encoded nonce value. 
        /// This must be provided if convergent encryption is enabled for this key and the key was generated with Vault 0.6.1. 
        /// Not required for keys created in 0.6.2+. 
        /// </summary>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Ordinarily, if a batch item fails to encrypt due to a bad input, 
        /// but other batch items succeed, the HTTP response code is 400 (Bad Request). 
        /// Some applications may want to treat partial failures differently. 
        /// Providing the parameter returns the given response code integer instead of 
        /// a failed status code in this case. If all values fail an error code is still returned. 
        /// Be warned that some failures (such as failure to decrypt) could be 
        /// indicative of a security breach and should not be ignored.
        /// </summary>
        [JsonPropertyName("partial_failure_response_code")]
        public int PartialFailureResponseCode { get; set; }

        public DecryptionItem()
        {
            this.PartialFailureResponseCode = (int)HttpStatusCode.BadRequest;
        }
    }
}