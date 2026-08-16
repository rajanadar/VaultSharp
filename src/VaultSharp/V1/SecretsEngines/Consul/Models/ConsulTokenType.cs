
using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<ConsulTokenType>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum ConsulTokenType
    {
        client,
        management
    }
}