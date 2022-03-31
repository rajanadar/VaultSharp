using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class HashResponse
    {
        [JsonProperty("sum")]
        public string HashSum { get; set; }
    }
}