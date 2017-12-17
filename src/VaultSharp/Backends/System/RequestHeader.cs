using Newtonsoft.Json;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// One Header.
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if this header's value is HMAC'ed in the audit logs.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("hmac")]
        public bool HMAC { get; set; }
    }
}