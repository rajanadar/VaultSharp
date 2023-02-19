using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class AWSLeaseConfigModel
    {
        [JsonPropertyName("lease")]
        public string Lease { get; set; }

        [JsonPropertyName("lease_max")]
        public string MaximumLease { get; set; }
    }
}