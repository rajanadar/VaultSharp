
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyDocumentModel
    {
        [JsonPropertyName("Statement")]
        public List<AliCloudInlinePolicyDocumentStatementModel> Statement { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}