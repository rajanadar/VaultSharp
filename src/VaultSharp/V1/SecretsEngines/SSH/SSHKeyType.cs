using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.SSH
{
    /// <summary>
    /// Represents the type of SSH key to be generated.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SSHKeyType
    {
        /// <summary>
        /// The one time password.
        /// </summary>
        otp = 1,

        /// <summary>
        /// The dynamic key.
        /// </summary>
        dynamic = 2,

        /// <summary>
        /// The ca key.
        /// </summary>
        ca = 3,
    }
}