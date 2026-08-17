using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public class IssuerConfigResponseData : IssuerConfigRequestData
    {
        [JsonPropertyName("ca_chain")] public List<string> CaChain { get; set; } = new List<string>();
        [JsonPropertyName("certificate")] public string Certificate { get; set; }
        [JsonPropertyName("issuer_id")] public string IssuerId { get; set; }
        [JsonPropertyName("issuing_certificates")] public List<string> IssuingCertificates { get; set; } = new List<string>();
        [JsonPropertyName("key_id")] public string KeyId { get; set; }
        [JsonPropertyName("revoked")] public bool Revoked { get; set; }
    }
}