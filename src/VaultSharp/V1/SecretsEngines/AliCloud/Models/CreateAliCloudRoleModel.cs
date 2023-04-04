
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.Core;

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
                return JsonSerializer.Serialize(RemotePolicies);
            }
        }

        [JsonIgnore]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonPropertyName("inline_policies")]
        public string InlinePoliciesJson
        {
            get
            {
                return JsonSerializer.Serialize(InlinePolicies);
            }
        }

        [JsonPropertyName("role_arn")]
        public string RoleARN { get; set; }

        [JsonPropertyName("ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string TimeToLive { get; set; }

        [JsonPropertyName("max_ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string MaximumTimeToLive { get; set; }
    }
}