using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines
{
    /// <summary>
    /// Represents credentials with a username and password.
    /// </summary>
    public class UsernamePasswordCredentials
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}