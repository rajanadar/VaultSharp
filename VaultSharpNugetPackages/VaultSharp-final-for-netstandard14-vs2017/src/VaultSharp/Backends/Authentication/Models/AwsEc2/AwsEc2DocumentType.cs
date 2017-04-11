using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the aws ec2 document type
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AwsEc2DocumentType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The PKCS7
        /// </summary>
        pkcs7 = 1,

        /// <summary>
        /// The instance
        /// </summary>
        instance = 2,
    }
}