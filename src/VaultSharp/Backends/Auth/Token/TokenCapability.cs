using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Auth.Token
{
    /// <summary>
    /// Represents the capabilities of a token.
    /// </summary>
    public class TokenCapability
    {
        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        [JsonProperty("capabilities")]
        public IEnumerable<string> Capabilities { get; set; }
    }
}