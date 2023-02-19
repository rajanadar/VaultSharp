using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Verification output.
    /// </summary>
    public class VerificationResponse
    {
        /// <summary>
        /// Flag to indicate if signature is valid.
        /// </summary>
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }
    }
}