using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies the Requested Time To Live. 
        /// Cannot be greater than the role's max_ttl value. 
        /// If not provided, the role's ttl value will be used. 
        /// Note that the role values default to system values if not explicitly set.
        /// </summary>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies valid principals, either usernames or hostnames, that the certificate should be signed for.
        /// </summary>
        [JsonProperty("valid_principals")]
        public string ValidPrincipals { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Specifies the type of certificate to be created; either "user" or "host".
        /// Defaults to "user"
        /// </summary>
        [JsonProperty("cert_type")]
        public string CertificateType { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies the key id that the created certificate should have. 
        /// If not specified, the display name of the token will be used.
        /// </summary>
        [JsonProperty("key_id")]
        public string KeyId { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies a map of the critical options that the certificate should be signed for.
        /// Defaults to none.
        /// </summary>
        [JsonProperty("critical_options")]
        public Dictionary<string, string> CriticalOptions { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Specifies a map of the extensions that the certificate should be signed for. Defaults to none.
        /// </summary>
        [JsonProperty("extension")]
        public Dictionary<string, string> Extension { get; set; }

        public SignKeyRequest()
        {
            CertificateType = "user";
        }
    }
}