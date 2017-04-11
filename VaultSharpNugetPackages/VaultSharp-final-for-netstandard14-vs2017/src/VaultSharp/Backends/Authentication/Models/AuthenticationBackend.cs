using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models
{
    /// <summary>
    /// Represents an authentication backend.
    /// </summary>
    public class AuthenticationBackend
    {
        /// <summary>
        /// Gets or sets the authentication path. If not set, the value will default to the <see cref="BackendType"/> value.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string AuthenticationPath { get; set; }

        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public AuthenticationBackendType BackendType { get; set; }

        /// <summary>
        /// Gets or sets a human-friendly description of the auth backend.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}