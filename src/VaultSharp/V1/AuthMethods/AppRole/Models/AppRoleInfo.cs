using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class AppRoleInfo
    {
        /// <summary>
        /// The incremental lifetime for generated tokens. 
        /// This current value of this will be referenced at renewal time.
        /// Duration in seconds.
        /// </summary>
        [JsonProperty("token_ttl")]
        public int TokenTimeToLive { get; set; }

        /// <summary>
        /// The maximum lifetime for generated tokens. 
        /// This current value of this will be referenced at renewal time.
        /// Duration in seconds.
        /// </summary>
        [JsonProperty("token_max_ttl")]
        public int TokenMaxTimeToLive { get; set; }

        /// <summary>
        /// Duration in either an integer number of seconds (3600) or an 
        /// integer time unit (60m) after which any SecretID expires.
        /// </summary>
        [JsonProperty("secret_id_ttl")]
        public string SecretIdTimeToLive { get; set; }

        /// <summary>
        /// Number of times any particular SecretID can be used to fetch 
        /// a token from this AppRole, after which the SecretID will expire. 
        /// A value of zero will allow unlimited uses.
        /// </summary>
        [JsonProperty("secret_id_num_uses")]
        public int SecretIdNumUses { get; set; }

        /// <summary>
        /// List of policies to encode onto generated tokens. Depending on the
        /// auth method, this list may be supplemented by user/group/other 
        /// values.
        /// </summary>
        [JsonProperty("token_policies")]
        public List<string> TokenPolicies { get; set; }

        /// <summary>
        /// The period, if any, to set on the token.
        /// </summary>
        [JsonProperty("period")]
        public int Period { get; set; }

        /// <summary>
        /// Require secret_id to be presented when logging in using this 
        /// AppRole.
        /// </summary>
        [JsonProperty("bind_secret_id")]
        public bool BindSecretId { get; set; }

        /// <summary>
        /// Comma-separated string or list of CIDR blocks; if set, specifies 
        /// blocks of IP addresses which can perform the login operation.
        /// </summary>
        [JsonProperty("bound_cidr_list")]
        public List<string> BoundCIDRList { get; set; }
    }
}
