using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class ReadNamedKeyResponse 
    {
        [JsonProperty("data")]
        public NamedKeyInfo Data { get; set; }
    }
}
