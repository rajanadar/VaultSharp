using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// Renewed Lease info.
    /// </summary>
    public class RenewedLease
    {
        /// <summary>
        /// Gets or sets the lease identifier.
        /// </summary>
        [JsonProperty("lease_id")]
        public string LeaseId { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if this lease is renewable.
        /// </summary>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the lease.
        /// </summary>
        [JsonProperty("lease_duration")]
        public int LeaseDuration { get; set; }
    }
}