
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    public class ConsulRoleModel
    {
        [JsonProperty("consul_namespace")]
        public string ConsulNamespace { get; set; }

        [JsonProperty("consul_policies")]
        public List<string> Base64EncodedACLConsulPolicies { get; set; }

        [JsonProperty("consul_roles")]
        public List<string> ConsulRoles { get; set; }

        [JsonProperty("lease")]
        public long? Lease { get; set; }

        [JsonProperty("local")]
        public bool Local { get; set; }

        [JsonProperty("max_ttl")]
        public long? MaximumTimeToLive { get; set; }

        [JsonProperty("node_identities")]
        public List<string> NodeIdentities { get; set; }

        [JsonProperty("partition")]
        public string ConsulAdminPartition { get; set; }

        [JsonProperty("service_identities")]
        public List<string> ServiceIdentities { get; set; }

        [JsonProperty("ttl")]
        public long? TimeToLive { get; set; }

        [JsonProperty("token_type")]
        public ConsulTokenType ConsulTokenType { get; set; } = ConsulTokenType.client;

        [JsonProperty("policy")]
        public string Base64EncodedACLPolicy { get; set; }

        [JsonProperty("policies")]
        public List<string> Base64EncodedACLPolicies { get; set; }
    }
}