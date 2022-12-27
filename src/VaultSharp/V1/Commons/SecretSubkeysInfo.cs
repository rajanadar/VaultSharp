using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    public class SecretSubkeysInfo
    { 
        [JsonProperty("metadata")]
        public CurrentSecretMetadata Metadata { get; set; }

        [JsonProperty("subkeys")]
        public Dictionary<string, object> Subkeys { get; set; }
    }
}
