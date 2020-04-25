using Newtonsoft.Json;

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
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}