
using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ConsulTokenType
    {
        client,
        management
    }
}