using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
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