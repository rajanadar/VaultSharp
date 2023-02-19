using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Lease info.
    /// </summary>
    public class Lease
    {
        /// <summary>
        /// Gets or sets the lease identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the issue time.
        /// </summary>
        [JsonPropertyName("issue_time")]
        public DateTimeOffset IssueTime { get; set; }

        /// <summary>
        /// Gets or sets the expiry time.
        /// </summary>
        [JsonPropertyName("expire_time")]
        public DateTimeOffset ExpiryTime { get; set; }

        /// <summary>
        /// Gets or sets the last renewal time.
        /// </summary>
        [JsonPropertyName("last_renewal_time")]
        public DateTimeOffset? LastRenewalTime { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if this lease is renewable.
        /// </summary>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the lease.
        /// </summary>
        [JsonPropertyName("ttl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TimeToLive { get; set; }
    }
}