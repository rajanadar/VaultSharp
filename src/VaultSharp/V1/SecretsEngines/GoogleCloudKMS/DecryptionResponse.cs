using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Decryption output.
    /// </summary>
    public class DecryptionResponse
    {
        /// <summary>
        ///  Decrypted plain text.
        /// </summary>
        [JsonPropertyName("plaintext")]
        public string PlainText { get; set; }
    }
}