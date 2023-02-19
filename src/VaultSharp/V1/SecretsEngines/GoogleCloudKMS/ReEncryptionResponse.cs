using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// ReEncryption output.
    /// </summary>
    public class ReEncryptionResponse
    {
        /// <summary>
        ///  Integer version of the crypto key.
        /// </summary>
        /// <remarks>
        /// raja todo: why is this not int?
        /// </remarks>
        [JsonPropertyName("key_version")]
        public string KeyVersion { get; set; }

        /// <summary>
        /// ReEncrypted cipher text.
        /// </summary>
        [JsonPropertyName("ciphertext")]
        public string CipherText { get; set; }
    }
}