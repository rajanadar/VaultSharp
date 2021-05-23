using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Azure
{
    /// <summary>
    /// Azure Secrets Engine.
    /// </summary>
    public interface IAzureSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic Azure token based on the role definition.
        /// </summary>
        /// <param name="azureRoleName"><para>[required]</para>
        /// Name of the Azure role.</param>
        /// <param name="azureBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Azure backend. Defaults to <see cref="SecretsEngineMountPoints.Azure" />
        /// Provide a value only if you have customized the Azure mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AzureCredentials" /> as the data.
        /// </returns>
        Task<Secret<AzureCredentials>> GetCredentialsAsync(string azureRoleName, string azureBackendMountPoint = null, string wrapTimeToLive = null);
    }
}