using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Azure
{
    /// <summary>
    /// Represents the Azure credentials.
    /// </summary>
    public class AzureCredentials
    {
        /// <summary>
        /// Gets or sets the Client Id.
        /// </summary>
        /// <value>
        /// The Client Id.
        /// </value>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Client secret.
        /// </summary>
        /// <value>
        /// The Client secret.
        /// </value>
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
    }
}