using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public class KeysData
    {
        [JsonPropertyName("keys")]
        public List<string> Keys { get; set; } = new List<string>();

        [JsonPropertyName("key_info")]
        public Dictionary<string, KeyInfo> KeyInfos { get; set; } = new Dictionary<string, KeyInfo>();
    }

    public class KeyInfo
    {
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; } = string.Empty;
    }

}