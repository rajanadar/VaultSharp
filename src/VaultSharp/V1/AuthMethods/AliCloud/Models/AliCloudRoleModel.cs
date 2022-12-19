using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AliCloud.Models
{
    // raja todo: This should ideally be CreateAliCloudRoleModel, check why not?
    public class AliCloudRoleModel
    {
        [JsonProperty("arn")]
        public string ARN { get; set; }

        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }
    }
}