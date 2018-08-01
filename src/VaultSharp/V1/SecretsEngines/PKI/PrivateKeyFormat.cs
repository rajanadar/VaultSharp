using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Private key format.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrivateKeyFormat
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
        /// The PKCS8 encoded format.
        /// </summary>
        pkcs8 = 2,
    }
}