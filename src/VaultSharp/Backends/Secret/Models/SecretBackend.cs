using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models
{
    /// <summary>
    /// Represents a secret backend.
    /// </summary>
    public class SecretBackend
    {
        /// <summary>
        /// Gets or sets the mount point. If not set, the value will default to the <see cref="BackendType"/> value.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string MountPoint { get; set; }

        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public SecretBackendType BackendType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the secret backend configuration.
        /// </summary>
        /// <value>
        /// The secret backend configuration.
        /// </value>
        [JsonProperty("config")]
        public SecretBackendConfiguration SecretBackendConfiguration { get; set; }
    }
}