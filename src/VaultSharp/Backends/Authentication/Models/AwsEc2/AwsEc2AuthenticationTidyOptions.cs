using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the AWS EC2 tidying options.
    /// </summary>
    public class AwsEc2AuthenticationTidyOptions
    {
        /// <summary>
        /// <para>[optional]</para> 
        /// Gets or sets the amount of extra time that must have passed beyond the roletag expiration, 
        /// before it is removed from the backend storage. Defaults to 72h.
        /// </summary>
        /// <value>
        /// The aws public key.
        /// </value>
        [JsonProperty("safety_buffer")]
        public string SafetyBuffer { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// If set to <c>true</c>, disables the periodic tidying of the 'identity-whitelist/' entries.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable periodic tidy]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("disable_periodic_tidy")]
        public bool DisablePeriodicTidy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsEc2AuthenticationTidyOptions"/> class.
        /// </summary>
        public AwsEc2AuthenticationTidyOptions()
        {
            SafetyBuffer = "72h";
        }
    }
}