
using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ActiveDirectoryRoleModel
    {
        [JsonPropertyName("last_vault_rotation")]
        public DateTimeOffset? LastVaultRotation { get; set; }

        [JsonPropertyName("password_last_set")]
        public DateTimeOffset? PasswordLastSet { get; set; }

        [JsonPropertyName("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonPropertyName("ttl")]
        public long TimeToLive { get; set; }
    }
}