using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class RandomBytesRequestOptions
    {
        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OutputEncodingFormat Format { get; set; }
    }
}