using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.JWT.Models
{
    public class OIDCAuthURLInfo
    {
        [JsonPropertyName("auth_url")]
        public string AuthorizationURL { get; set; }
    }
}