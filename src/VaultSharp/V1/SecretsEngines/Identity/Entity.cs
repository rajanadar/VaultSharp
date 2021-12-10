using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class Entity
    {
        /// <summary>
        /// Name of the entity. 
        /// The recommended format for the name is 'entity-UUID'
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// [optional]
        /// ID of the entity. If set, updates the corresponding existing entity
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Key-Values pairs of the metadata associated with the entity
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// List of strings which with policies to be tied to the entity
        /// </summary>
        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        /// <summary>
        /// Boolean indicating whether the entity is disabled.
        /// </summary>
        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// List of aliases of the entity.
        /// </summary>
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }

        /// <summary>
        /// Bucket Key Hash of the entity.
        /// </summary>
        [JsonProperty("bucket_key_hash")]
        public string BucketKeyHash { get; set; }

        /// <summary>
        /// Creation Time of the entity.
        /// </summary>
        [JsonProperty("creation_time")]
        public System.DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Last Update Time of the entity.
        /// </summary>
        [JsonProperty("last_update_time")]
        public System.DateTime LastUpdateTime { get; set; }
    }
}