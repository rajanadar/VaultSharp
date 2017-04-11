namespace VaultSharp.Backends.Secret.Models.SSH
{
    /// <summary>
    /// Represents the SSH OTP Key type Credentials
    /// </summary>
    public class SSHOTPCredentials : SSHCredentials
    {
        /// <summary>
        /// Type of credentials
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        public override SSHKeyType KeyType
        {
            get { return SSHKeyType.otp; }
        }
    }
}