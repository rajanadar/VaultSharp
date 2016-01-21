using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the Certificate key type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CertificateKeyType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The RSA Key type.
        /// </summary>
        rsa = 1,

        /// <summary>
        /// The Elliptic Curve key type.
        /// </summary>
        ec = 2
    }
}