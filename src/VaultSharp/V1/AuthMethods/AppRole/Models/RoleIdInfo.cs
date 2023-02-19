using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    /// <summary>
    /// https://developer.hashicorp.com/vault/api-docs/auth/approle#read-approle-role-id
    /// </summary>
    public class RoleIdInfo
    {
        [JsonPropertyName("role_id")]
        public string RoleId { get; set; }
    }
}
