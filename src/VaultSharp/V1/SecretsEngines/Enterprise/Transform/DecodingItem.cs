using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Represents a single Decoding item.
    /// </summary>
    public class DecodingItem
    {
        /// <summary>
        /// Specifies the value to be decoded.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// Specifies the transformation within the role that should be used for this decode operation. 
        /// If a single transformation exists for role, this parameter may be skipped and will be inferred. 
        /// If multiple transformations exist, one must be specified.
        /// </summary>
        [JsonPropertyName("transformation")]
        public string Transformation { get; set; }

        /// <summary>
        /// Specifies the base64 decoded tweak to use. 
        /// Only applicable for FPE transformations with supplied as the tweak source.
        /// </summary>
        [JsonPropertyName("tweak")]
        public string Tweak { get; set; }
    }
}