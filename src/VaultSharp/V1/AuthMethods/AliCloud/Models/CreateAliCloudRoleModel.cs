using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AliCloud.Models
{
    public class CreateAliCloudRoleModel
    {
        [JsonProperty("arn")]
        public string ARN { get; set; }

        [JsonProperty("token_ttl")]
        public string TokenTimeToLive { get; set; }

        [JsonProperty("token_max_ttl")]
        public string TokenMaximumTimeToLive { get; set; }

        [JsonProperty("token_policies")]
        public List<string> TokenPolicies { get; set; }

        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        [JsonProperty("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }

        [JsonProperty("token_explicit_max_ttl")]
        public string TokenExplicitMaximumTimeToLive { get; set; }

        [JsonProperty("token_no_default_policy")]
        public bool TokenNoDefaultPolicy { get; set; }

        [JsonProperty("token_num_uses")]
        public bool TokenNumberOfUses { get; set; }

        [JsonProperty("token_period")]
        public string TokenPeriod { get; set; }

        [JsonProperty("token_type")]
        public AuthTokenType TokenType { get; set; }
    }
}