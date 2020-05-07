using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class AWSRoleModel
    {
        [JsonProperty("policy_document")]
        public string PolicyDocument { get; set; }

        [JsonProperty("policy_arns")]
        public List<string> PolicyARNs { get; set; }

        [JsonProperty("credential_types")]
        public List<string> CredentialTypes { get; set; }

        [JsonProperty("role_arns")]
        public List<string> RoleARNs { get; set; }
    }
}