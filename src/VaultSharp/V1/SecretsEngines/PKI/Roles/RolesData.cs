using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Roles
{
    public class RolesData
    {
        [JsonPropertyName("keys")]
        public List<string> Keys { get; set; }
    }
}