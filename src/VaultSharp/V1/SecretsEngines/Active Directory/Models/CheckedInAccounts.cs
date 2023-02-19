using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CheckedInAccounts
    {
        [JsonPropertyName("check_ins")]
        public List<string> AccountNames { get; set; }
    }
}