﻿using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate format.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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