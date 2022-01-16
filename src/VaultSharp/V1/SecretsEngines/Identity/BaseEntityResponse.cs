using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// ID of the entity created or updated.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// List of aliases assigned to the entity.
        /// </summary>
        [JsonProperty("aliases")]
        public IList<string> Aliases { get; set; }
    }
}