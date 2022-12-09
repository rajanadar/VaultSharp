using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.JWT.Models
{
    public class OIDCAuthURLInfo
    {
        [JsonProperty("auth_url")]
        public string AuthorizationURL { get; set; }
    }
}