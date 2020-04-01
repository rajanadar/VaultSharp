using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.CloudFoundry.Token
{
    public class Signature
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("signing_time")]
        public string  SigningTime { get; set; }

        [JsonProperty("cf_instance_cert")]
        public string CFInstanceCert { get; set; }
        
        [JsonProperty("signature")]
        public string SignatureKey { get; set; }
        
    }
}