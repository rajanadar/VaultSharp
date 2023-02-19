using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the list of keys.
    /// </summary>
    public class ListInfo
    {
        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        [JsonPropertyName("keys")]
        public IEnumerable<string> Keys { get; set; }
    }
}