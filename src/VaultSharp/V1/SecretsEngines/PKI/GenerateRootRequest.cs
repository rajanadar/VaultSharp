using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class GenerateRootRequest
    {
        /// <summary>
        /// The requested Subject Alternative Names, if any, in a comma-delimited list. May contain both DNS names and email addresses.
        /// </summary>
        [JsonPropertyName("alt_names")]
        public string AltNames { get; set; }

        /// <summary>
        /// The requested common name; if you want more than one, specify the alternative names in the alt_names map. If not specified when signing, 
        /// the common name will be taken from the CSR; other names must still be specified in alt_names or ip_sans.
        /// </summary>
        [JsonPropertyName("common_name")]
        public string CommonName { get; set; }

        /// <summary>
        /// If set, Country will be set to this value.
        /// </summary>
        [JsonPropertyName("country")]
        public List<string> Country { get; set; }

        /// <summary>
        /// If true, the Common Name will not be included in DNS or Email Subject Alternate Names. Defaults to false (CN is included).
        /// </summary>
        [JsonPropertyName("exclude_cn_from_sans")]
        public bool ExcludeCNFromSans { get; set; }

        /// <summary>
        /// Format for returned data. Can be "pem", "der", or "pem_bundle". If "pem_bundle", any private key and issuing cert will be appended to the certificate pem. 
        /// If "der", the value will be base64 encoded. 
        /// Defaults to "pem".
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// The requested IP SANs, if any
        /// </summary>
        [JsonPropertyName("ip_sans")]
        public List<string> IpSans { get; set; }

        /// <summary>
        /// Provide a name to the generated or existing issuer, the name must be unique across all issuers and not be the reserved value 'default'
        /// </summary>
        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        /// <summary>
        /// The number of bits to use. Allowed values are 0 (universal default); 
        /// with rsa key_type: 2048 (default), 3072, 4096 or 8192; 
        /// with ec key_type: 224, 256 (default), 384, or 521; 
        /// ignored with ed25519.
        /// </summary>
        [JsonPropertyName("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// Provide a name to the generated or existing key, the name must be unique across all keys and not be the reserved value 'default'
        /// </summary>
        [JsonPropertyName("key_name")]
        public string KeyName { get; set; }

        /// <summary>
        /// Reference to a existing key; either "default" for the configured default key, an identifier or the name assigned to the key.
        /// </summary>
        [JsonPropertyName("key_ref")]
        public string KeyRef { get; set; }

        /// <summary>
        /// The type of key to use; 
        /// defaults to RSA. 
        /// "rsa" "ec" and "ed25519" are the only valid values.
        /// </summary>
        [JsonPropertyName("key_type")]
        public string KeyType { get; set; }

        /// <summary>
        /// If set, Locality will be set to this value.
        /// </summary>
        [JsonPropertyName("locality")]
        public List<string> Locality { get; set; }

        /// <summary>
        /// The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_name is required. 
        /// Ignored for other types.
        /// </summary>
        [JsonPropertyName("managed_key_id")]
        public string ManagedKeyId { get; set; }

        /// <summary>
        /// The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_id is required. 
        /// Ignored for other types.
        /// </summary>
        [JsonPropertyName("managed_key_name")]
        public string ManagedKeyName { get; set; }

        /// <summary>
        /// The maximum allowable path length
        /// </summary>
        [JsonPropertyName("max_path_length")]
        public int MaxPathLength { get; set; }

        /// <summary>
        /// Set the not after field of the certificate with specified date value. 
        /// The value format should be given in UTC format YYYY-MM-ddTHH:MM:SSZ
        /// </summary>
        [JsonPropertyName("not_after")]
        public string NotAfter { get; set; }

        /// <summary>
        /// The duration before now which the certificate needs to be backdated by.
        /// </summary>
        [JsonPropertyName("not_before_duration")]
        public string NotBeforeDuration { get; set; }

        /// <summary>
        /// If set, O (Organization) will be set to this value.
        /// </summary>
        [JsonPropertyName("organization")]
        public List<string> Organization { get; set; }

        /// <summary>
        /// Requested other SANs, in an array with the format ;UTF8: for each entry.
        /// </summary>
        [JsonPropertyName("other_sans")]
        public List<string> OtherSans { get; set; }

        /// <summary>
        /// If set, OU (OrganizationalUnit) will be set to this value.
        /// </summary>
        [JsonPropertyName("ou")]
        public List<string> OrganizationalUnit { get; set; }

        /// <summary>
        /// Domains for which this certificate is allowed to sign or issue child certificates. 
        /// If set, all DNS names (subject and alt) on child certs must be exact matches or subsets of the given domains 
        /// (see https://tools.ietf.org/html/rfc5280#section-4.2.1.10).
        /// </summary>
        [JsonPropertyName("permitted_dns_domains")]
        public List<string> PermittedDnsDomains { get; set; }

        /// <summary>
        /// If set, Postal Code will be set to this value.
        /// </summary>
        [JsonPropertyName("postal_code")]
        public List<string> PostalCode { get; set; }

        /// <summary>
        /// Format for the returned private key. 
        /// Generally the default will be controlled by the "format" parameter as either base64-encoded DER or PEM-encoded DER. 
        /// However, this can be set to "pkcs8" to have the returned private key contain base64-encoded pkcs8 or PEM-encoded pkcs8 instead. 
        /// Defaults to "der".
        /// </summary>
        [JsonPropertyName("private_key_format")]
        public string PrivateKeyFormat { get; set; }

        /// <summary>
        /// If set, Province will be set to this value.
        /// </summary>
        [JsonPropertyName("province")]
        public List<string> Province { get; set; }

        /// <summary>
        /// The Subject's requested serial number, if any. See RFC 4519 Section 2.31 'serialNumber' for a description of this field. 
        /// If you want more than one, specify alternative names in the alt_names map using OID 2.5.4.5. 
        /// This has no impact on the final certificate's Serial Number field.
        /// </summary>
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// The number of bits to use in the signature algorithm; accepts 256 for SHA-2-256, 384 for SHA-2-384, and 512 for SHA-2-512. 
        /// Defaults to 0 to automatically detect based on key length (SHA-2-256 for RSA keys, and matching the curve size for NIST P-Curves).
        /// </summary>
        [JsonPropertyName("signature_bits")]
        public int SignatureBits { get; set; }

        /// <summary>
        /// If set, Street Address will be set to this value.
        /// </summary>
        [JsonPropertyName("street_address")]
        public List<string> StreetAddress { get; set; }

        /// <summary>
        /// The requested Time To Live for the certificate; sets the expiration date. 
        /// If not specified the role default, backend default, or system default TTL is used, in that order. Cannot be larger than the mount max TTL. 
        /// Note: this only has an effect when generating a CA cert or signing a CA cert, not when generating a CSR for an intermediate CA.
        /// </summary>
        [JsonPropertyName("ttl")]
        public string TTL { get; set; }

        /// <summary>
        /// The requested URI SANs, if any, in a comma-delimited list.
        /// </summary>
        [JsonPropertyName("uri_sans")]
        public List<string> UriSans { get; set; }

        /// <summary>
        /// Whether or not to use PSS signatures when using a RSA key-type issuer. Defaults to false.
        /// </summary>
        [JsonPropertyName("use_pss")]
        public bool UsePss { get; set; }

        public GenerateRootRequest()
        {
            //set defaults
            Format = "pem";
            KeyType = "rsa";
            IssuerName = string.Empty;
            KeyName = string.Empty;
        }
    }
}
