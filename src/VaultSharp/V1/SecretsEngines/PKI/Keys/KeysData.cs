using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Keys
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

        [JsonPropertyName("key_name")]
        public string KeyName { get; set; } = string.Empty;
    }

}