using System.Collections.Generic;
using System.Text.Json.Serialization;
using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    /// <summary>
    /// Request for signing a key.
    /// </summary>
    public class SignKeyRequest
    {
        /// <summary>
        /// <para>[required]</para>
        /// Specifies the SSH public key that should be signed.
        /// </summary>
        [JsonPropertyName("public_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PublicKey { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies the Requested Time To Live. 
        /// Cannot be greater than the role's max_ttl value. 
        /// If not provided, the role's ttl value will be used. 
        /// Note that the role values default to system values if not explicitly set.
        /// </summary>
        [JsonPropertyName("ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies valid principals, either usernames or hostnames, that the certificate should be signed for.
        /// </summary>
        [JsonPropertyName("valid_principals")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ValidPrincipals { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Specifies the type of certificate to be created; either "user" or "host".
        /// Defaults to "user"
        /// </summary>
        [JsonPropertyName("cert_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CertificateType { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies the key id that the created certificate should have. 
        /// If not specified, the display name of the token will be used.
        /// </summary>
        [JsonPropertyName("key_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string KeyId { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies a map of the critical options that the certificate should be signed for.
        /// Defaults to none.
        /// </summary>
        [JsonPropertyName("critical_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string> CriticalOptions { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies a map of the extensions that the certificate should be signed for. Defaults to none.
        /// </summary>
        [JsonPropertyName("extension")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string> Extension { get; set; }

        public SignKeyRequest()
        {
            CertificateType = "user";
        }
    }
}