using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KMIP
{
    /// <summary>
    /// KMIP Secrets Engine.
    /// </summary>
    public interface IKMIPSecretsEngine
    {
        /// <summary>
        /// Generates a new client certificate tied to the given role and scope.
        /// </summary>
        /// <param name="scopeName"><para>[required]</para>
        /// Name of the KMIP scope.</param>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the KMIP role.</param>
        /// <param name="format"><para>[required]</para>
        /// Format to return the certificate, private key, and CA chain in.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KMIP backend. Defaults to <see cref="SecretsEngineMountPoints.KMIP" />
        /// Provide a value only if you have customized the KMIP mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="KMIPCredentials" /> as the data.
        /// </returns>
        Task<Secret<KMIPCredentials>> GetCredentialsAsync(string scopeName, string roleName, CertificateFormat format = CertificateFormat.pem, string mountPoint = null, string wrapTimeToLive = null);
    }
}