using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Returns a list of entity ids.
    /// </summary>
    public class ListEntitiesResponse
    {
        [JsonProperty("keys")]
        public List<string> Keys { get; set; }
    }
}