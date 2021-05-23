using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Nomad
{
    /// <summary>
    /// Nomad Secrets Engine.
    /// </summary>
    public interface INomadSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic Nomad token based on the role definition.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of an existing role against which to create this Nomad token.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Nomad backend. Defaults to <see cref="SecretsEngineMountPoints.Nomad" />
        /// Provide a value only if you have customized the Nomad mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="NomadCredentials" /> as the data.
        /// </returns>
        Task<Secret<NomadCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);
    }
}