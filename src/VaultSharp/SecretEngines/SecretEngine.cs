using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.SecretEngines
{
    /// <summary>
    /// Represents a secret backend.
    /// </summary>
    public class SecretEngine : AbstractBackend
    {
        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public SecretEngineType Type { get; set; }
    }
}