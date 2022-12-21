using Newtonsoft.Json;
using System.Collections.Generic;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class AppRolePoliciesModel
    {
        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        [JsonProperty("token_policies")]
        public List<string> TokenPolicies { get; set; }
    }
}