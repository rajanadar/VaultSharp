using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class VerifyRequestOptions : VerifyBatchInput
    {
        [JsonProperty("batch_input")]
        public List<VerifyBatchInput> BatchInput { get; set; }

        [JsonProperty("hmac")]
        public string Hmac { get; set; }

        [JsonProperty("prehashed")]
        public bool PreHashed { get; set; }

        [JsonIgnore]
        public SignatureAlgorithm SignatureAlgorithm { get; set; }

        [JsonProperty("signature_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        protected string SignatureAlgorithmAsString
        {
            get
            {
                switch (SignatureAlgorithm)
                {
                    case SignatureAlgorithm.Pkcs1v15: return "pkcs1v15";
                    case SignatureAlgorithm.Pss: return "pss";
                    default:
                        return null;
                }
            }
        }

        [JsonIgnore]
        public MarshalingAlgorithm MarshalingAlgorithm { get; set; }

        [JsonProperty("marshaling_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        protected string MarshalingAlgorithmAsString
        {
            get
            {
                switch (MarshalingAlgorithm)
                {
                    case MarshalingAlgorithm.Asn1: return "asn1";
                    case MarshalingAlgorithm.Jws: return "jws";
                    default:
                        return null;
                }
            }
        }
    }

    public class VerifyBatchInput
    {
        [JsonProperty("input")]
        public string Base64EncodedInput { get; set; }


        [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedKeyDerivationContext { get; set; }

        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }
}