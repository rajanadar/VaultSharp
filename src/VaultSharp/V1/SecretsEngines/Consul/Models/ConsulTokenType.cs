
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConsulTokenType
    {
        client,
        management
    }
}