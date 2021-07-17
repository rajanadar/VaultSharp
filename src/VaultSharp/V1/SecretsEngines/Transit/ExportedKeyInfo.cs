using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("keys")]
        public Dictionary<string, object> Keys { get; set; }

        /// <summary>
        /// The name of the key
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of the key
        /// </summary>
        [JsonProperty("type")]
        public TransitKeyType Type { get; set; }
    }
}