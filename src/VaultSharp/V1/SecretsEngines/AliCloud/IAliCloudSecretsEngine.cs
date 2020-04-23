using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    /// <summary>
    /// The AliCloud Secrets Engine.
    /// </summary>
    public interface IAliCloudSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic AliCloud RAM credential based on the named role.
        /// </summary>
        /// <param name="aliCloudRoleName"><para>[required]</para>
        /// Name of the AliCloud role.</param>
        /// <param name="aliCloudMountPoint"><para>[optional]</para>
        /// The mount point for the AliCloud backend. Defaults to <see cref="SecretsEngineDefaultPaths.AliCloud" />
        /// Provide a value only if you have customized the AliCloud mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AliCloudCredentials" /> as the data.
        /// </returns>
        Task<Secret<AliCloudCredentials>> GetCredentialsAsync(string aliCloudRoleName, string aliCloudMountPoint = SecretsEngineDefaultPaths.AliCloud, string wrapTimeToLive = null);
    }
}