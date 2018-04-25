using Newtonsoft.Json;

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
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}