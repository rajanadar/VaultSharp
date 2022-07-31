using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    public class SecretSubkeysInfo
    {
        [JsonProperty("subkeys")]
        public Dictionary<string, object> Subkeys { get; set; }

        [JsonProperty("metadata")]
        public CurrentSecretMetadata Metadata { get; set; }
    }
}
