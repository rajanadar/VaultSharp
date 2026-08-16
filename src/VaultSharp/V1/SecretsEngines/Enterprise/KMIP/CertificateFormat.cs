using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.Enterprise.KMIP
{
    /// <summary>
    /// Represents the Certificate format.
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<CertificateFormat>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
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