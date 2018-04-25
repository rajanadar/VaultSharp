using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("expiration_time")]
        public DateTimeOffset ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        [JsonProperty("features")]
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// Gets or sets the license id.
        /// </summary>
        [JsonProperty("license_id")]
        public string LicenseId { get; set; }

        /// <summary>
        /// Gets or sets the license start time.
        /// </summary>
        [JsonProperty("start_time")]
        public DateTimeOffset StartTime { get; set; }
    }
}