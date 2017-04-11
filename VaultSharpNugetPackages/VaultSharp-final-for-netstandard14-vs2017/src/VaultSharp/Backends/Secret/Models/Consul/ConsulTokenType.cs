using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Backends.Secret.Models.Consul
{
    /// <summary>
    /// Represents the token type for Consul.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConsulTokenType
    {
        /// <summary>
        /// The client
        /// </summary>
        client = 1,

        /// <summary>
        /// The management
        /// </summary>
        management = 2,
    }
}