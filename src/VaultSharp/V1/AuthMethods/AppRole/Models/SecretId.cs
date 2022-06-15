using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class SecretId
    {
        /// <summary>
        ///     Accessor for the Secret Id
        /// </summary>
        [JsonProperty("secret_id_accessor")]
        public string Secret_Id_Accessor { get; set; }

        /// <summary>
        ///     Secret Id Token
        /// </summary>
        [JsonProperty("secret_id")]
        public string Secret_Id { get; set; }

        /// <summary>
        ///     Secret Id Time to live
        /// </summary>
        [JsonProperty("secret_id_ttl")]
        public int Secret_Id_Ttl { get; set; }
    }
}