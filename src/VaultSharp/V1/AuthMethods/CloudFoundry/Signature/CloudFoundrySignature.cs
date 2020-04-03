using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.CloudFoundry.Signature
{
    public class CloudFoundrySignature
    {
        [JsonProperty("role")]
        public string RoleName { get; set; }

        [JsonProperty("signing_time")]
        public string  SigningTime { get; set; }

        [JsonProperty("cf_instance_cert")]
        public string InstanceCert { get; set; }
        
        [JsonProperty("signature")]
        public string SignatureKey { get; set; }
        
    }
}