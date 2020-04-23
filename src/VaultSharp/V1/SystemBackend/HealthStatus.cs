using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the health status of a Vault instance.
    /// </summary>
    public class HealthStatus
    {
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
        /// Gets or sets the performance standby.
        /// </summary>
        [JsonProperty("performance_standby")]
        public bool PerformanceStandby { get; set; }

        /// <summary>
        /// Gets or sets the replication performance mode.
        /// </summary>
        [JsonProperty("replication_performance_mode")]
        public string ReplicationPerformanceMode { get; set; }

        /// <summary>
        /// Gets or sets the replication dr mode.
        /// </summary>
        [JsonProperty("replication_dr_mode")]
        public string ReplicationDisasterRecoverymode { get; set; }

        /// <summary>
        /// Gets or sets the server time UTC unix timestamp.
        /// </summary>
        /// <value>
        /// The server time UTC unix timestamp.
        /// </value>
        [JsonProperty("server_time_utc")]
        public long ServerTimeUtcUnixTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster.
        /// </summary>
        /// <value>
        /// The name of the cluster.
        /// </value>
        [JsonProperty("cluster_name")]
        public string ClusterName { get; set; }

        /// <summary>
        /// Gets or sets the cluster identifier.
        /// </summary>
        /// <value>
        /// The cluster identifier.
        /// </value>
        [JsonProperty("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets the friendly server time from ServerTimeUtcUnixTimestamp 
        /// which is in seconds since January 1, 1970 12:00:00 a.m. UTC.
        /// </summary>
        /// <value>
        /// The server time in UTC. (zero offset)
        /// </value>
        [JsonIgnore]
        public DateTimeOffset ServerTimeUtc
        {
            get
            {
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return new DateTimeOffset(date, TimeSpan.Zero).AddSeconds(ServerTimeUtcUnixTimestamp);
            }
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [JsonIgnore]
        public int? HttpStatusCode { get; set; }
    }
}