using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class CacheResponse
    {
        [JsonProperty("size")]
        public uint Size { get; set; }
    }
}