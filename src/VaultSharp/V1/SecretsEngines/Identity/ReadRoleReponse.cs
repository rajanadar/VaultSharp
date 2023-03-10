using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class ReadRoleResponse
    {
        [JsonProperty("data")]
        public RoleInfo Data { get; set; }
    }
}
