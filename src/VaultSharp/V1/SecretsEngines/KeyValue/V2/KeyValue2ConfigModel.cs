using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    public class KeyValue2ConfigModel
    { 
        [JsonProperty("cas_required")]
        public bool CASRequired { get; set; }

        [JsonProperty("delete_version_after")]
        public string DeleteVersionAfter { get; set; }

        [JsonProperty("max_versions")]
        public int MaxVersions { get; set; }
    }
}