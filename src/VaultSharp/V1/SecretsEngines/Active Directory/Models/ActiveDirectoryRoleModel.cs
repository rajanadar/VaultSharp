
using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class ActiveDirectoryRoleModel
    {
        [JsonProperty("last_vault_rotation")]
        public DateTimeOffset? LastVaultRotation { get; set; }

        [JsonProperty("password_last_set")]
        public DateTimeOffset? PasswordLastSet { get; set; }

        [JsonProperty("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonProperty("ttl")]
        public long TimeToLive { get; set; }
    }
}