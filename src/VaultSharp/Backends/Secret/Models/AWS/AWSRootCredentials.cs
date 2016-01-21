using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.AWS
{
    /// <summary>
    /// Represents the AWS Root credentials.
    /// </summary>
    public class AWSRootCredentials
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the access key.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [JsonProperty("region")]
        public string Region { get; set; }
    }
}