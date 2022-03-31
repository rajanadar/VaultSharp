using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class SigningResponse
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("publickey")]
        public string PublicKey { get; set; }

        [JsonProperty("batch_results")]
        public List<SigningBatchResponse> BatchResults { get; set; }
    }

    public class SigningBatchResponse
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("publickey")]
        public string PublicKey { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}