using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object containing the list of entities to be deleted.
    /// </summary>
    public class BatchDeleteEntitiesRequest
    {
        [JsonProperty("entity_ids")]
        public IList<string> EntityIds { get; set; }
    }
}