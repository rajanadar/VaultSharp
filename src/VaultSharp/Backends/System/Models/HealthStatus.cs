using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the health status of a Vault instance.
    /// </summary>
    public class HealthStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether [health check succeeded].
        /// </summary>
        /// <value>
        /// <c>true</c> if [health check succeeded]; otherwise, <c>false</c>.
        /// </value>
        public bool HealthCheckSucceeded { get; set; }

        /// <summary>
        /// Gets or sets the cluster identifier.
        /// </summary>
        /// <value>
        /// The cluster identifier.
        /// </value>
        [JsonProperty("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster.
        /// </summary>
        /// <value>
        /// The name of the cluster.
        /// </value>
        [JsonProperty("cluster_name")]
        public string ClusterName { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the server time UTC unix timestamp.
        /// </summary>
        /// <value>
        /// The server time UTC unix timestamp.
        /// </value>
        [JsonProperty("server_time_utc")]
        public double ServerTimeUtcUnixTimestamp { get; set; }

        /// <summary>
        /// Gets the friendly server time from ServerTimeUtcUnixTimestamp 
        /// which is in seconds since January 1, 1970 12:00:00 a.m. UTC.
        /// </summary>
        /// <value>
        /// The server time in UTC. (zero offset)
        /// </value>
        public DateTimeOffset ServerTimeUtc
        {
            get
            {
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return new DateTimeOffset(date, TimeSpan.Zero).AddSeconds(ServerTimeUtcUnixTimestamp);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is a standby.
        /// </summary>
        /// <value>
        ///   <c>true</c> if standby; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("standby")]
        public bool Standby { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is sealed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sealed; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("sealed")]
        public bool Sealed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance of Vault is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("initialized")]
        public bool Initialized { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}