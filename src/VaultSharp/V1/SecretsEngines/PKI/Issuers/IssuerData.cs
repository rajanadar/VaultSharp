using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public class IssuerData
    {
        public IssuerData()
        {
            this.Format = IssuerFormat.pem;
            this.ExcludeCnFromSans = false;
            this.KeyRef = "default";
            this.KeyType = PrivateKeyType.rsa;
            this.MaxPathLength = -1;
            this.NotBeforeDuration = "30";
            this.PrivateKeyFormat = IssuerPrivateKeyFormat.pem;
            this.UsePss = false;
        }

        /// <summary>
        ///     Format for returned data. Can be \&quot;pem\&quot;, \&quot;der\&quot;, or \&quot;pem_bundle\&quot;. If \&quot;
        ///     pem_bundle\&quot;, any private key and issuing cert will be appended to the certificate pem. If \&quot;der\&quot;,
        ///     the value will be base64 encoded. Defaults to \&quot;pem\&quot;.
        /// </summary>
        /// <value>
        ///     Format for returned data. Can be \&quot;pem\&quot;, \&quot;der\&quot;, or \&quot;pem_bundle\&quot;. If \&quot;
        ///     pem_bundle\&quot;, any private key and issuing cert will be appended to the certificate pem. If \&quot;der\&quot;,
        ///     the value will be base64 encoded. Defaults to \&quot;pem\&quot;.
        /// </value>
        [JsonPropertyName("format")]
        public IssuerFormat? Format { get; set; }

        /// <summary>
        ///     The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot; and \&quot;ed25519\&quot; are the only
        ///     valid values.
        /// </summary>
        /// <value>
        ///     The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot; and \&quot;ed25519\&quot; are the
        ///     only valid values.
        /// </value>
        [JsonPropertyName("key_type")]
        public PrivateKeyType? KeyType { get; set; }

        /// <summary>
        ///     Format for the returned private key. Generally the default will be controlled by the \&quot;format\&quot; parameter
        ///     as either base64-encoded DER or PEM-encoded DER. However, this can be set to \&quot;pkcs8\&quot; to have the
        ///     returned private key contain base64-encoded pkcs8 or PEM-encoded pkcs8 instead. Defaults to \&quot;der\&quot;.
        /// </summary>
        /// <value>
        ///     Format for the returned private key. Generally the default will be controlled by the \&quot;format\&quot;
        ///     parameter as either base64-encoded DER or PEM-encoded DER. However, this can be set to \&quot;pkcs8\&quot; to have
        ///     the returned private key contain base64-encoded pkcs8 or PEM-encoded pkcs8 instead. Defaults to \&quot;der\&quot;.
        /// </value>
        [JsonPropertyName("private_key_format")]
        public IssuerPrivateKeyFormat? PrivateKeyFormat { get; set; }

        /// <summary>
        ///     The requested Subject Alternative Names, if any, in a comma-delimited list. May contain both DNS names and email
        ///     addresses.
        /// </summary>
        /// <value>
        ///     The requested Subject Alternative Names, if any, in a comma-delimited list. May contain both DNS names and email
        ///     addresses.
        /// </value>
        [JsonPropertyName("alt_names")]
        public string AltNames { get; set; }

        /// <summary>
        ///     The requested common name; if you want more than one, specify the alternative names in the alt_names map. If not
        ///     specified when signing, the common name will be taken from the CSR; other names must still be specified in
        ///     alt_names or ip_sans.
        /// </summary>
        /// <value>
        ///     The requested common name; if you want more than one, specify the alternative names in the alt_names map. If not
        ///     specified when signing, the common name will be taken from the CSR; other names must still be specified in
        ///     alt_names or ip_sans.
        /// </value>
        [JsonPropertyName("common_name")]
        public string CommonName { get; set; }

        /// <summary>
        ///     If set, Country will be set to this value.
        /// </summary>
        /// <value>If set, Country will be set to this value.</value>
        [JsonPropertyName("country")]
        public List<string> Country { get; set; }

        /// <summary>
        ///     If true, the Common Name will not be included in DNS or Email Subject Alternate Names. Defaults to false (CN is
        ///     included).
        /// </summary>
        /// <value>
        ///     If true, the Common Name will not be included in DNS or Email Subject Alternate Names. Defaults to false (CN is
        ///     included).
        /// </value>
        [JsonPropertyName("exclude_cn_from_sans")]
        public bool? ExcludeCnFromSans { get; set; }


        /// <summary>
        ///     The requested IP SANs, if any, in a comma-delimited list
        /// </summary>
        /// <value>The requested IP SANs, if any, in a comma-delimited list</value>
        [JsonPropertyName("ip_sans")]
        public List<string> IpSans { get; set; }

        /// <summary>
        ///     Provide a name to the generated or existing issuer, the name must be unique across all issuers and not be the
        ///     reserved value &#x27;default&#x27;
        /// </summary>
        /// <value>
        ///     Provide a name to the generated or existing issuer, the name must be unique across all issuers and not be the
        ///     reserved value &#x27;default&#x27;
        /// </value>
        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        /// <summary>
        ///     The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or
        ///     4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.
        /// </summary>
        /// <value>
        ///     The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or
        ///     4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.
        /// </value>
        [JsonPropertyName("key_bits")]
        public int? KeyBits { get; set; }

        /// <summary>
        ///     Provide a name to the generated or existing key, the name must be unique across all keys and not be the reserved
        ///     value &#x27;default&#x27;
        /// </summary>
        /// <value>
        ///     Provide a name to the generated or existing key, the name must be unique across all keys and not be the reserved
        ///     value &#x27;default&#x27;
        /// </value>
        [JsonPropertyName("key_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string KeyName { get; set; }

        /// <summary>
        ///     Reference to a existing key; either \&quot;default\&quot; for the configured default key, an identifier or the name
        ///     assigned to the key.
        /// </summary>
        /// <value>
        ///     Reference to a existing key; either \&quot;default\&quot; for the configured default key, an identifier or the
        ///     name assigned to the key.
        /// </value>
        [JsonPropertyName("key_ref")]
        public string KeyRef { get; set; }


        /// <summary>
        ///     If set, Locality will be set to this value.
        /// </summary>
        /// <value>If set, Locality will be set to this value.</value>
        [JsonPropertyName("locality")]
        public List<string> Locality { get; set; }

        /// <summary>
        ///     The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or
        ///     managed_key_name is required. Ignored for other types.
        /// </summary>
        /// <value>
        ///     The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or
        ///     managed_key_name is required. Ignored for other types.
        /// </value>
        [JsonPropertyName("managed_key_id")]
        public string ManagedKeyId { get; set; }

        /// <summary>
        ///     The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or
        ///     managed_key_id is required. Ignored for other types.
        /// </summary>
        /// <value>
        ///     The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or
        ///     managed_key_id is required. Ignored for other types.
        /// </value>
        [JsonPropertyName("managed_key_name")]
        public string ManagedKeyName { get; set; }

        /// <summary>
        ///     The maximum allowable path length
        /// </summary>
        /// <value>The maximum allowable path length</value>
        [JsonPropertyName("max_path_length")]
        public int? MaxPathLength { get; set; }

        /// <summary>
        ///     Set the not after field of the certificate with specified date value. The value format should be given in UTC
        ///     format YYYY-MM-ddTHH:MM:SSZ
        /// </summary>
        /// <value>
        ///     Set the not after field of the certificate with specified date value. The value format should be given in UTC
        ///     format YYYY-MM-ddTHH:MM:SSZ
        /// </value>
        [JsonPropertyName("not_after")]
        public string NotAfter { get; set; }

        /// <summary>
        ///     The duration before now which the certificate needs to be backdated by.
        /// </summary>
        /// <value>The duration before now which the certificate needs to be backdated by.</value>
        [JsonPropertyName("not_before_duration")]
        public string NotBeforeDuration { get; set; }

        /// <summary>
        ///     If set, O (Organization) will be set to this value.
        /// </summary>
        /// <value>If set, O (Organization) will be set to this value.</value>
        [JsonPropertyName("organization")]
        public List<string> Organization { get; set; }

        /// <summary>
        ///     Requested other SANs, in an array with the format &lt;oid&gt;;UTF8:&lt;utf8 string value&gt; for each entry.
        /// </summary>
        /// <value>Requested other SANs, in an array with the format &lt;oid&gt;;UTF8:&lt;utf8 string value&gt; for each entry.</value>
        [JsonPropertyName("other_sans")]
        public List<string> OtherSans { get; set; }

        /// <summary>
        ///     If set, OU (OrganizationalUnit) will be set to this value.
        /// </summary>
        /// <value>If set, OU (OrganizationalUnit) will be set to this value.</value>
        [JsonPropertyName("ou")]
        public List<string> Ou { get; set; }

        /// <summary>
        ///     Domains for which this certificate is allowed to sign or issue child certificates. If set, all DNS names (subject
        ///     and alt) on child certs must be exact matches or subsets of the given domains (see
        ///     https://tools.ietf.org/html/rfc5280#section-4.2.1.10).
        /// </summary>
        /// <value>
        ///     Domains for which this certificate is allowed to sign or issue child certificates. If set, all DNS names
        ///     (subject and alt) on child certs must be exact matches or subsets of the given domains (see
        ///     https://tools.ietf.org/html/rfc5280#section-4.2.1.10).
        /// </value>
        [JsonPropertyName("permitted_dns_domains")]
        public List<string> PermittedDnsDomains { get; set; }

        /// <summary>
        ///     If set, Postal Code will be set to this value.
        /// </summary>
        /// <value>If set, Postal Code will be set to this value.</value>
        [JsonPropertyName("postal_code")]
        public List<string> PostalCode { get; set; }


        /// <summary>
        ///     If set, Province will be set to this value.
        /// </summary>
        /// <value>If set, Province will be set to this value.</value>
        [JsonPropertyName("province")]
        public List<string> Province { get; set; }

        /// <summary>
        ///     The Subject&#x27;s requested serial number, if any. See RFC 4519 Section 2.31 &#x27;serialNumber&#x27; for a
        ///     description of this field. If you want more than one, specify alternative names in the alt_names map using OID
        ///     2.5.4.5. This has no impact on the final certificate&#x27;s Serial Number field.
        /// </summary>
        /// <value>
        ///     The Subject&#x27;s requested serial number, if any. See RFC 4519 Section 2.31 &#x27;serialNumber&#x27; for a
        ///     description of this field. If you want more than one, specify alternative names in the alt_names map using OID
        ///     2.5.4.5. This has no impact on the final certificate&#x27;s Serial Number field.
        /// </value>
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        ///     The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for
        ///     SHA-2-512. Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the
        ///     curve size for NIST P-Curves).
        /// </summary>
        /// <value>
        ///     The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for
        ///     SHA-2-512. Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the
        ///     curve size for NIST P-Curves).
        /// </value>
        [JsonPropertyName("signature_bits")]
        public int? SignatureBits { get; set; }

        /// <summary>
        ///     If set, Street Address will be set to this value.
        /// </summary>
        /// <value>If set, Street Address will be set to this value.</value>
        [JsonPropertyName("street_address")]
        public List<string> StreetAddress { get; set; }

        /// <summary>
        ///     The requested Time To Live for the certificate; sets the expiration date. If not specified the role default,
        ///     backend default, or system default TTL is used, in that order. Cannot be larger than the mount max TTL. Note: this
        ///     only has an effect when generating a CA cert or signing a CA cert, not when generating a CSR for an intermediate
        ///     CA.
        /// </summary>
        /// <value>
        ///     The requested Time To Live for the certificate; sets the expiration date. If not specified the role default,
        ///     backend default, or system default TTL is used, in that order. Cannot be larger than the mount max TTL. Note: this
        ///     only has an effect when generating a CA cert or signing a CA cert, not when generating a CSR for an intermediate
        ///     CA.
        /// </value>
        [JsonPropertyName("ttl")]
        public string Ttl { get; set; }

        /// <summary>
        ///     The requested URI SANs, if any, in a comma-delimited list.
        /// </summary>
        /// <value>The requested URI SANs, if any, in a comma-delimited list.</value>
        [JsonPropertyName("uri_sans")]
        public List<string> UriSans { get; set; }

        /// <summary>
        ///     Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.
        /// </summary>
        /// <value>Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.</value>
        [JsonPropertyName("use_pss")]
        public bool? UsePss { get; set; }
    }
}
