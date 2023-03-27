using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    /// <summary>
    /// Represents the Active Directory checked out credentials.
    /// </summary>
    public class CheckedOutCredentials
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("service_account_name")]
        public string ServiceAccountName { get; set; }
    }
}