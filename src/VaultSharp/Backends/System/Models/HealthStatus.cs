using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the health status of a Vault instance.
    /// </summary>
    public class HealthStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether [health check succeeded].
        /// </summary>
        /// <value>
        /// <c>true</c> if [health check succeeded]; otherwise, <c>false</c>.
        /// </value>
        public bool HealthCheckSucceeded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("initialized")]
        public bool Initialized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is sealed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sealed; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("sealed")]
        public bool Sealed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is a standby.
        /// </summary>
        /// <value>
        ///   <c>true</c> if standby; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("standby")]
        public bool Standby { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}