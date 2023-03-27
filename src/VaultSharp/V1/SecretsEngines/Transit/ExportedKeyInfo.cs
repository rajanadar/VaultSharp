using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Details about an exported key.
    /// </summary>
    public class ExportedKeyInfo
    {
        /// <summary>
        /// The list of key-version pairs in the key ring.
        /// </summary>
        [JsonPropertyName("keys")]
        public Dictionary<string, object> Keys { get; set; }

        /// <summary>
        /// The name of the key
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of the key
        /// </summary>
        [JsonPropertyName("type")]
        public TransitKeyType Type { get; set; }
    }
}