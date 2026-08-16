

using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<TransitDataKeyType>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum TransitDataKeyType
    {        
        plaintext,
        wrapped
    }
}