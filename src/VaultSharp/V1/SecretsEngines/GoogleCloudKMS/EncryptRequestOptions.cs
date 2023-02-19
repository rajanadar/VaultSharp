using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Encryption input.
    /// </summary>
    public class EncryptRequestOptions
    {
        /// <summary>
        ///  Integer version of the crypto key version to use for encryption. 
        ///  If unspecified, this defaults to the latest active crypto key version.
        /// </summary>
        [JsonPropertyName("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        ///  Plaintext value to be encrypted. 
        ///  This can be a string or binary, but the size is limited. 
        ///  See the Google Cloud KMS documentation for information on size limitations by key types.
        /// </summary>
        [JsonPropertyName("plaintext")]
        public string PlainText { get; set; }

        /// <summary>
        ///  Optional base64-encoded data that, if specified, must also be provided to decrypt this payload.
        /// </summary>
        [JsonPropertyName("additional_authenticated_data")]
        public string AdditionalAuthenticatedData { get; set; }
    }
}