using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class ReadRoleReponse
    {
        [JsonProperty("data")]
        public RoleInfo Data { get; set; }
    }
}
