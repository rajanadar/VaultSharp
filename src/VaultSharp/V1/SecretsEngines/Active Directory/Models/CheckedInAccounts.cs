using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CheckedInAccounts
    {
        [JsonProperty("check_ins")]
        public List<string> AccountNames { get; set; }
    }
}