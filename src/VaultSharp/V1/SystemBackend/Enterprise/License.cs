using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// License information.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Gets or sets the license expiration time.
        /// </summary>
        [JsonPropertyName("expiration_time")]
        public DateTimeOffset ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        [JsonPropertyName("features")]
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// Gets or sets the license id.
        /// </summary>
        [JsonPropertyName("license_id")]
        public string LicenseId { get; set; }

        /// <summary>
        /// Gets or sets the license start time.
        /// </summary>
        [JsonPropertyName("start_time")]
        public DateTimeOffset StartTime { get; set; }
    }
}