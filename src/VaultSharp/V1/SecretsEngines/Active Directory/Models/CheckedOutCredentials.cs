using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    /// <summary>
    /// Represents the Active Directory checked out credentials.
    /// </summary>
    public class CheckedOutCredentials
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("service_account_name")]
        public string ServiceAccountName { get; set; }
    }
}