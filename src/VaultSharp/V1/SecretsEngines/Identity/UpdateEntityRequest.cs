using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object to update an entity.
    /// </summary>
    public class UpdateEntityRequest : CustomizableEntityProperties
    {
        /// <summary>
        /// Name of the entity. 
        /// The recommended format for the name is 'entity-UUID'
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}