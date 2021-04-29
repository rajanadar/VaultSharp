using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.RabbitMQ
{
    /// <summary>
    /// The RabbitMQ Secrets Engine.
    /// </summary>
    public interface IRabbitMQSecretsEngine
    {
        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role to create credentials against.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.RabbitMQ" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);
    }
}