
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.Core;
#if NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

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
#if NET8_0_OR_GREATER
                return JsonSerializer.Serialize(RemotePolicies, (JsonTypeInfo<List<AliCloudRemotePolicyModel>>)VaultSharpJsonContext.Default.GetTypeInfo(typeof(List<AliCloudRemotePolicyModel>)));
#else
                return JsonSerializer.Serialize(RemotePolicies);
#endif
            }
        }

        [JsonIgnore]
        public List<AliCloudInlinePolicyModel> InlinePolicies { get; set; }

        [JsonPropertyName("inline_policies")]
        public string InlinePoliciesJson
        {
            get
            {
#if NET8_0_OR_GREATER
                return JsonSerializer.Serialize(InlinePolicies, (JsonTypeInfo<List<AliCloudInlinePolicyModel>>)VaultSharpJsonContext.Default.GetTypeInfo(typeof(List<AliCloudInlinePolicyModel>)));
#else
                return JsonSerializer.Serialize(InlinePolicies);
#endif
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