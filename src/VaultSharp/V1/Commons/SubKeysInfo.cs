using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaultSharp.Core;

namespace VaultSharp.V1.Commons
{
    public class SubKeysInfo
    {
        [JsonProperty("subkeys")]
        public IDictionary<string, object> SubKeys { get; set; }

        [JsonProperty("metadata")]
        public CurrentSecretMetadata Metadata { get; set; }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            CreateTreeStructure(SubKeys);
        }

        private void CreateTreeStructure(IDictionary<string, object> rootLevel)
        {
            Checker.NotNull(rootLevel, "rootLevel");

            foreach (var item in rootLevel)
            {
                if (item.Value != null)
                {
                    var json = JsonConvert.SerializeObject(item.Value);
                    IDictionary<string, object> subLevel = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                    CreateTreeStructure(subLevel);
                    rootLevel[item.Key] = subLevel;
                }
            }
        }
    }
}
