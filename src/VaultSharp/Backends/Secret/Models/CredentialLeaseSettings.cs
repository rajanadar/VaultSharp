using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models
{
    /// <summary>
    /// Represents the lease settings for the dynamic credentials generated for a backend.
    /// </summary>
    public class CredentialLeaseSettings
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the duration of the lease provided as a string duration with time suffix. Hour is the largest suffix..
        /// </summary>
        /// <value>
        /// The duration of the lease.
        /// </value>
        [JsonProperty("lease")]
        public string LeaseTime { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the maximum lease value provided as a string duration with time suffix. Hour is the largest suffix.
        /// </summary>
        /// <value>
        /// The maximum duration of the lease.
        /// </value>
        [JsonProperty("lease_max")]
        public string MaximumLeaseTime { get; set; }
    }
}