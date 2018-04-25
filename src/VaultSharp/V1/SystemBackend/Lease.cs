using System;
using Newtonsoft.Json;

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
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the issue time.
        /// </summary>
        [JsonProperty("issue_time")]
        public DateTimeOffset IssueTime { get; set; }

        /// <summary>
        /// Gets or sets the expiry time.
        /// </summary>
        [JsonProperty("expire_time")]
        public DateTimeOffset ExpiryTime { get; set; }

        /// <summary>
        /// Gets or sets the last renewal time.
        /// </summary>
        [JsonProperty("last_renewal_time")]
        public DateTimeOffset? LastRenewalTime { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if this lease is renewable.
        /// </summary>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the lease.
        /// </summary>
        [JsonProperty("ttl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TimeToLive { get; set; }
    }
}