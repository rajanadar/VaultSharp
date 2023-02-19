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
                    var values = this["capabilities"] as JsonArray;
                    var results = new List<string>();

                    if (values != null)
                    {
                        values.ToList().ForEach(jn => results.Add(jn.ToString()));
                        return results;
                    }                     
                }

                return Enumerable.Empty<string>();
            }
        }
    }
}