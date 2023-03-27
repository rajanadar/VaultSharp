using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class FullSecretIdInfo
    {
        [JsonPropertyName("cidr_list")]
        public List<string> CIDRList { get; set; }

        [JsonPropertyName("creation_time")]
        public DateTimeOffset? CreationTime { get; set; }

        [JsonPropertyName("expiration_time")]
        public DateTimeOffset? ExpirationTime { get; set; }

        [JsonPropertyName("last_updated_time")]
        public DateTimeOffset? LastUpdatedTime { get; set; }

        [JsonPropertyName("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonPropertyName("secret_id_accessor")]
        public string SecretIdAccessor { get; set; }
        
        [JsonPropertyName("secret_id_num_uses")]
        public long SecretIdNumberOfUses { get; set; }

        [JsonPropertyName("secret_id_ttl")]
        public long SecretIdTimeToLive { get; set; }

        [JsonPropertyName("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }
    }
}
