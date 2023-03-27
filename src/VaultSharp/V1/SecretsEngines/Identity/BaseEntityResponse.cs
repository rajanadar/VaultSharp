using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Response object containing some entity metadata and information
    /// </summary>
    public class BaseEntityResponse
    {
        /// <summary>
        /// Name of the entity created or updated.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// ID of the entity created or updated.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// List of aliases assigned to the entity.
        /// </summary>
        [JsonPropertyName("aliases")]
        public IList<string> Aliases { get; set; }
    }
}