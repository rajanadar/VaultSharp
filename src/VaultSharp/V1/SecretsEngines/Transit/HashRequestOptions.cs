using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class HashRequestOptions
    {
        [JsonProperty("input")]
        public string Base64EncodedInput { get; set; }


        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OutputEncodingFormat Format { get; set; }
    }
}