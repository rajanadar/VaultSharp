using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class PushSecretIdRequestOptions : PullSecretIdRequestOptions
    {
        /// <summary>
        /// SecretID to be attached to the Role.
        /// </summary>
        [JsonProperty("secret_id")]
        public string SecretId { get; set; }
    }
}