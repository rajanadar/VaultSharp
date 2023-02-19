using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Nomad
{
    /// <summary>
    /// Represents the Nomad credentials.
    /// </summary>
    public class NomadCredentials
    {
        /// <summary>
        /// Gets or sets the accessor id.
        /// </summary>
        /// <value>
        /// The accessor id.
        /// </value>
        [JsonPropertyName("accessor_id")]
        public string AccessorId { get; set; }

        /// <summary>
        /// Gets or sets the secret id.
        /// </summary>
        /// <value>
        /// The secret id.
        /// </value>
        [JsonPropertyName("secret_id")]
        public string SecretId { get; set; }
    }
}