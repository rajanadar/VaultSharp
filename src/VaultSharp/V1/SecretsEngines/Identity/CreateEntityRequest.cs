using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object to create or update an entity.
    /// </summary>
    public class CreateEntityRequest : CustomizableEntityProperties
    {
        /// <summary>
        /// [optional]
        /// ID of the entity. If set, updates the corresponding existing entity
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the entity. 
        /// The recommended format for the name is 'entity-UUID'
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}