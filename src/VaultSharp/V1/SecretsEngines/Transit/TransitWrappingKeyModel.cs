using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class TransitWrappingKeyModel
    {
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}