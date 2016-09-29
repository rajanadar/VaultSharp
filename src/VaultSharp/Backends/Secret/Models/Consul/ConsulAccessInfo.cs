using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Consul
{
    /// <summary>
    /// Represents the Consul access information.
    /// </summary>
    public class ConsulAccessInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the address of the Consul instance, provided as host:port
        /// </summary>
        /// <value>
        /// The address with port.
        /// </value>
        [JsonProperty("address")]
        public string AddressWithPort { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the URL scheme to use. 
        /// Defaults to HTTP, as Consul does not expose HTTPS by default.
        /// </summary>
        /// <value>
        /// The URI scheme.
        /// </value>
        [JsonProperty("scheme")]
        public string UriScheme { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the Consul ACL token to use. 
        /// Must be a management type token.
        /// </summary>
        /// <value>
        /// The management token.
        /// </value>
        [JsonProperty("token")]
        public string ManagementToken { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulAccessInfo"/> class.
        /// </summary>
        public ConsulAccessInfo()
        {
            UriScheme = "http";
        }
    }
}