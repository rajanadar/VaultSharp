using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.Token
{
    public class ListTokenRoles
    {
        /// <summary>
        /// List of available token roles.
        /// </summary>
        [JsonPropertyName("keys")]
        public List<string> Keys { get; set; }
    }
}
