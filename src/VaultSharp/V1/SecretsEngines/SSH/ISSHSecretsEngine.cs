using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    /// <summary>
    /// The SSH Secrets Engine.
    /// </summary>
    public interface ISSHSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic SSH credentials for a specific username and IP Address based on the named role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the SSH Role.</param>
        /// <param name="ipAddress"><para>[required]</para>
        /// The ip address of the remote host.</param>
        /// <param name="username"><para>[optional]</para>
        /// The username on the remote host.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretsEngineDefaultPaths.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the SSH credentials.
        /// </returns>
        Task<Secret<SSHCredentials>> GetCredentialsAsync(string roleName, string ipAddress, string username = null, string mountPoint = SecretsEngineDefaultPaths.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint signs an SSH public key based on the supplied parameters, 
        /// subject to the restrictions contained in the role named in the endpoint.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the role to sign.</param>
        /// <param name="signKeyRequest"><para>[required]</para>
        /// The request parameters</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretsEngineDefaultPaths.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>The signed key</returns>
        Task<Secret<SignedKeyResponse>> SignKeyAsync(string roleName, SignKeyRequest signKeyRequest, string mountPoint = SecretsEngineDefaultPaths.SSH);
    }
}