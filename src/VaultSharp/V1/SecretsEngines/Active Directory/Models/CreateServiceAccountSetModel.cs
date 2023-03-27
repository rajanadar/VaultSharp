using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CreateServiceAccountSetModel
    {
        [JsonPropertyName("disable_check_in_enforcement")]
        public bool DisableCheckInEnforcement { get; set; }

        [JsonPropertyName("max_ttl")]
        public long MaximumTimeToLive { get; set; } = 24 * 60 * 60;

        [JsonPropertyName("service_account_names")]
        public List<string> ServiceAccountNames { get; set; }

        [JsonPropertyName("ttl")]
        public long TimeToLive { get; set; } = 24 * 60 * 60;
    }
}