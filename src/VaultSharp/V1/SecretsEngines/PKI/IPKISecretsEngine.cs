using System.Collections.Generic;
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
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the new Certificate credentials.
        /// </returns>
        Task<Secret<CertificateCredentials>> GetCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint signs a new certificate based upon the provided CSR and the supplied parameters,
        /// subject to the restrictions contained in the role named in the endpoint.
        /// The issuing CA certificate is returned as well, so that only the root CA need be in a client's trust store.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.
        /// </param>
        /// <param name="signCertificateRequestOptions"><para>[required]</para>
        /// The sign certificate request options.
        /// </param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the signed Certificate.
        /// </returns>
        Task<Secret<SignedCertificateData>> SignCertificateAsync(string pkiRoleName, SignCertificatesRequestOptions signCertificateRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint revokes a certificate using its serial number. 
        /// This is an alternative option to the standard method of revoking using Vault lease IDs.
        /// A successful revocation will rotate the CRL.
        /// </summary>
        /// <param name="serialNumber"><para>[required]</para>
        /// Specifies the serial number of the certificate to revoke, in hyphen-separated or colon-separated octal.
        /// </param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the Certificate revokation info.
        /// </returns>
        Task<Secret<RevokeCertificateResponse>> RevokeCertificateAsync(string serialNumber, string pkiBackendMountPoint = null);

        /// <summary>
        /// This endpoint allows tidying up the storage backend and/or CRL by removing certificates that have expired 
        /// and are past a certain buffer period beyond their expiration time.
        /// </summary>
        /// <param name="certificateTidyRequest">The request object</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The task
        /// </returns>
        Task TidyAsync(CertificateTidyRequest certificateTidyRequest = null, string pkiBackendMountPoint = null);

        /// <summary>
        /// This endpoint allows auto tidying up the storage backend and/or CRL by removing certificates that have expired 
        /// and are past a certain buffer period beyond their expiration time.
        /// </summary>
        /// <param name="certificateAutoTidyRequest">The request object</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The task
        /// </returns>
        Task AutoTidyAsync(CertificateAutoTidyRequest certificateAutoTidyRequest = null, string pkiBackendMountPoint = null);

        /// <summary>
        /// This is a read only endpoint that returns information about the current tidy operation, 
        /// or the most recent if none are currently running.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>The tidy status</returns>
        Task<Secret<CertificateTidyStatus>> GetTidyStatusAsync(string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves the CA certificate in raw DER-encoded form. 
        /// This is a bare endpoint that does not return a standard Vault data structure. 
        /// The CA certificate can be returned in DER or PEM format.
        /// This is an unauthenticated endpoint.
        /// </summary>
        /// <param name="certificateFormat"><para>[optional]</para>
        /// The certificate format needed.
        /// Defaults to <see cref="CertificateFormat.der" /></param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The raw certificate data.
        /// </returns>
        Task<RawCertificateData> ReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves a certificate by key (serial number). The certificate format is always <see cref="CertificateFormat.pem"/>.
        /// This is an unauthenticated endpoint.
        /// </summary>
        /// <param name="serialNumber">
        /// The serial number of the certificate to be retrieved (Example: '17:67:16:b0:b9:45:58:c0:3a:29:e3:cb:d6:98:33:7a:a6:3b:66:c1').
        /// To retrieve the CA certificate, use the value 'ca'.
        /// To retrieve the CA Chain, use the value 'ca_chain'.
        /// To retrieve the current CRL, use the value 'crl'.
        /// </param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the certificate data
        /// </returns>
        Task<Secret<CertificateData>> ReadCertificateAsync(string serialNumber, string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves a list of all certificate keys (serial numbers).
        /// </summary>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the list of certificate keys (serial numbers)
        /// </returns>
        Task<Secret<CertificateKeys>> ListCertificatesAsync(string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves a list of all revoked certificate keys (serial numbers).
        /// </summary>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the list of revoked certificate keys (serial numbers)
        /// </returns>
        Task<Secret<CertificateKeys>> ListRevokedCertificatesAsync(string pkiBackendMountPoint = null);
    }
}