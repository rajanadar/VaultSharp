using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Backends.Secret.Models.PKI
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
    }
}