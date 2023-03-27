using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class PushSecretIdRequestOptions : PullSecretIdRequestOptions
    {
        /// <summary>
        /// SecretID to be attached to the Role.
        /// </summary>
        [JsonPropertyName("secret_id")]
        public string SecretId { get; set; }
    }
}