using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class HmacResponse : HmacBatchResponse
    {
        [JsonProperty("batch_results")]
        public List<HmacBatchResponse> BatchResults { get; set; }
    }

    public class HmacBatchResponse
    {

        [JsonProperty("hmac")]
        public string Hmac { get; set; }

        [JsonProperty("error")]
        public string ErrorResponse { get; set; }
    }
}