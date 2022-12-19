using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class AWSLeaseConfigModel
    {
        [JsonProperty("lease")]
        public string Lease { get; set; }

        [JsonProperty("lease_max")]
        public string MaximumLease { get; set; }
    }
}