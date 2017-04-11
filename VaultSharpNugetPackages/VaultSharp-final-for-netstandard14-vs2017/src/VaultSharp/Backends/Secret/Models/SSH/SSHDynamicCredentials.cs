using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.SSH
{
    /// <summary>
    /// Represents the SSH Dynamic Key type Credentials
    /// </summary>
    public class SSHDynamicCredentials : SSHCredentials
    {
        /// <summary>
        /// Type of credentials
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        public override SSHKeyType KeyType
        {
            get { return SSHKeyType.dynamic; }
        }
    }
}