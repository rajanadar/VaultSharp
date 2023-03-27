
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    public class ConsulRoleModel
    {
        [JsonPropertyName("consul_namespace")]
        public string ConsulNamespace { get; set; }

        [JsonPropertyName("consul_policies")]
        public List<string> Base64EncodedACLConsulPolicies { get; set; }

        [JsonPropertyName("consul_roles")]
        public List<string> ConsulRoles { get; set; }

        [JsonPropertyName("lease")]
        public long? Lease { get; set; }

        [JsonPropertyName("local")]
        public bool Local { get; set; }

        [JsonPropertyName("max_ttl")]
        public long? MaximumTimeToLive { get; set; }

        [JsonPropertyName("node_identities")]
        public List<string> NodeIdentities { get; set; }

        [JsonPropertyName("partition")]
        public string ConsulAdminPartition { get; set; }

        [JsonPropertyName("service_identities")]
        public List<string> ServiceIdentities { get; set; }

        [JsonPropertyName("ttl")]
        public long? TimeToLive { get; set; }

        [JsonPropertyName("token_type")]
        public ConsulTokenType ConsulTokenType { get; set; } = ConsulTokenType.client;

        [JsonPropertyName("policy")]
        public string Base64EncodedACLPolicy { get; set; }

        [JsonPropertyName("policies")]
        public List<string> Base64EncodedACLPolicies { get; set; }
    }
}