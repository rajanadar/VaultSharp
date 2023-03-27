
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudRoleModel
    {
        [JsonPropertyName("inline_policies")]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonPropertyName("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        [JsonPropertyName("remote_policies")]
        public List<AliCloudRemotePolicyModel> RemotePolicies { get; set; }

        [JsonPropertyName("role_arn")]
        public string RoleARN { get; set; }

        [JsonPropertyName("ttl")]
        public string TimeToLive { get; set; }
    }
}