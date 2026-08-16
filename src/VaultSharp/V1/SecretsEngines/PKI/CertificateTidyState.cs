using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate tidy state.
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<CertificateTidyState>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum CertificateTidyState
    {
        Inactive,
        Running,
        Finished,
        Error
    }
}