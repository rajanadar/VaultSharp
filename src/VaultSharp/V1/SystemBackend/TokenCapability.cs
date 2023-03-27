using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the capabilities of a token.
    /// </summary>
    public class TokenCapability : Dictionary<string, object>
    {
        /// <summary>
        /// Gets the capabilities.
        /// </summary>
        public IEnumerable<string> Capabilities
        {
            get
            {
                if (this.ContainsKey("capabilities"))
                {
                    var enumerator = ((System.Text.Json.JsonElement)this["capabilities"]).EnumerateArray();

                    var results = new List<string>();

                    foreach (var item in enumerator)
                    {
                        results.Add(item.GetString());
                    }

                    return results;
                }

                return Enumerable.Empty<string>();
            }
        }
    }
}