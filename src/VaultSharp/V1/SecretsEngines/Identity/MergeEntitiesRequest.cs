using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object containing the from_entity_ids and to_entity_id
    /// to merge a list of entities into a single entity.
    /// </summary>
    public class MergeEntitiesRequest
    {
        /// <summary>
        /// [required]
        /// Entity IDs which needs to get merged.
        /// </summary>
        [JsonProperty("from_entity_ids")]
        public IList<string> FromEntityIds { get; set; }

        /// <summary>
        /// [required]
        /// Entity ID into which all the other entities need to get merged.
        /// </summary>
        [JsonProperty("to_entity_id")]
        public string ToEntityId { get; set; }

        /// <summary>
        /// [optional]
        /// Setting this will follow the 'mine' strategy for merging MFA 
        /// secrets. If there are secrets of the same type both in entities 
        /// that are merged from and in entity into which all others are 
        /// getting merged, secrets in the destination will be unaltered. If 
        /// not set, this API will throw an error containing all the conflicts.
        /// </summary>
        [JsonProperty("force")]
        public bool Force { get; set; }
    }
}