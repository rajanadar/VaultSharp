
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CreateActiveDirectoryRoleModel
    {
        [JsonProperty("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonProperty("ttl")]
        public long TimeToLive { get; set; }
    }
}