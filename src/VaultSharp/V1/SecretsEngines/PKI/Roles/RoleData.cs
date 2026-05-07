using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace VaultSharp.V1.SecretsEngines.PKI.Roles
{
    public class RoleData
    {
        public RoleData()
        {
            this.IssuerRef = @"default";
            this.NotBeforeDuration = 30;
            this.AllowIpSans = true;
            this.AllowLocalhost = true;
            this.AllowWildcardCertificates = true;
            this.AllowedUriSansTemplate = false;
            this.KeyType = PrivateKeyRoleType.rsa;
            this.RequireCn = true;
            this.UseCsrCommonName = true;
            this.UseCsrSans = true;
            this.UsePss = false;
        }

        [JsonIgnore]
        public string Name { get; set; }

        /// <summary>
        /// The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot;, \&quot;ed25519\&quot; and \&quot;any\&quot; are the only valid values.
        /// </summary>
        /// <value>The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot;, \&quot;ed25519\&quot; and \&quot;any\&quot; are the only valid values.</value>
        [JsonPropertyName("key_type")]
        public PrivateKeyRoleType KeyType { get; set; }

        /// <summary>
        /// If set, clients can request certificates for any domain, regardless of allowed_domains restrictions. See the documentation for more information.
        /// </summary>
        /// <value>If set, clients can request certificates for any domain, regardless of allowed_domains restrictions. See the documentation for more information.</value>
        [JsonPropertyName("allow_any_name")]
        public bool AllowAnyName { get; set; }

        /// <summary>
        /// If set, clients can request certificates for the base domains themselves, e.g. \&quot;example.com\&quot; of domains listed in allowed_domains. This is a separate option as in some cases this can be considered a security threat. See the documentation for more information.
        /// </summary>
        /// <value>If set, clients can request certificates for the base domains themselves, e.g. \&quot;example.com\&quot; of domains listed in allowed_domains. This is a separate option as in some cases this can be considered a security threat. See the documentation for more information.</value>
        [JsonPropertyName("allow_bare_domains")]
        public bool AllowBareDomains { get; set; }

        /// <summary>
        /// If set, domains specified in allowed_domains can include shell-style glob patterns, e.g. \&quot;ftp*.example.com\&quot;. See the documentation for more information.
        /// </summary>
        /// <value>If set, domains specified in allowed_domains can include shell-style glob patterns, e.g. \&quot;ftp*.example.com\&quot;. See the documentation for more information.</value>
        [JsonPropertyName("allow_glob_domains")]
        public bool AllowGlobDomains { get; set; }

        /// <summary>
        /// If set, IP Subject Alternative Names are allowed. Any valid IP is accepted and No authorization checking is performed.
        /// </summary>
        /// <value>If set, IP Subject Alternative Names are allowed. Any valid IP is accepted and No authorization checking is performed.</value>
        [JsonPropertyName("allow_ip_sans")]
        public bool AllowIpSans { get; set; }

        /// <summary>
        /// Whether to allow \&quot;localhost\&quot; and \&quot;localdomain\&quot; as a valid common name in a request, independent of allowed_domains value.
        /// </summary>
        /// <value>Whether to allow \&quot;localhost\&quot; and \&quot;localdomain\&quot; as a valid common name in a request, independent of allowed_domains value.</value>
        [JsonPropertyName("allow_localhost")]
        public bool AllowLocalhost { get; set; }

        /// <summary>
        /// If set, clients can request certificates for subdomains of domains listed in allowed_domains, including wildcard subdomains. See the documentation for more information.
        /// </summary>
        /// <value>If set, clients can request certificates for subdomains of domains listed in allowed_domains, including wildcard subdomains. See the documentation for more information.</value>
        [JsonPropertyName("allow_subdomains")]
        public bool AllowSubdomains { get; set; }

        /// <summary>
        /// If set, allows certificates with wildcards in the common name to be issued, conforming to RFC 6125&#39;s Section 6.4.3; e.g., \&quot;*.example.net\&quot; or \&quot;b*z.example.net\&quot;. See the documentation for more information.
        /// </summary>
        /// <value>If set, allows certificates with wildcards in the common name to be issued, conforming to RFC 6125&#39;s Section 6.4.3; e.g., \&quot;*.example.net\&quot; or \&quot;b*z.example.net\&quot;. See the documentation for more information.</value>
        [JsonPropertyName("allow_wildcard_certificates")]
        public bool AllowWildcardCertificates { get; set; }

        /// <summary>
        /// Specifies the domains this role is allowed to issue certificates for. This is used with the allow_bare_domains, allow_subdomains, and allow_glob_domains to determine matches for the common name, DNS-typed SAN entries, and Email-typed SAN entries of certificates. See the documentation for more information. This parameter accepts a comma-separated string or list of domains.
        /// </summary>
        /// <value>Specifies the domains this role is allowed to issue certificates for. This is used with the allow_bare_domains, allow_subdomains, and allow_glob_domains to determine matches for the common name, DNS-typed SAN entries, and Email-typed SAN entries of certificates. See the documentation for more information. This parameter accepts a comma-separated string or list of domains.</value>
        [JsonPropertyName("allowed_domains")]
        public List<string> AllowedDomains { get; set; }

        /// <summary>
        /// If set, Allowed domains can be specified using identity template policies. Non-templated domains are also permitted.
        /// </summary>
        /// <value>If set, Allowed domains can be specified using identity template policies. Non-templated domains are also permitted.</value>
        [JsonPropertyName("allowed_domains_template")]
        public bool AllowedDomainsTemplate { get; set; }

        /// <summary>
        /// If set, an array of allowed other names to put in SANs. These values support globbing and must be in the format &lt;oid&gt;;&lt;type&gt;:&lt;value&gt;. Currently only \&quot;utf8\&quot; is a valid type. All values, including globbing values, must use this syntax, with the exception being a single \&quot;*\&quot; which allows any OID and any value (but type must still be utf8).
        /// </summary>
        /// <value>If set, an array of allowed other names to put in SANs. These values support globbing and must be in the format &lt;oid&gt;;&lt;type&gt;:&lt;value&gt;. Currently only \&quot;utf8\&quot; is a valid type. All values, including globbing values, must use this syntax, with the exception being a single \&quot;*\&quot; which allows any OID and any value (but type must still be utf8).</value>
        [JsonPropertyName("allowed_other_sans")]
        public List<string> AllowedOtherSans { get; set; }

        /// <summary>
        /// If set, an array of allowed serial numbers to put in Subject. These values support globbing.
        /// </summary>
        /// <value>If set, an array of allowed serial numbers to put in Subject. These values support globbing.</value>
        [JsonPropertyName("allowed_serial_numbers")]
        public List<string> AllowedSerialNumbers { get; set; }

        /// <summary>
        /// If set, an array of allowed URIs for URI Subject Alternative Names. Any valid URI is accepted, these values support globbing.
        /// </summary>
        /// <value>If set, an array of allowed URIs for URI Subject Alternative Names. Any valid URI is accepted, these values support globbing.</value>
        [JsonPropertyName("allowed_uri_sans")]
        public List<string> AllowedUriSans { get; set; }

        /// <summary>
        /// If set, Allowed URI SANs can be specified using identity template policies. Non-templated URI SANs are also permitted.
        /// </summary>
        /// <value>If set, Allowed URI SANs can be specified using identity template policies. Non-templated URI SANs are also permitted.</value>
        [JsonPropertyName("allowed_uri_sans_template")]
        public bool AllowedUriSansTemplate { get; set; }

        /// <summary>
        /// If set, an array of allowed user-ids to put in user system login name specified here: https://www.rfc-editor.org/rfc/rfc1274#section-9.3.1
        /// </summary>
        /// <value>If set, an array of allowed user-ids to put in user system login name specified here: https://www.rfc-editor.org/rfc/rfc1274#section-9.3.1</value>
        [JsonPropertyName("allowed_user_ids")]
        public List<string> AllowedUserIds { get; set; }

        /// <summary>
        /// Backend Type
        /// </summary>
        /// <value>Backend Type</value>
        [JsonPropertyName("backend")]
        public string Backend { get; set; }

        /// <summary>
        /// Mark Basic Constraints valid when issuing non-CA certificates.
        /// </summary>
        /// <value>Mark Basic Constraints valid when issuing non-CA certificates.</value>
        [JsonPropertyName("basic_constraints_valid_for_non_ca")]
        public bool BasicConstraintsValidForNonCa { get; set; }

        /// <summary>
        /// If set, certificates are flagged for client auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        /// <value>If set, certificates are flagged for client auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.</value>
        [JsonPropertyName("client_flag")]
        public bool ClientFlag { get; set; }

        /// <summary>
        /// List of allowed validations to run against the Common Name field. Values can include &#39;email&#39; to validate the CN is a email address, &#39;hostname&#39; to validate the CN is a valid hostname (potentially including wildcards). When multiple validations are specified, these take OR semantics (either email OR hostname are allowed). The special value &#39;disabled&#39; allows disabling all CN name validations, allowing for arbitrary non-Hostname, non-Email address CNs.
        /// </summary>
        /// <value>List of allowed validations to run against the Common Name field. Values can include &#39;email&#39; to validate the CN is a email address, &#39;hostname&#39; to validate the CN is a valid hostname (potentially including wildcards). When multiple validations are specified, these take OR semantics (either email OR hostname are allowed). The special value &#39;disabled&#39; allows disabling all CN name validations, allowing for arbitrary non-Hostname, non-Email address CNs.</value>
        [JsonPropertyName("cn_validations")]
        public List<string> CnValidations { get; set; }

        /// <summary>
        /// If set, certificates are flagged for code signing use. Defaults to false. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        /// <value>If set, certificates are flagged for code signing use. Defaults to false. See also RFC 5280 Section 4.2.1.12.</value>
        [JsonPropertyName("code_signing_flag")]
        public bool CodeSigningFlag { get; set; }

        /// <summary>
        /// If set, Country will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, Country will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("country")]
        public List<string> Country { get; set; }

        /// <summary>
        /// If set, certificates are flagged for email protection use. Defaults to false. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        /// <value>If set, certificates are flagged for email protection use. Defaults to false. See also RFC 5280 Section 4.2.1.12.</value>
        [JsonPropertyName("email_protection_flag")]
        public bool EmailProtectionFlag { get; set; }

        /// <summary>
        /// If set, only valid host names are allowed for CN and DNS SANs, and the host part of email addresses. Defaults to true.
        /// </summary>
        /// <value>If set, only valid host names are allowed for CN and DNS SANs, and the host part of email addresses. Defaults to true.</value>
        [JsonPropertyName("enforce_hostnames")]
        public bool EnforceHostnames { get; set; }

        /// <summary>
        /// A comma-separated string or list of extended key usages. Valid values can be found at https://golang.org/pkg/crypto/x509/#ExtKeyUsage - - simply drop the \&quot;ExtKeyUsage\&quot; part of the name. To remove all key usages from being set, set this value to an empty list. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        /// <value>A comma-separated string or list of extended key usages. Valid values can be found at https://golang.org/pkg/crypto/x509/#ExtKeyUsage - - simply drop the \&quot;ExtKeyUsage\&quot; part of the name. To remove all key usages from being set, set this value to an empty list. See also RFC 5280 Section 4.2.1.12.</value>
        [JsonPropertyName("ext_key_usage")]
        public List<string> ExtKeyUsage { get; set; }

        /// <summary>
        /// A comma-separated string or list of extended key usage oids.
        /// </summary>
        /// <value>A comma-separated string or list of extended key usage oids.</value>
        [JsonPropertyName("ext_key_usage_oids")]
        public List<string> ExtKeyUsageOids { get; set; }

        /// <summary>
        /// If set, certificates issued/signed against this role will have Vault leases attached to them. Defaults to \&quot;false\&quot;. Certificates can be added to the CRL by \&quot;vault revoke &lt;lease_id&gt;\&quot; when certificates are associated with leases. It can also be done using the \&quot;pki/revoke\&quot; endpoint. However, when lease generation is disabled, invoking \&quot;pki/revoke\&quot; would be the only way to add the certificates to the CRL. When large number of certificates are generated with long lifetimes, it is recommended that lease generation be disabled, as large amount of leases adversely affect the startup time of Vault.
        /// </summary>
        /// <value>If set, certificates issued/signed against this role will have Vault leases attached to them. Defaults to \&quot;false\&quot;. Certificates can be added to the CRL by \&quot;vault revoke &lt;lease_id&gt;\&quot; when certificates are associated with leases. It can also be done using the \&quot;pki/revoke\&quot; endpoint. However, when lease generation is disabled, invoking \&quot;pki/revoke\&quot; would be the only way to add the certificates to the CRL. When large number of certificates are generated with long lifetimes, it is recommended that lease generation be disabled, as large amount of leases adversely affect the startup time of Vault.</value>
        [JsonPropertyName("generate_lease")]
        public bool GenerateLease { get; set; }

        /// <summary>
        /// Reference to the issuer used to sign requests serviced by this role.
        /// </summary>
        /// <value>Reference to the issuer used to sign requests serviced by this role.</value>
        [JsonPropertyName("issuer_ref")]
        public string IssuerRef { get; set; }

        /// <summary>
        /// The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or 4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.
        /// </summary>
        /// <value>The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or 4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.</value>
        [JsonPropertyName("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// A comma-separated string or list of key usages (not extended key usages). Valid values can be found at https://golang.org/pkg/crypto/x509/#KeyUsage - - simply drop the \&quot;KeyUsage\&quot; part of the name. To remove all key usages from being set, set this value to an empty list. See also RFC 5280 Section 4.2.1.3.
        /// </summary>
        /// <value>A comma-separated string or list of key usages (not extended key usages). Valid values can be found at https://golang.org/pkg/crypto/x509/#KeyUsage - - simply drop the \&quot;KeyUsage\&quot; part of the name. To remove all key usages from being set, set this value to an empty list. See also RFC 5280 Section 4.2.1.3.</value>
        [JsonPropertyName("key_usage")]
        public List<string> KeyUsage { get; set; }

        /// <summary>
        /// If set, Locality will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, Locality will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("locality")]
        public List<string> Locality { get; set; }

        /// <summary>
        /// The maximum allowed lease duration. If not set, defaults to the system maximum lease TTL.
        /// </summary>
        /// <value>The maximum allowed lease duration. If not set, defaults to the system maximum lease TTL.</value>
        [JsonPropertyName("max_ttl")]
        public long MaxTtl { get; set; }

        /// <summary>
        /// If set, certificates issued/signed against this role will not be stored in the storage backend. This can improve performance when issuing large numbers of certificates. However, certificates issued in this way cannot be enumerated or revoked, so this option is recommended only for certificates that are non-sensitive, or extremely short-lived. This option implies a value of \&quot;false\&quot; for \&quot;generate_lease\&quot;.
        /// </summary>
        /// <value>If set, certificates issued/signed against this role will not be stored in the storage backend. This can improve performance when issuing large numbers of certificates. However, certificates issued in this way cannot be enumerated or revoked, so this option is recommended only for certificates that are non-sensitive, or extremely short-lived. This option implies a value of \&quot;false\&quot; for \&quot;generate_lease\&quot;.</value>
        [JsonPropertyName("no_store")]
        public bool NoStore { get; set; }

        /// <summary>
        /// Set the not after field of the certificate with specified date value. The value format should be given in UTC format YYYY-MM-ddTHH:MM:SSZ.
        /// </summary>
        /// <value>Set the not after field of the certificate with specified date value. The value format should be given in UTC format YYYY-MM-ddTHH:MM:SSZ.</value>
        [JsonPropertyName("not_after")]
        public string NotAfter { get; set; }

        /// <summary>
        /// The duration before now which the certificate needs to be backdated by.
        /// </summary>
        /// <value>The duration before now which the certificate needs to be backdated by.</value>
        [JsonPropertyName("not_before_duration")]
        public long NotBeforeDuration { get; set; }

        /// <summary>
        /// If set, O (Organization) will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, O (Organization) will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("organization")]
        public List<string> Organization { get; set; }

        /// <summary>
        /// If set, OU (OrganizationalUnit) will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, OU (OrganizationalUnit) will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("ou")]
        public List<string> Ou { get; set; }

        /// <summary>
        /// A comma-separated string or list of policy OIDs, or a JSON list of qualified policy information, which must include an oid, and may include a notice and/or cps url, using the form [{\&quot;oid\&quot;&#x3D;\&quot;1.3.6.1.4.1.7.8\&quot;,\&quot;notice\&quot;&#x3D;\&quot;I am a user Notice\&quot;}, {\&quot;oid\&quot;&#x3D;\&quot;1.3.6.1.4.1.44947.1.2.4 \&quot;,\&quot;cps\&quot;&#x3D;\&quot;https://example.com\&quot;}].
        /// </summary>
        /// <value>A comma-separated string or list of policy OIDs, or a JSON list of qualified policy information, which must include an oid, and may include a notice and/or cps url, using the form [{\&quot;oid\&quot;&#x3D;\&quot;1.3.6.1.4.1.7.8\&quot;,\&quot;notice\&quot;&#x3D;\&quot;I am a user Notice\&quot;}, {\&quot;oid\&quot;&#x3D;\&quot;1.3.6.1.4.1.44947.1.2.4 \&quot;,\&quot;cps\&quot;&#x3D;\&quot;https://example.com\&quot;}].</value>
        [JsonPropertyName("policy_identifiers")]
        public List<string> PolicyIdentifiers { get; set; }

        /// <summary>
        /// If set, Postal Code will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, Postal Code will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("postal_code")]
        public List<string> PostalCode { get; set; }

        /// <summary>
        /// If set, Province will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, Province will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("province")]
        public List<string> Province { get; set; }

        /// <summary>
        /// If set to false, makes the &#39;common_name&#39; field optional while generating a certificate.
        /// </summary>
        /// <value>If set to false, makes the &#39;common_name&#39; field optional while generating a certificate.</value>
        [JsonPropertyName("require_cn")]
        public bool RequireCn { get; set; }

        /// <summary>
        /// If set, certificates are flagged for server auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        /// <value>If set, certificates are flagged for server auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.</value>
        [JsonPropertyName("server_flag")]
        public bool ServerFlag { get; set; }

        /// <summary>
        /// The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for SHA-2-512. Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the curve size for NIST P-Curves).
        /// </summary>
        /// <value>The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for SHA-2-512. Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the curve size for NIST P-Curves).</value>
        [JsonPropertyName("signature_bits")]
        public int SignatureBits { get; set; }

        /// <summary>
        /// If set, Street Address will be set to this value in certificates issued by this role.
        /// </summary>
        /// <value>If set, Street Address will be set to this value in certificates issued by this role.</value>
        [JsonPropertyName("street_address")]
        public List<string> StreetAddress { get; set; }

        /// <summary>
        /// The lease duration (validity period of the certificate) if no specific lease duration is requested. The lease duration controls the expiration of certificates issued by this backend. Defaults to the system default value or the value of max_ttl, whichever is shorter.
        /// </summary>
        /// <value>The lease duration (validity period of the certificate) if no specific lease duration is requested. The lease duration controls the expiration of certificates issued by this backend. Defaults to the system default value or the value of max_ttl, whichever is shorter.</value>
        [JsonPropertyName("ttl")]
        public long Ttl { get; set; }

        /// <summary>
        /// If set, when used with a signing profile, the common name in the CSR will be used. This does *not* include any requested Subject Alternative Names; use use_csr_sans for that. Defaults to true.
        /// </summary>
        /// <value>If set, when used with a signing profile, the common name in the CSR will be used. This does *not* include any requested Subject Alternative Names; use use_csr_sans for that. Defaults to true.</value>
        [JsonPropertyName("use_csr_common_name")]
        public bool UseCsrCommonName { get; set; }

        /// <summary>
        /// If set, when used with a signing profile, the SANs in the CSR will be used. This does *not* include the Common Name (cn); use use_csr_common_name for that. Defaults to true.
        /// </summary>
        /// <value>If set, when used with a signing profile, the SANs in the CSR will be used. This does *not* include the Common Name (cn); use use_csr_common_name for that. Defaults to true.</value>
        [JsonPropertyName("use_csr_sans")]
        public bool UseCsrSans { get; set; }

        /// <summary>
        /// Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.
        /// </summary>
        /// <value>Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.</value>
        [JsonPropertyName("use_pss")]
        public bool UsePss { get; set; }
    }
}