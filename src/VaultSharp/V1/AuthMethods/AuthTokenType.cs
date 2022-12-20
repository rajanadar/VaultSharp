using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.AuthMethods
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthTokenType
    {
        [EnumMember(Value = "service")]
        Service,

        [EnumMember(Value = "batch")]
        Batch,

        [EnumMember(Value = "default")]
        Default,

        [EnumMember(Value = "default-service")]
        DefaultService,

        [EnumMember(Value = "default-batch")]
        DefaultBatch,
    }
}