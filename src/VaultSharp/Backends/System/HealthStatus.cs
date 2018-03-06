using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System
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
        /// Gets or sets the replication_performance_mode.
        /// </summary>
        /// <value>
        /// The replication_performance_mode.
        /// </value>
        [JsonProperty("replication_performance_mode")]
        public string ReplicationPerformanceMode { get; set; }

        /// <summary>
        /// Gets or sets the replication_dr_mode.
        /// </summary>
        /// <value>
        /// The replication_dr_mode.
        /// </value>
        [JsonProperty("replication_dr_mode")]
        public string ReplicationDrMode { get; set; }

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