using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.SSH
{
    /// <summary>
    /// Represents the SSH OTP verification data.
    /// </summary>
    public class SSHOTPVerificationData
    {
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        [JsonProperty("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}