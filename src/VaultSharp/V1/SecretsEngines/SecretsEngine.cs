using System.Text.Json.Serialization;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines
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
        [JsonPropertyName("type")]
        public SecretsEngineType Type { get; set; }
    }
}