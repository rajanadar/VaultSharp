using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class FullSecretIdInfo
    {
        [JsonProperty("cidr_list")]
        public List<string> CIDRList { get; set; }

        [JsonProperty("creation_time")]
        public DateTimeOffset? CreationTime { get; set; }

        [JsonProperty("expiration_time")]
        public DateTimeOffset? ExpirationTime { get; set; }

        [JsonProperty("last_updated_time")]
        public DateTimeOffset? LastUpdatedTime { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("secret_id_accessor")]
        public string SecretIdAccessor { get; set; }
        
        [JsonProperty("secret_id_num_uses")]
        public long SecretIdNumberOfUses { get; set; }

        [JsonProperty("secret_id_ttl")]
        public long SecretIdTimeToLive { get; set; }

        [JsonProperty("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }
    }
}
