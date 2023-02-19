using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Returns a list of entity ids.
    /// </summary>
    public class ListEntitiesResponse
    {
        [JsonPropertyName("keys")]
        public List<string> Keys { get; set; }
    }
}