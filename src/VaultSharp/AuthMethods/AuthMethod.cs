using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.AuthMethods
{
    /// <summary>
    /// Represents an auth method.
    /// </summary>
    public class AuthMethod : AbstractBackend
    {
        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public AuthBackendType Type { get; set; }
    }
}