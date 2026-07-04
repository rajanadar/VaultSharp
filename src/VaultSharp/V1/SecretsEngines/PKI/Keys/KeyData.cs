using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Keys
{
    public class KeyData
    {
        public KeyData()
        {
            this.KeyType = PrivateKeyType.rsa;

        }

        /// <summary>
        /// The type of key generation. 
        /// </summary>
        [JsonIgnore]
        public KeyGenerationType KeyGenerationType { get; set; }

        /// <summary>
        /// The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot; and \&quot;ed25519\&quot; are the only valid values.
        /// </summary>
        /// <value>The type of key to use; defaults to RSA. \&quot;rsa\&quot; \&quot;ec\&quot; and \&quot;ed25519\&quot; are the only valid values.</value>
        [JsonPropertyName("key_type")]
        public PrivateKeyType KeyType { get; set; }

        /// <summary>
        /// The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or 4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.
        /// </summary>
        /// <value>The number of bits to use. Allowed values are 0 (universal default); with rsa key_type: 2048 (default), 3072, or 4096; with ec key_type: 224, 256 (default), 384, or 521; ignored with ed25519.</value>
        [JsonPropertyName("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// Optional name to be used for this key
        /// </summary>
        /// <value>Optional name to be used for this key</value>
        [JsonPropertyName("key_name")]
        public string KeyName { get; set; }

        /// <summary>
        /// The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_name is required. Ignored for other types.
        /// </summary>
        /// <value>The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_name is required. Ignored for other types.</value>
        [JsonPropertyName("managed_key_id")]
        public string ManagedKeyId { get; set; }

        /// <summary>
        /// The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_id is required. Ignored for other types.
        /// </summary>
        /// <value>The name of the managed key to use when the exported type is kms. When kms type is the key type, this field or managed_key_id is required. Ignored for other types.</value>
        [JsonPropertyName("managed_key_name")]
        public string ManagedKeyName { get; set; }

        [JsonPropertyName("private_key")]
        public string PrivateKey { get; set; }
    }
}