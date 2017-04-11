using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.RabbitMQ
{
    /// <summary>
    /// Represents the RabbitMQ connection information.
    /// </summary>
    public class RabbitMQConnectionInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the RabbitMQ management connection URI.
        /// </summary>
        /// <value>
        /// The Connection Uri
        /// </value>
        [JsonProperty("connection_uri")]
        public string ConnectionUri { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ management administrator username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ management administrator password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a flag which when set verifies the <see cref="ConnectionUri"/>, 
        /// <see cref="Username"/> and <see cref="Password"/> by actually connecting to the database. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// A flag to verify the connection.
        /// </value>
        [JsonProperty("verify_connection")]
        public bool VerifyConnection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQConnectionInfo" /> class.
        /// </summary>
        public RabbitMQConnectionInfo()
        {
            VerifyConnection = true;
        }
    }
}