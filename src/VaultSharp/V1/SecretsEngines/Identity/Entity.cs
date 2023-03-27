using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class Entity
    {
        /// <summary>
        /// Name of the entity. 
        /// The recommended format for the name is 'entity-UUID'
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// [optional]
        /// ID of the entity. If set, updates the corresponding existing entity
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

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
        /// Boolean indicating whether the entity is disabled.
        /// </summary>
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// List of aliases of the entity.
        /// </summary>
        [JsonPropertyName("aliases")]
        public IList<string> Aliases { get; set; }

        /// <summary>
        /// Bucket Key Hash of the entity.
        /// </summary>
        [JsonPropertyName("bucket_key_hash")]
        public string BucketKeyHash { get; set; }

        /// <summary>
        /// Creation Time of the entity.
        /// </summary>
        [JsonPropertyName("creation_time")]
        public System.DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Last Update Time of the entity.
        /// </summary>
        [JsonPropertyName("last_update_time")]
        public System.DateTime LastUpdateTime { get; set; }
    }
}