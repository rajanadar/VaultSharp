using Newtonsoft.Json;
using VaultSharp.Infrastructure.JsonConverters;

namespace VaultSharp.Backends.Secret.Models.SSH
{
    /// <summary>
    /// Represents the SSH Credentials
    /// </summary>
    [JsonConverter(typeof(SSHCredentialsJsonConverter))]

    public abstract class SSHCredentials
    {
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        [JsonProperty("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Type of credentials
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        [JsonProperty("key_type")]
        public abstract SSHKeyType KeyType { get; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}