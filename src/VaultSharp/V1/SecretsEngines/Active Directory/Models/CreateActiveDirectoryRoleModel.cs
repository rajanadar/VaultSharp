
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CreateActiveDirectoryRoleModel
    {
        [JsonPropertyName("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonPropertyName("ttl")]
        public long TimeToLive { get; set; }
    }
}