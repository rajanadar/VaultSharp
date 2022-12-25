
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudRoleModel
    {
        [JsonProperty("inline_policies")]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        [JsonProperty("remote_policies")]
        public List<AliCloudRemotePolicyModel> RemotePolicies { get; set; }

        [JsonProperty("role_arn")]
        public string RoleARN { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }
    }
}