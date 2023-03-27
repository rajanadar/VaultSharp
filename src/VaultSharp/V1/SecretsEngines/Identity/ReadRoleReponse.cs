using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class ReadRoleResponse
    {
        [JsonPropertyName("data")]
        public RoleInfo Data { get; set; }
    }
}
