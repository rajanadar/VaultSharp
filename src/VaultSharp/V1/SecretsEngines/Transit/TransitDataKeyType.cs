
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransitDataKeyType
    {        
        [EnumMember(Value = "plaintext")]
        PlainText,

        [EnumMember(Value = "wrapped")]
        Wrapped
    }
}