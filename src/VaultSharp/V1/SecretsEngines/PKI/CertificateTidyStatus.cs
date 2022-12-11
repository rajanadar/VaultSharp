using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class CertificateTidyStatus
    {
        [JsonProperty("safety_buffer")]
        public string SafetyBuffer { get; set; }

        [JsonProperty("tidy_cert_store")]
        public bool TidyCertStore { get; set; }

        [JsonProperty("tidy_revoked_certs")]
        public bool TidyRevokedCerts { get; set; }

        [JsonProperty("state")]
        public CertificateTidyState TidyState { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("time_started")]
        public DateTimeOffset? TimeStarted { get; set; }

        [JsonProperty("time_finished")]
        public DateTimeOffset? TimeFinished { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("cert_store_deleted_count")]
        public int CertStoreDeletedCount { get; set; }

        [JsonProperty("revoked_cert_deleted_count")]
        public string RevokedCertDeletedCount { get; set; }
    }
}