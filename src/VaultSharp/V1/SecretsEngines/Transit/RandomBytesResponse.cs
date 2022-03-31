using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class RandomBytesResponse
    {
        [JsonProperty("random_bytes")]
        public string EncodedRandomBytes { get; set; }
    }
}