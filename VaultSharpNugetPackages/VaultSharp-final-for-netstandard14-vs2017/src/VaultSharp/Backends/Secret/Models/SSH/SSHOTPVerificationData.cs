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

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }
}