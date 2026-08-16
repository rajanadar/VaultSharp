
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The the source of the requested bytes
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<RandomBytesSource>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
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