
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The the source of the requested bytes
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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