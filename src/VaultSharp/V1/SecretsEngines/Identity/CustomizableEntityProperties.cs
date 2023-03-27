using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// A base class containing the customizable properties of an entity.
    /// </summary>
    public class CustomizableEntityProperties
    {
        /// <summary>
        /// Key-Values pairs of the metadata associated with the entity
        /// </summary>
        [JsonPropertyName("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// List of strings which with policies to be tied to the entity
        /// </summary>
        [JsonPropertyName("policies")]
        public IList<string> Policies { get; set; }

        /// <summary>
        /// Boolean indicating whether the entity starts disabled.
        /// </summary>
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }
    }
}