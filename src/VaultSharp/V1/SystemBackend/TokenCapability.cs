using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

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
                    var values = this["capabilities"] as JArray;

                    if (values != null)
                    {
                        return values.ToObject<List<string>>();
                    }                     
                }

                return Enumerable.Empty<string>();
            }
        }
    }
}