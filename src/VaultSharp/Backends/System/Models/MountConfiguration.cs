using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the configuration values for a mounted backend.
    /// </summary>
    public class MountConfiguration
    {
        /// <summary>
        /// Gets or sets the default lease TTL.
        /// A value of "0" or "system" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The default lease TTL.
        /// </value>
        [JsonProperty("default_lease_ttl")]
        public string DefaultLeaseTtl { get; set; }

        /// <summary>
        /// Gets or sets the maximum lease TTL.
        /// A value of "0" or "system" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The maximum lease TTL.
        /// </value>
        [JsonProperty("max_lease_ttl")]
        public string MaximumLeaseTtl { get; set; }

        /// <summary>
        /// Gets the default lease TTL value.
        /// </summary>
        /// <value>
        /// The default lease TTL value.
        /// </value>
        public int? DefaultLeaseTtlValue
        {
            get
            {
                int result;

                if (int.TryParse(DefaultLeaseTtl, out result))
                {
                    return result;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the maximum lease TTL value.
        /// </summary>
        /// <value>
        /// The maximum lease TTL value.
        /// </value>
        public int? MaximumLeaseTtlValue
        {
            get
            {
                int result;

                if (int.TryParse(MaximumLeaseTtl, out result))
                {
                    return result;
                }

                return null;
            }
        }
    }
}