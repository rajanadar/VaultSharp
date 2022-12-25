using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The the source of the requested bytes
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RandomBytesSource
    {
        /// <summary>
        /// platform entropy source
        /// </summary>
        platform,

        /// <summary>
        /// seal entropy source
        /// </summary>
        seal,

        /// <summary>
        /// mixed entropy source between platform and seal
        /// </summary>
        all
    }
}