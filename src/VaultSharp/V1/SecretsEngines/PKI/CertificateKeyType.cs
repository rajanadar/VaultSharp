using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate key type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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
        ec = 2,

        /// <summary>
        /// The Ed25519 Key type.
        /// </summary>
        ed25519 = 3,
    }
}