using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public class IssuerConfigRequestData
    {
        /// <summary>
        /// Comma-separated list of URLs to be used for the CRL distribution points attribute. See also RFC 5280 Section 4.2.1.13.
        /// </summary>
        /// <value>Comma-separated list of URLs to be used for the CRL distribution points attribute. See also RFC 5280 Section 4.2.1.13.</value>
        [JsonPropertyName("crl_distribution_points")]
        public List<string> CrlDistributionPoints { get; set; }

        /// <summary>
        /// Whether or not to enabling templating of the above AIA fields. When templating is enabled the special values &#39;{{issuer_id}}&#39;, &#39;{{cluster_path}}&#39;, &#39;{{cluster_aia_path}}&#39; are available, but the addresses are not checked for URL validity until issuance time. Using &#39;{{cluster_path}}&#39; requires /config/cluster&#39;s &#39;path&#39; member to be set on all PR Secondary clusters and using &#39;{{cluster_aia_path}}&#39; requires /config/cluster&#39;s &#39;aia_path&#39; member to be set on all PR secondary clusters.
        /// </summary>
        /// <value>Whether or not to enabling templating of the above AIA fields. When templating is enabled the special values &#39;{{issuer_id}}&#39;, &#39;{{cluster_path}}&#39;, &#39;{{cluster_aia_path}}&#39; are available, but the addresses are not checked for URL validity until issuance time. Using &#39;{{cluster_path}}&#39; requires /config/cluster&#39;s &#39;path&#39; member to be set on all PR Secondary clusters and using &#39;{{cluster_aia_path}}&#39; requires /config/cluster&#39;s &#39;aia_path&#39; member to be set on all PR secondary clusters.</value>
        [JsonPropertyName("enable_aia_url_templating")]
        public bool EnableAiaUrlTemplating { get; set; }

        /// <summary>
        /// Provide a name to the generated or existing issuer, the name must be unique across all issuers and not be the reserved value &#39;default&#39;
        /// </summary>
        /// <value>Provide a name to the generated or existing issuer, the name must be unique across all issuers and not be the reserved value &#39;default&#39;</value>
        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        /// <summary>
        /// Comma-separated list of URLs to be used for the issuing certificate attribute. See also RFC 5280 Section 4.2.2.1.
        /// </summary>
        /// <value>Comma-separated list of URLs to be used for the issuing certificate attribute. See also RFC 5280 Section 4.2.2.1.</value>
        [JsonPropertyName("issuing_certificates")]
        public List<string> IssuingCertificates { get; set; } = new List<string>();

        /// <summary>
        /// Behavior of leaf&#39;s NotAfter fields: \&quot;err\&quot; to error if the computed NotAfter date exceeds that of this issuer; \&quot;truncate\&quot; to silently truncate to that of this issuer; or \&quot;permit\&quot; to allow this issuance to succeed (with NotAfter exceeding that of an issuer). Note that not all values will results in certificates that can be validated through the entire validity period. It is suggested to use \&quot;truncate\&quot; for intermediate CAs and \&quot;permit\&quot; only for root CAs.
        /// </summary>
        /// <value>Behavior of leaf&#39;s NotAfter fields: \&quot;err\&quot; to error if the computed NotAfter date exceeds that of this issuer; \&quot;truncate\&quot; to silently truncate to that of this issuer; or \&quot;permit\&quot; to allow this issuance to succeed (with NotAfter exceeding that of an issuer). Note that not all values will results in certificates that can be validated through the entire validity period. It is suggested to use \&quot;truncate\&quot; for intermediate CAs and \&quot;permit\&quot; only for root CAs.</value>
        [JsonPropertyName("leaf_not_after_behavior")]
        public string LeafNotAfterBehavior { get; set; }

        /// <summary>
        /// Chain of issuer references to use to build this issuer&#39;s computed CAChain field, when non-empty.
        /// </summary>
        /// <value>Chain of issuer references to use to build this issuer&#39;s computed CAChain field, when non-empty.</value>
        [JsonPropertyName("manual_chain")]
        public List<string> ManualChain { get; set; } = new List<string>();

        /// <summary>
        /// Comma-separated list of URLs to be used for the OCSP servers attribute. See also RFC 5280 Section 4.2.2.1.
        /// </summary>
        /// <value>Comma-separated list of URLs to be used for the OCSP servers attribute. See also RFC 5280 Section 4.2.2.1.</value>
        [JsonPropertyName("ocsp_servers")]
        public List<string> OcspServers { get; set; }

        /// <summary>
        /// Which x509.SignatureAlgorithm name to use for signing CRLs. This parameter allows differentiation between PKCS#1v1.5 and PSS keys and choice of signature hash algorithm. The default (empty string) value is for Go to select the signature algorithm. This can fail if the underlying key does not support the requested signature algorithm, which may not be known at modification time (such as with PKCS#11 managed RSA keys).
        /// </summary>
        /// <value>Which x509.SignatureAlgorithm name to use for signing CRLs. This parameter allows differentiation between PKCS#1v1.5 and PSS keys and choice of signature hash algorithm. The default (empty string) value is for Go to select the signature algorithm. This can fail if the underlying key does not support the requested signature algorithm, which may not be known at modification time (such as with PKCS#11 managed RSA keys).</value>
        [JsonPropertyName("revocation_signature_algorithm")]
        public string RevocationSignatureAlgorithm { get; set; }

        /// <summary>
        /// Comma-separated list (or string slice) of usages for this issuer; valid values are \&quot;read-only\&quot;, \&quot;issuing-certificates\&quot;, \&quot;crl-signing\&quot;, and \&quot;ocsp-signing\&quot;. Multiple values may be specified. Read-only is implicit and always set.
        /// </summary>
        /// <value>Comma-separated list (or string slice) of usages for this issuer; valid values are \&quot;read-only\&quot;, \&quot;issuing-certificates\&quot;, \&quot;crl-signing\&quot;, and \&quot;ocsp-signing\&quot;. Multiple values may be specified. Read-only is implicit and always set.</value>
        [JsonPropertyName("usage")]
        public string Usage { get; set; }
    }
}