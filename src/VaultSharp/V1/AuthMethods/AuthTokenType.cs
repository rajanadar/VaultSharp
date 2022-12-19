using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.AuthMethods
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthTokenType
    {
        [JsonProperty("service")]
        Service,

        [JsonProperty("batch")]
        Batch,

        [JsonProperty("default")]
        Default,

        [JsonProperty("default-service")]
        DefaultService,

        [JsonProperty("default-batch")]
        DefaultBatch,
    }
}