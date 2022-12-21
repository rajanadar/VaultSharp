using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Abstract Role definition.
    /// </summary>
    public class AbstractAWSRoleModel
    {
        [JsonProperty("role_arns")]
        public List<string> RoleARNs { get; set; }

        [JsonProperty("policy_arns")]
        public List<string> PolicyARNs { get; set; }

        [JsonProperty("policy_document")]
        public string PolicyDocument { get; set; }

        [JsonProperty("iam_groups")]
        public List<string> IAMGroups { get; set; }

        [JsonProperty("iam_tags")]
        public List<string> IAMTags { get; set; }

        [JsonProperty("default_sts_ttl")]
        public string DefaultSTSTimeToLive { get; set; }

        [JsonProperty("max_sts_ttl")]
        public string MaximumSTSTimeToLive { get; set; }

        [JsonProperty("user_path")]
        public string UserPath { get; set; }

        [JsonProperty("permissions_boundary_arn")]
        public string PermissionsBoundaryARN { get; set; }

        
        
        [JsonProperty("policy")]
        [Obsolete]
        public string LegacyParameterPolicy { get; set; }

        [JsonProperty("arn")]
        [Obsolete]
        public string LegacyParameterARN { get; set; }
    }
}