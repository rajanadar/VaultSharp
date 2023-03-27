using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ServiceAccountSetModel
    {
        [JsonPropertyName("disable_check_in_enforcement")]
        public bool DisableCheckInEnforcement { get; set; }

        [JsonPropertyName("max_ttl")]
        public long MaximumTimeToLive { get; set; }

        [JsonPropertyName("service_account_names")]
        public List<string> ServiceAccountNames { get; set; }

        [JsonPropertyName("ttl")]
        public long TimeToLive { get; set; }
    }
}