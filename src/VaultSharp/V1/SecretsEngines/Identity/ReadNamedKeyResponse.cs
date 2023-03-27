using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class ReadNamedKeyResponse 
    {
        [JsonPropertyName("data")]
        public NamedKeyInfo Data { get; set; }
    }
}
