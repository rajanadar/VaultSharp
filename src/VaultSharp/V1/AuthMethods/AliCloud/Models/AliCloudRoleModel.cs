using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AliCloud.Models
{
    // raja todo: This should ideally be CreateAliCloudRoleModel, check why not?
    public class AliCloudRoleModel
    {
        [JsonPropertyName("arn")]
        public string ARN { get; set; }

        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }

        [JsonPropertyName("ttl")]
        public string TimeToLive { get; set; }

        [JsonPropertyName("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        [JsonPropertyName("period")]
        public string Period { get; set; }
    }
}