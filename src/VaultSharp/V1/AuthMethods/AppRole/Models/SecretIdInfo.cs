using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    /// <summary>
    /// https://developer.hashicorp.com/vault/api-docs/auth/approle#generate-new-secret-id
    /// </summary>
    public class SecretIdInfo
    {
        [JsonPropertyName("secret_id")]
        public string SecretId { get; set; }

        [JsonPropertyName("secret_id_accessor")]
        public string SecretIdAccessor { get; set; }
        
        [JsonPropertyName("secret_id_num_uses")]
        public long SecretIdNumberOfUses { get; set; }

        [JsonPropertyName("secret_id_ttl")]
        public long SecretIdTimeToLive { get; set; }
    }
}
