using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Abstract Role definition.
    /// </summary>
    public class AbstractAWSRoleModel
    {
        [JsonPropertyName("role_arns")]
        public List<string> RoleARNs { get; set; }

        [JsonPropertyName("policy_arns")]
        public List<string> PolicyARNs { get; set; }

        [JsonPropertyName("policy_document")]
        public string PolicyDocument { get; set; }

        [JsonPropertyName("iam_groups")]
        public List<string> IAMGroups { get; set; }

        [JsonPropertyName("iam_tags")]
        public List<string> IAMTags { get; set; }

        [JsonPropertyName("default_sts_ttl")]
        public string DefaultSTSTimeToLive { get; set; }

        [JsonPropertyName("max_sts_ttl")]
        public string MaximumSTSTimeToLive { get; set; }

        [JsonPropertyName("user_path")]
        public string UserPath { get; set; }

        [JsonPropertyName("permissions_boundary_arn")]
        public string PermissionsBoundaryARN { get; set; }

        
        
        [JsonPropertyName("policy")]
        [Obsolete]
        public string LegacyParameterPolicy { get; set; }

        [JsonPropertyName("arn")]
        [Obsolete]
        public string LegacyParameterARN { get; set; }
    }
}