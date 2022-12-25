
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyDocumentModel
    {
        [JsonProperty("Statement")]
        public List<AliCloudInlinePolicyDocumentStatementModel> Statement { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}