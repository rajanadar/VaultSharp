using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.Token
{
    public class ListTokenRoles
    {
        /// <summary>
        /// List of available token roles.
        /// </summary>
        [JsonProperty("keys")]
        public List<string> Keys { get; set; }
    }
}
