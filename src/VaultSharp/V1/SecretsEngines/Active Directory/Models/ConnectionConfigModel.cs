using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ConnectionConfigModel
    {
        [JsonProperty("binddn")]
        public string BindingDistinguishedName { get; set; }

        [JsonProperty("certificate")]
        public string X509PEMEncodedCertificate { get; set; }

        [JsonProperty("formatter", NullValueHandling = NullValueHandling.Ignore)]
        [Obsolete]
        public string LegacyParameterFormatter { get; set; }

        [JsonProperty("insecure_tls")]
        public bool? ConnectionInsecureTLS { get; set; }

        [JsonProperty("last_rotation_tolerance")]
        public long LastRotationTolerance { get; set; }

        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        [Obsolete]
        public string LegacyParameterLength { get; set; }

        [JsonProperty("max_ttl")]
        public long PasswordMaximumTimeToLive { get; set; }

        [JsonProperty("password_policy")]
        public string PasswordPolicy { get; set; }

        [JsonProperty("request_timeout")]
        public long ConnectionRequestTimeout { get; set; }

        [JsonProperty("starttls")]
        public bool? ConnectionStartTLS { get; set; }

        [JsonProperty("tls_max_version")]
        public string TLSMaxVersion { get; set; }

        [JsonProperty("tls_min_version")]
        public string TLSMinVersion { get; set; }

        [JsonProperty("ttl")]
        public long PasswordTimeToLive { get; set; }

        [JsonProperty("upndomain")]
        public string UPNDomain { get; set; }

        [JsonProperty("url")]
        public string ConnectionURL { get; set; }

        [JsonProperty("userdn")]
        public string UserDistinguishedName { get; set; }
    }
}