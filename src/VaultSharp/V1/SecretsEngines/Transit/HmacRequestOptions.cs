using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class HmacRequestOptions : HmacBatchInput
    {
        [JsonProperty("key_version")]
        public int? KeyVersion { get; set; }

        [JsonProperty("batch_input")]
        public List<HmacBatchInput> BatchInput { get; set; }
    }

    public class HmacBatchInput
    {
        [JsonProperty("input")]
        public string Base64EncodedInput { get; set; }
    }
}