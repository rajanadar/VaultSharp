using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    /// <summary>
    /// Cubbyhole Secrets Engine.
    /// </summary>
    public interface IOpenLDAPSecretsEngine
    {
         /// <summary>
        /// Generates a new set of STATIC credentials based on the named role.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Specifies the name of the static role to get credentials for.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the OpenLDAP backend. Defaults to <see cref="SecretsEngineMountPoints.OpenLDAP" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="StaticCredentials" /> as the data.
        /// </returns>
        Task<Secret<StaticCredentials>> GetStaticCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);
    }
}