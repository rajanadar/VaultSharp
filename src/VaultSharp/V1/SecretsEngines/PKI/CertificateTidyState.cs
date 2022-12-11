using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate tidy state.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CertificateTidyState
    {
        Inactive,
        Running,
        Finished,
        Error
    }
}