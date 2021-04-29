using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    /// <summary>
    /// Consul Secrets Engine.
    /// </summary>
    public interface IConsulSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic Consul token based on the role definition.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretsEngineMountPoints.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="ConsulCredentials" /> as the data.
        /// </returns>
        Task<Secret<ConsulCredentials>> GetCredentialsAsync(string consulRoleName, string consulBackendMountPoint = null, string wrapTimeToLive = null);
    }
}