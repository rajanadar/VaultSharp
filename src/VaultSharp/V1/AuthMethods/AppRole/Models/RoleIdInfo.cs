using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    /// <summary>
    /// https://developer.hashicorp.com/vault/api-docs/auth/approle#read-approle-role-id
    /// </summary>
    public class RoleIdInfo
    {
        [JsonProperty("role_id")]
        public string RoleId { get; set; }
    }
}
