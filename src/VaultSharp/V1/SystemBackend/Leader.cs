using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents information about high availability status and current leader instance of Vault.
    /// </summary>
    public class Leader
    {
        /// <summary>
        /// Gets or sets a value indicating whether [high availability enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [high availability enabled]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("ha_enabled")]
        public bool HighAvailabilityEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is the leader.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is the leader; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("is_self")]
        public bool IsSelf { get; set; }

        /// <summary>
        /// Gets or sets the address of the leader.
        /// e.g. https://127.0.0.1:8200/
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonProperty("leader_address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the address of the leader cluster.
        /// e.g. https://127.0.0.1:8201/
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonProperty("leader_cluster_address")]
        public string ClusterAddress { get; set; }

        /// <summary>
        /// Gets or sets the performance standby.
        /// </summary>
        [JsonProperty("performance_standby")]
        public bool PerformanceStandby { get; set; }

        /// <summary>
        /// Gets or sets the performance standby last remote wal.
        /// </summary>
        [JsonProperty("performance_standby_last_remote_wal")]
        public int PerformanceStandbyLastRemoteWal { get; set; }
    }
}