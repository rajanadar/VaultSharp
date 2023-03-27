using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    /// <summary>
    /// Represents the Active Directory credentials.
    /// </summary>
    public class ActiveDirectoryCredentials
    {
        /// <summary>
        /// Gets or sets the current password.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonPropertyName("current_password")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the last password.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonPropertyName("last_password")]
        public string LastPassword { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}