using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.KMIP
{
    /// <summary>
    /// Represents the Certificate format.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CertificateFormat
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The DER Encoded format
        /// </summary>
        der = 1,

        /// <summary>
        /// The PEM encoded format.
        /// </summary>
        pem = 2,

        /// <summary>
        /// The PEM Bundle encoded format.
        /// </summary>
        pem_bundle = 3,
    }
}