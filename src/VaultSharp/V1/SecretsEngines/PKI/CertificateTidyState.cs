using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate tidy state.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CertificateTidyState
    {
        Inactive,
        Running,
        Finished,
        Error
    }
}