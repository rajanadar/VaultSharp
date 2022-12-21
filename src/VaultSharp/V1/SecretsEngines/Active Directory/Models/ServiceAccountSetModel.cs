using Newtonsoft.Json;
using System.Collections.Generic;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ServiceAccountSetModel
    {
        [JsonProperty("disable_check_in_enforcement")]
        public bool DisableCheckInEnforcement { get; set; }

        [JsonProperty("max_ttl")]
        public long MaximumTimeToLive { get; set; }

        [JsonProperty("service_account_names")]
        public List<string> ServiceAccountNames { get; set; }

        [JsonProperty("ttl")]
        public long TimeToLive { get; set; }
    }
}