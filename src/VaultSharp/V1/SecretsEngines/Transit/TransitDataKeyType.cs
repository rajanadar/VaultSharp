

using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransitDataKeyType
    {        
        plaintext,
        wrapped
    }
}