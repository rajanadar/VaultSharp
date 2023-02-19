using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    /// <summary>
    /// SSH Credentials.
    /// </summary>
    public class SSHCredentials
    {
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// Type of credentials
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        [JsonPropertyName("key_type")]
        public SSHKeyType KeyType { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}