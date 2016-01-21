using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models
{
    /// <summary>
    /// Represents the configuration values for a secret backend.
    /// </summary>
    public class SecretBackendConfiguration
    {
        /// <summary>
        /// Gets or sets the default lease TTL.
        /// A value of "0" or "system" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The default lease TTL.
        /// </value>
        [JsonProperty("default_lease_ttl")]
        public string DefaultLeaseTtl { get; set; }

        /// <summary>
        /// Gets or sets the maximum lease TTL.
        /// A value of "0" or "system" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The maximum lease TTL.
        /// </value>
        [JsonProperty("max_lease_ttl")]
        public string MaximumLeaseTtl { get; set; }
    }
}