using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.RabbitMQ
{
    /// <summary>
    /// Represents the RabbitMQ role definition
    /// </summary>
    public class RabbitMQRoleDefinition
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets comma-separated RabbitMQ management tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [JsonProperty("tags")]
        public string Tags { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a map of virtual hosts to permissions.
        /// </summary>
        /// <value>
        /// The virtual hosts to permissions map.
        /// </value>
        [JsonProperty("vhosts")]
        public object VirtualHostPermissions { get; set; }
    }
}