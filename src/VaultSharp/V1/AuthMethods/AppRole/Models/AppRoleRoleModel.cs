using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class AppRoleRoleModel
    {
        [JsonPropertyName("bind_secret_id")]
        public bool BindSecretId { get; set; } = true;

        [JsonPropertyName("local_secret_ids")]
        public bool LocalSecretIds { get; set; }

        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }

        [JsonPropertyName("secret_id_bound_cidrs")]
        public List<string> SecretIdBoundCIDRs { get; set; }

        [JsonPropertyName("secret_id_num_uses")]
        public long SecretIdNumberOfUses { get; set; }

        [JsonPropertyName("secret_id_ttl")]
        public long SecretIdTimeToLive { get; set; }

        [JsonPropertyName("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }

        [JsonPropertyName("token_explicit_max_ttl")]
        public long TokenExplicitMaximumTimeToLive { get; set; }

        [JsonPropertyName("token_max_ttl")]
        public long TokenMaximumTimeToLive { get; set; }

        [JsonPropertyName("token_no_default_policy")]
        public bool TokenNoDefaultPolicy { get; set; }

        [JsonPropertyName("token_num_uses")]
        public long TokenNumberOfUses { get; set; }

        [JsonPropertyName("token_period")]
        public long TokenPeriod { get; set; }

        [JsonPropertyName("token_policies")]
        public List<string> TokenPolicies { get; set; }

        [JsonPropertyName("token_ttl")]
        public long TokenTimeToLive { get; set; }        

        [JsonPropertyName("token_type")]
        public AuthTokenType TokenType { get; set; }
    }
}