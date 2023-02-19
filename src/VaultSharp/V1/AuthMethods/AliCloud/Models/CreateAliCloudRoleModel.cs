using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AliCloud.Models
{
    public class CreateAliCloudRoleModel
    {
        [JsonPropertyName("arn")]
        public string ARN { get; set; }

        [JsonPropertyName("token_ttl")]
        public string TokenTimeToLive { get; set; }

        [JsonPropertyName("token_max_ttl")]
        public string TokenMaximumTimeToLive { get; set; }

        [JsonPropertyName("token_policies")]
        public List<string> TokenPolicies { get; set; }

        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }

        [JsonPropertyName("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }

        [JsonPropertyName("token_explicit_max_ttl")]
        public string TokenExplicitMaximumTimeToLive { get; set; }

        [JsonPropertyName("token_no_default_policy")]
        public bool TokenNoDefaultPolicy { get; set; }

        [JsonPropertyName("token_num_uses")]
        public int TokenNumberOfUses { get; set; }

        [JsonPropertyName("token_period")]
        public string TokenPeriod { get; set; }

        [JsonPropertyName("token_type")]
        public AuthTokenType TokenType { get; set; }
    }
}