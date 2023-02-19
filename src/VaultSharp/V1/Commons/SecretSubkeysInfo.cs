using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    public class SecretSubkeysInfo
    { 
        [JsonPropertyName("metadata")]
        public CurrentSecretMetadata Metadata { get; set; }

        [JsonPropertyName("subkeys")]
        public Dictionary<string, object> Subkeys { get; set; }
    }
}
