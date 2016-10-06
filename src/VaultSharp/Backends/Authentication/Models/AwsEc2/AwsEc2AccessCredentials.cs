using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the AWS EC2 Client access credentials.
    /// </summary>
    public class AwsEc2AccessCredentials
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the AWS Access key with permissions to query EC2 DescribeInstances API.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the AWS Secret key with permissions to query EC2 DescribeInstances API.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the URL to override the default generated endpoint for making AWS EC2 API calls.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }
    }
}