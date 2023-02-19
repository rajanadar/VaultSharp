using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.MFA
{
    public abstract class AbstractMFAConfig
    { 
        /// <summary>
        /// Gets the name of MFA method.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of MFA.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}