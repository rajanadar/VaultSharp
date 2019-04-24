using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// The PKI Secrets Engine.
    /// </summary>
    public interface IPKISecretsEngine
    {
        /// <summary>
        /// Generates a new set of credentials (private key and certificate) based on the role named in the endpoint.
        /// The issuing CA certificate is returned as well, so that only the root CA need be in a client's trust store.
        /// The private key is not stored.
        /// If you do not save the private key, you will need to request a new certificate.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.
        /// </param>
        /// <param name="certificateCredentialRequestOptions"><para>[required]</para>
        /// The certificate credential request options.
        /// </param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineDefaultPaths.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the new Certificate credentials.
        /// </returns>
        Task<Secret<CertificateCredentials>> GetCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = SecretsEngineDefaultPaths.PKI, string wrapTimeToLive = null);

        /// <summary>
        /// Revokes a certificate using its serial number.
        /// </summary>
        /// <param name="serialNumber"><para>[required]</para>
        /// The serial number.
        /// </param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineDefaultPaths.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the Certificate revokation info.
        /// </returns>
        Task<Secret<CertificateCredentialRevoke>> RevokeCredentialAsync(string serialNumber, string pkiBackendMountPoint = SecretsEngineDefaultPaths.PKI);
    }
}