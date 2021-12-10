using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object to create a entity by name
    /// </summary>
    public class CreateEntityByNameRequest : CustomizableEntityProperties
    {
        /// <summary>
        /// Name of the entity. 
        /// The recommended format for the name is 'entity-UUID'
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}