using Newtonsoft.Json;
using VaultSharp.V1.Core;

namespace VaultSharp.V1.SecretEngines
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