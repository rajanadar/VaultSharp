using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class TransitWrappingKeyModel
    {
        [JsonPropertyName("public_key")]
        public string PublicKey { get; set; }
    }
}