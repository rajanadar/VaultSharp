using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Signature output.
    /// </summary>
    public class SignatureResponse
    {
        /// <summary>
        /// The signature
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}