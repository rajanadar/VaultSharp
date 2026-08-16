
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The output encoding format options for a request.
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<OutputEncodingFormat>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum OutputEncodingFormat
    {
        /// <summary>
        /// Return the response data in a base64 encoded string format.
        /// </summary>
        base64,
        /// <summary>
        /// Return the response data in a hexadecimal string format.
        /// </summary>
        hex
    }
}