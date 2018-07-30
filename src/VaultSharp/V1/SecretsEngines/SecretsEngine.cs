using Newtonsoft.Json;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretEngines
{
    /// <summary>
    /// Represents a secret backend.
    /// </summary>
    public class SecretsEngine : AbstractBackend
    {
        /// <summary>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public SecretsEngineType Type { get; set; }
    }
}