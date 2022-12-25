
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class CreateAliCloudRoleModel
    {
        [JsonIgnore]
        public List<AliCloudRemotePolicyModel> RemotePolicies { get; set; }

        [JsonProperty("remote_policies")]
        public string RemotePoliciesJson
        {
            get
            {
                return JsonConvert.SerializeObject(RemotePolicies);
            }
        }

        [JsonIgnore]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonProperty("inline_policies")]
        public string InlinePoliciesJson
        {
            get
            {
                return JsonConvert.SerializeObject(InlinePolicies);
            }
        }

        [JsonProperty("role_arn")]
        public string RoleARN { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }
    }
}