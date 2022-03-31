using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    public class SignRequestOptions : SignBatchInput
    {
        [JsonProperty("key_version")]
        public int? KeyVersion { get; set; }

        [JsonProperty("batch_input")]
        public List<SignBatchInput> BatchInput { get; set; }

        [JsonProperty("prehashed")]
        public bool PreHashed { get; set; }

        [JsonIgnore]
        public SignatureAlgorithm SignatureAlgorithm { get; set; }

        [JsonProperty("signature_algorithm")]
        protected string SignatureAlgorithmAsString
        {
            get
            {
                switch(SignatureAlgorithm)
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

        [JsonProperty("marshaling_algorithm")]
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

    public class SignBatchInput
    {
        [JsonProperty("input", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedInput { get; set; }



        [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedKeyDerivationContext { get; set; }
    }

    public enum SignatureAlgorithm
    {
        None,
        Pss,
        Pkcs1v15
    }

    public enum MarshalingAlgorithm
    {
        None,
        Asn1,
        Jws
    }
}