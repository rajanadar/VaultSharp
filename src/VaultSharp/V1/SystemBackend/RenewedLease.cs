using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Renewed Lease info.
    /// </summary>
    public class RenewedLease
    {
        /// <summary>
        /// Gets or sets the lease identifier.
        /// </summary>
        [JsonPropertyName("lease_id")]
        public string LeaseId { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if this lease is renewable.
        /// </summary>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the lease.
        /// </summary>
        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; set; }
    }
}