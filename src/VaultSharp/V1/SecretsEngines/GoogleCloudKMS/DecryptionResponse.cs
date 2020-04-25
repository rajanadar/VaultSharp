using Newtonsoft.Json;

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
        [JsonProperty("plaintext")]
        public string PlainText { get; set; }
    }
}