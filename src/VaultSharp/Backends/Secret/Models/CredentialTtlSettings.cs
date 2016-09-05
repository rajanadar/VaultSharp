
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models
{
    /// <summary>
    /// Represents the TTL settings for the dynamic credentials generated for a backend.
    /// </summary>
    public class CredentialTtlSettings
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the duration of the lease provided as a string duration with time suffix. Hour is the largest suffix..
        /// </summary>
        /// <value>
        /// The duration of the lease.
        /// </value>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the maximum lease value provided as a string duration with time suffix. Hour is the largest suffix.
        /// </summary>
        /// <value>
        /// The maximum duration of the lease.
        /// </value>
        [JsonProperty("ttl_max")]
        public string MaximumTimeToLive { get; set; }
    }
}