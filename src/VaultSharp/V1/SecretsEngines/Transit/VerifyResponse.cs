using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class VerifyResponse
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("batch_results")]
        public List<VerifyBatchResponse> BatchResults { get; set; }
    }

    public class VerifyBatchResponse
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }
}