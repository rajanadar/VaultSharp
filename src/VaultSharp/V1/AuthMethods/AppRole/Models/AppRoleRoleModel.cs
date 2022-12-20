using Newtonsoft.Json;
using System.Collections.Generic;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class AppRoleRoleModel
    {
        [JsonProperty("bind_secret_id")]
        public bool BindSecretId { get; set; } = true;

        [JsonProperty("local_secret_ids")]
        public bool LocalSecretIds { get; set; }

        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        [JsonProperty("secret_id_bound_cidrs")]
        public List<string> SecretIdBoundCIDRs { get; set; }

        [JsonProperty("secret_id_num_uses")]
        public int SecretIdNumberOfUses { get; set; }

        [JsonProperty("secret_id_ttl")]
        public int SecretIdTimeToLive { get; set; }

        [JsonProperty("token_bound_cidrs")]
        public List<string> TokenBoundCIDRs { get; set; }

        [JsonProperty("token_explicit_max_ttl")]
        public int TokenExplicitMaximumTimeToLive { get; set; }

        [JsonProperty("token_max_ttl")]
        public int TokenMaximumTimeToLive { get; set; }

        [JsonProperty("token_no_default_policy")]
        public bool TokenNoDefaultPolicy { get; set; }

        [JsonProperty("token_num_uses")]
        public int TokenNumberOfUses { get; set; }

        [JsonProperty("token_period")]
        public int TokenPeriod { get; set; }

        [JsonProperty("token_policies")]
        public List<string> TokenPolicies { get; set; }

        [JsonProperty("token_ttl")]
        public int TokenTimeToLive { get; set; }        

        [JsonProperty("token_type")]
        public AuthTokenType TokenType { get; set; }
    }
}