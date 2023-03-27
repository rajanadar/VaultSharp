using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    public class KeyValue2ConfigModel
    { 
        [JsonPropertyName("cas_required")]
        public bool CASRequired { get; set; }

        [JsonPropertyName("delete_version_after")]
        public string DeleteVersionAfter { get; set; }

        [JsonPropertyName("max_versions")]
        public int MaxVersions { get; set; }
    }
}