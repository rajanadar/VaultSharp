using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ConnectionConfigModel
    {
        [JsonPropertyName("binddn")]
        public string BindingDistinguishedName { get; set; }

        [JsonPropertyName("certificate")]
        public string X509PEMEncodedCertificate { get; set; }

        [JsonPropertyName("formatter")]
        [Obsolete]
        public string LegacyParameterFormatter { get; set; }

        [JsonPropertyName("insecure_tls")]
        public bool? ConnectionInsecureTLS { get; set; }

        [JsonPropertyName("last_rotation_tolerance")]
        public long LastRotationTolerance { get; set; }

        [JsonPropertyName("length")]
        [Obsolete]
        public string LegacyParameterLength { get; set; }

        [JsonPropertyName("max_ttl")]
        public long PasswordMaximumTimeToLive { get; set; }

        [JsonPropertyName("password_policy")]
        public string PasswordPolicy { get; set; }

        [JsonPropertyName("request_timeout")]
        public long ConnectionRequestTimeout { get; set; }

        [JsonPropertyName("starttls")]
        public bool? ConnectionStartTLS { get; set; }

        [JsonPropertyName("tls_max_version")]
        public string TLSMaxVersion { get; set; }

        [JsonPropertyName("tls_min_version")]
        public string TLSMinVersion { get; set; }

        [JsonPropertyName("ttl")]
        public long PasswordTimeToLive { get; set; }

        [JsonPropertyName("upndomain")]
        public string UPNDomain { get; set; }

        [JsonPropertyName("url")]
        public string ConnectionURL { get; set; }

        [JsonPropertyName("userdn")]
        public string UserDistinguishedName { get; set; }
    }
}