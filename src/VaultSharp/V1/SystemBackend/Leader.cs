using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("ha_enabled")]
        public bool HighAvailabilityEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is the leader.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is the leader; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("is_self")]
        public bool IsSelf { get; set; }

        /// <summary>
        /// Gets or sets active time of the leader.
        /// </summary>
        [JsonPropertyName("active_time")]
        public DateTimeOffset ActiveTime { get; set; }

        /// <summary>
        /// Gets or sets the address of the leader.
        /// e.g. https://127.0.0.1:8200/
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonPropertyName("leader_address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the address of the leader cluster.
        /// e.g. https://127.0.0.1:8201/
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonPropertyName("leader_cluster_address")]
        public string ClusterAddress { get; set; }

        /// <summary>
        /// Gets or sets the performance standby.
        /// </summary>
        [JsonPropertyName("performance_standby")]
        public bool PerformanceStandby { get; set; }

        /// <summary>
        /// Gets or sets the performance standby last remote wal.
        /// </summary>
        [JsonPropertyName("performance_standby_last_remote_wal")]
        public long PerformanceStandbyLastRemoteWal { get; set; }

        /// <summary>
        /// Gets or sets the last remote.
        /// </summary>
        [JsonPropertyName("last_wal")]
        public long LastWal { get; set; }

        /// <summary>
        /// Gets or sets the raft committed index.
        /// </summary>
        [JsonPropertyName("raft_committed_index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long RaftCommittedIndex { get; set; }

        /// <summary>
        /// Gets or sets the raft applied index.
        /// </summary>
        [JsonPropertyName("raft_applied_index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long RaftAppliedIndex { get; set; }
    }
}