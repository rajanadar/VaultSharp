using Newtonsoft.Json;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods
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
        public AuthMethodType Type { get; set; }
    }
}