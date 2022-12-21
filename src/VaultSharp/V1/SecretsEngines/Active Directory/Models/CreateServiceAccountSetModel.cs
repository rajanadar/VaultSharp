using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CreateServiceAccountSetModel
    {
        [JsonProperty("disable_check_in_enforcement")]
        public bool DisableCheckInEnforcement { get; set; }

        [JsonProperty("max_ttl")]
        public long MaximumTimeToLive { get; set; } = 24 * 60 * 60;

        [JsonProperty("service_account_names")]
        public List<string> ServiceAccountNames { get; set; }

        [JsonProperty("ttl")]
        public long TimeToLive { get; set; } = 24 * 60 * 60;
    }
}