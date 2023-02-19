
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class CreateAliCloudRoleModel
    {
        [JsonIgnore]
        public List<AliCloudRemotePolicyModel> RemotePolicies { get; set; }

        [JsonPropertyName("remote_policies")]
        public string RemotePoliciesJson
        {
            get
            {
                return JsonConvert.SerializeObject(RemotePolicies);
            }
        }

        [JsonIgnore]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonPropertyName("inline_policies")]
        public string InlinePoliciesJson
        {
            get
            {
                return JsonConvert.SerializeObject(InlinePolicies);
            }
        }

        [JsonPropertyName("role_arn")]
        public string RoleARN { get; set; }

        [JsonPropertyName("ttl")]
        public string TimeToLive { get; set; }

        [JsonPropertyName("max_ttl")]
        public string MaximumTimeToLive { get; set; }
    }
}