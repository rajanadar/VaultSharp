using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object containing the list of entities to be deleted.
    /// </summary>
    public class BatchDeleteEntitiesRequest
    {
        [JsonPropertyName("entity_ids")]
        public IList<string> EntityIds { get; set; }
    }
}