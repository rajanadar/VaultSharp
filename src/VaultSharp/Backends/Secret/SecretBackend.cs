using Newtonsoft.Json;
using VaultSharp.Backends.System;

namespace VaultSharp.Backends.Secret
{
    /// <summary>
    /// Represents a secret backend.
    /// </summary>
    public class SecretBackend : AbstractBackendBase
    {
        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public SecretBackendType Type { get; set; }
    }
}