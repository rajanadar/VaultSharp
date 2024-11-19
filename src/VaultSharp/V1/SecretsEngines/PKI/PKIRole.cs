using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class PKIRole
    {
        /// <summary>
        /// If set, clients can request certificates for any domain, regardless of allowed_domains restrictions. See the documentation for more information.
        /// </summary>
        [JsonPropertyName("allow_any_name")]
        public bool AllowAnyName { get; set; }

        /// <summary>
        /// If set, clients can request certificates for the base domains themselves, e.g. "example.com" of domains listed in allowed_domains. 
        /// This is a separate option as in some cases this can be considered a security threat. See the documentation for more information.
        /// </summary>
        [JsonPropertyName("allow_bare_domains")]
        public bool AllowBareDomains { get; set; }

        /// <summary>
        /// If set, domains specified in allowed_domains can include shell-style glob patterns, e.g. "ftp*.example.com". 
        /// See the documentation for more information.
        /// </summary>
        [JsonPropertyName("allow_glob_domains")]
        public bool AllowGlobDomains { get; set; }

        /// <summary>
        /// If set, IP Subject Alternative Names are allowed. Any valid IP is accepted and No authorization checking is performed.
        /// </summary>
        [JsonPropertyName("allow_ip_sans")]
        public bool AllowIpSans { get; set; }

        /// <summary>
        /// Whether to allow "localhost" and "localdomain" as a valid common name in a request, independent of allowed_domains value.
        /// </summary>
        [JsonPropertyName("allow_localhost")]
        public bool AllowLocalhost { get; set; }

        /// <summary>
        /// If set, clients can request certificates for subdomains of domains listed in allowed_domains, including wildcard subdomains. 
        /// See the documentation for more information.
        /// </summary>
        [JsonPropertyName("allow_subdomains")]
        public bool AllowSubdomains { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("allow_token_displayname")]
        public bool AllowTokenDisplayname { get; set; }

        /// <summary>
        /// If set, allows certificates with wildcards in the common name to be issued, conforming to RFC 6125's Section 6.4.3; e.g., 
        /// ".example.net" or "bz.example.net". See the documentation for more information.
        /// </summary>
        [JsonPropertyName("allow_wildcard_certificates")]
        public bool AllowWildcardIdentifiers { get; set; }

        /// <summary>
        /// Specifies the domains this role is allowed to issue certificates for. This is used with the allow_bare_domains, allow_subdomains, 
        /// and allow_glob_domains to determine matches for the common name, DNS-typed SAN entries, and Email-typed SAN entries of certificates. 
        /// See the documentation for more information. This parameter accepts a comma-separated string or list of domains.
        /// </summary>
        [JsonPropertyName("allowed_domains")]
        public List<string> AllowedDomains { get; set; }

        /// <summary>
        /// If set, Allowed domains can be specified using identity template policies. Non-templated domains are also permitted.
        /// </summary>
        [JsonPropertyName("allowed_domains_template")]
        public bool AllowedDomainsTemplate { get; set; }

        /// <summary>
        /// If set, an array of allowed serial numbers to put in Subject. These values support globbing.
        /// </summary>
        [JsonPropertyName("allowed_serial_numbers")]
        public List<string> AllowedSerialNumbers { get; set; }

        /// <summary>
        /// If set, an array of allowed URIs for URI Subject Alternative Names. Any valid URI is accepted, these values support globbing.
        /// </summary>
        [JsonPropertyName("allowed_uri_sans")]
        public List<string> AllowedUriSans { get; set; }

        /// <summary>
        /// If set, Allowed URI SANs can be specified using identity template policies. Non-templated URI SANs are also permitted.
        /// </summary>
        [JsonPropertyName("allowed_uri_sans_template")]
        public bool AllowedUriSansTemplate { get; set; }

        /// <summary>
        /// If set, an array of allowed user-ids to put in user system login name specified here: https://www.rfc-editor.org/rfc/rfc1274#section-9.3.1
        /// </summary>
        [JsonPropertyName("allowed_user_ids")]
        public List<string> AllowedUserIds { get; set; }

        /// <summary>
        /// Mark Basic Constraints valid when issuing non-CA certificates.
        /// </summary>
        [JsonPropertyName("basic_constraints_valid_for_non_ca")]
        public bool BasicConstraintsValidForNonCA { get; set; }

        /// <summary>
        /// If set, certificates are flagged for client auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        [JsonPropertyName("client_flag")]
        public bool ClientFlag { get; set; }

        /// <summary>
        /// List of allowed validations to run against the Common Name field. Values can include 'email' to validate the CN is a email address, 
        /// 'hostname' to validate the CN is a valid hostname (potentially including wildcards). When multiple validations are specified, 
        /// these take OR semantics (either email OR hostname are allowed). The special value 'disabled' allows disabling all CN name validations, 
        /// allowing for arbitrary non-Hostname, non-Email address CNs.
        /// </summary>
        [JsonPropertyName("cn_validations")]
        public List<string> CNVAlidations { get; set; }

        /// <summary>
        /// If set, certificates are flagged for code signing use. Defaults to false. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        [JsonPropertyName("code_signing_flag")]
        public bool CodeSigningFlag { get; set; }

        /// <summary>
        /// If set, Country will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// If set, certificates are flagged for email protection use. Defaults to false. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        [JsonPropertyName("email_protection_flag")]
        public bool EmailProtectionFlag { get; set; }

        /// <summary>
        /// If set, only valid host names are allowed for CN and DNS SANs, and the host part of email addresses. Defaults to true.
        /// </summary>
        [JsonPropertyName("enforce_hostnames")]
        public bool EnforceHostnames { get; set; }

        /// <summary>
        /// A comma-separated string or list of extended key usages. Valid values can be found at https://golang.org/pkg/crypto/x509/#ExtKeyUsage -- 
        /// simply drop the "ExtKeyUsage" part of the name. To remove all key usages from being set, set this value to an empty list. 
        /// See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        [JsonPropertyName("ext_key_usage")]
        public List<string> ExtKeyUsage { get; set; }

        /// <summary>
        /// If set, certificates issued/signed against this role will have Vault leases attached to them. Defaults to "false". 
        /// Certificates can be added to the CRL by "vault revoke <lease_id>" when certificates are associated with leases. 
        /// It can also be done using the "pki/revoke" endpoint. However, when lease generation is disabled, invoking "pki/revoke" would be the only 
        /// way to add the certificates to the CRL. When large number of certificates are generated with long lifetimes, it is recommended that lease generation be disabled,
        /// as large amount of leases adversely affect the startup time of Vault.
        /// </summary>
        [JsonPropertyName("generate_lease")]
        public bool GenerateLease { get; set; }

        /// <summary>
        /// Reference to the issuer used to sign requests serviced by this role.
        /// </summary>
        [JsonPropertyName("issuer_ref")]
        public string IssuerRef { get; set; }

        /// <summary>
        /// The number of bits to use. Allowed values are: 
        /// 0 (universal default); 
        /// with rsa key_type: 2048 (default), 3072, or 4096; 
        /// with ec key_type: 224, 256 (default), 384, or 521; 
        /// ignored with ed25519.
        /// </summary>
        [JsonPropertyName("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// The type of key to use; defaults to RSA. "rsa" "ec", "ed25519" and "any" are the only valid values.
        /// </summary>
        [JsonPropertyName("key_type")]
        public string KeyType { get; set; }

        /// <summary>
        /// A comma-separated string or list of key usages (not extended key usages). 
        /// Valid values can be found at https://golang.org/pkg/crypto/x509/#KeyUsage -- simply drop the "KeyUsage" part of the name. 
        /// To remove all key usages from being set, set this value to an empty list. See also RFC 5280 Section 4.2.1.3.
        /// </summary>
        [JsonPropertyName("key_usage")]
        public List<string> KeyUsage { get; set; }

        /// <summary>
        /// If set, Locality will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("locality")]
        public List<string> Locality { get; set; }

        /// <summary>
        /// The maximum allowed lease duration. If not set, defaults to the system maximum lease TTL.
        /// </summary>
        [JsonPropertyName("max_ttl")]
        public long MaxTTL { get; set; }

        /// <summary>
        /// If set, certificates issued/signed against this role will not be stored in the storage backend. 
        /// This can improve performance when issuing large numbers of certificates. However, certificates issued in this way cannot be enumerated or revoked, 
        /// so this option is recommended only for certificates that are non-sensitive, or extremely short-lived. 
        /// This option implies a value of "false" for "generate_lease".
        /// </summary>
        [JsonPropertyName("no_store")]
        public bool NoStore { get; set; }

        /// <summary>
        /// If set, if a client attempts to issue or sign a certificate with attached cert_metadata to store, the issuance / signing instead fails.
        /// </summary>
        [JsonPropertyName("no_store_metadata")]
        public bool NoStoreMetadata { get; set; }

        /// <summary>
        /// Set the not after field of the certificate with specified date value. The value format should be given in UTC format YYYY-MM-ddTHH:MM:SSZ.
        /// </summary>
        [JsonPropertyName("not_after")]
        public string NotAfter { get; set; }

        /// <summary>
        /// The duration in seconds before now which the certificate needs to be backdated by.
        /// </summary>
        [JsonPropertyName("not_before_duration")]
        public long NotBeforeDuration { get; set; }

        /// <summary>
        /// If set, O (Organization) will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("organization")]
        public List<string> Organization { get; set; }

        /// <summary>
        /// If set, OU (OrganizationalUnit) will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("ou")]
        public List<string> OrganizationalUnit { get; set; }

        /// <summary>
        /// A comma-separated string or list of policy OIDs, or a JSON list of qualified policy information, 
        /// which must include an oid, and may include a notice and/or cps url, 
        /// using the form [{"oid"="1.3.6.1.4.1.7.8","notice"="I am a user Notice"}, {"oid"="1.3.6.1.4.1.44947.1.2.4 ","cps"="https://example.com"}].
        /// </summary>
        [JsonPropertyName("policy_identifiers")]
        public List<string> PolicyIdentifiers { get; set; }

        /// <summary>
        /// If set, Postal Code will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("postal_code")]
        public List<string> PostalCode { get; set; }

        /// <summary>
        /// If set, Province will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("province")]
        public List<string> Province { get; set; }

        /// <summary>
        /// If set to false, makes the 'common_name' field optional while generating a certificate.
        /// </summary>
        [JsonPropertyName("require_cn")]
        public bool RequireCN { get; set; }

        /// <summary>
        /// If set, certificates are flagged for server auth use. Defaults to true. See also RFC 5280 Section 4.2.1.12.
        /// </summary>
        [JsonPropertyName("server_flag")]
        public bool ServerFlag { get; set; }

        /// <summary>
        /// The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for SHA-2-512. 
        /// Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the curve size for NIST P-Curves).
        /// </summary>
        [JsonPropertyName("signature_bits")]
        public int SignatureBits { get; set; }

        /// <summary>
        /// If set, Street Address will be set to this value in certificates issued by this role.
        /// </summary>
        [JsonPropertyName("street_address")]
        public List<string> StreetAddress { get; set; }

        /// <summary>
        /// The lease duration (validity period of the certificate) if no specific lease duration is requested. 
        /// The lease duration controls the expiration of certificates issued by this backend. Defaults to the system default value or the value of max_ttl, 
        /// whichever is shorter.
        /// </summary>
        [JsonPropertyName("ttl")]
        public long TTL { get; set; }

        /// <summary>
        /// If set, when used with a signing profile, the common name in the CSR will be used. This does not include any requested Subject Alternative Names; 
        /// use use_csr_sans for that. 
        /// Defaults to true.
        /// </summary>
        [JsonPropertyName("use_csr_common_name")]
        public bool UseCsrCommonName { get; set; }

        /// <summary>
        /// If set, when used with a signing profile, the SANs in the CSR will be used. This does not include the Common Name (cn); use use_csr_common_name for that. 
        /// Defaults to true.
        /// </summary>
        [JsonPropertyName("use_csr_sans")]
        public bool UseCsrSans { get; set; }

        /// <summary>
        /// Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.
        /// </summary>
        [JsonPropertyName("use_pss")]
        public bool UsePss { get; set; }
    }
}
