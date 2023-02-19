using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class CertificateTidyStatus
    {
        [JsonPropertyName("safety_buffer")]
        public string SafetyBuffer { get; set; }

        [JsonPropertyName("tidy_cert_store")]
        public bool TidyCertStore { get; set; }

        [JsonPropertyName("tidy_revoked_certs")]
        public bool TidyRevokedCerts { get; set; }

        [JsonPropertyName("state")]
        public CertificateTidyState TidyState { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("time_started")]
        public DateTimeOffset? TimeStarted { get; set; }

        [JsonPropertyName("time_finished")]
        public DateTimeOffset? TimeFinished { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("cert_store_deleted_count")]
        public int CertStoreDeletedCount { get; set; }

        [JsonPropertyName("revoked_cert_deleted_count")]
        public string RevokedCertDeletedCount { get; set; }
    }
}