using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    /// <summary>
    /// The Active Directory Secrets Engine.
    /// </summary>
    public interface IActiveDirectorySecretsEngine
    {
        /// <summary>
        /// Offers the credential information for a given role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the AD backend. Defaults to <see cref="SecretsEngineMountPoints.ActiveDirectory" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="ActiveDirectoryCredentials" /> as the data.
        /// </returns>
        Task<Secret<ActiveDirectoryCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);
    }
}