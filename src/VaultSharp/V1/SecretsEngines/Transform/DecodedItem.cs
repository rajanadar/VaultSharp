using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transform
{
    /// <summary>
    /// Represents a single Decoded item.
    /// </summary>
    public class DecodedItem
    {
        /// <summary>
        /// Specifies the decoded value.
        /// </summary>
        [JsonProperty("decoded_value")]
        public string DecodedValue { get; set; }

        /// <summary>
        /// Specifies the base64 encoded tweak that was provided during encoding.
        /// </summary>
        [JsonProperty("tweak")]
        public string Tweak { get; set; }
    }
}