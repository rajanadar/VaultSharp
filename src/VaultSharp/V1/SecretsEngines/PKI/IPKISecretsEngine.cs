﻿using System.Collections.Generic;
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
        /// This endpoint allows cancelling a running tidy operation. 
        /// It takes no parameter and cancels the tidy at the next available checkpoint, 
        /// which may process additional certificates between when the operation was 
        /// marked as cancelled and when the operation stopped.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>The tidy status</returns>
        Task<Secret<CertificateTidyStatus>> CancelTidyAsync(string pkiBackendMountPoint = null);

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
        Task<Secret<RawCertificateData>> ReadCertificateAsync(string serialNumber, string pkiBackendMountPoint = null);

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
        
        /// <summary>
        /// Retrieves the default issuer's CA certificate chain, including the default issuer.
        /// </summary>
        /// <param name="certificateFormat">The certificate format needed.</param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the certificate chain data
        /// </returns>
        Task<Secret<CertificateData>> ReadDefaultIssuerCertificateChainAsync(CertificateFormat certificateFormat, string pkiBackendMountPoint = null);

        /// <summary>
        /// Generates a new self-signed CA certificate and private key. If the path ends with exported, the private key will be returned in the response; 
        /// if it is internal the private key will not be returned and cannot be retrieved later; 
        /// if it is existing, the key specified by key_ref will be reused for this root; 
        /// if it is kms, a managed key will be used.
        /// </summary>
        /// <param name="type">valid values are "exported", "internal", and "existing"</param>
        /// <param name="generateRootRequest">The configuration values to use for the newly generated root certificate</param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the generated root response
        /// if the type was "exported", the private key will be included.
        /// </returns>
        Task<Secret<GenerateRootResponse>> GenerateRootAsync(string type, GenerateRootRequest generateRootRequest, string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves a list of all PKI role names.
        /// </summary>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the list of role names
        /// </returns>
        Task<Secret<PKIRoleNames>> ListRolesAsync(string pkiBackendMountPoint = null);

        /// <summary>
        /// Retrieves details for a role by name.
        /// </summary>
        /// <param name="pkiRoleName">
        /// The name of the role to be retrieved.
        /// </param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the role data
        /// </returns>
        Task<Secret<PKIRole>> ReadRoleAsync(string pkiRoleName, string pkiBackendMountPoint = null);

        /// <summary>
        /// Writes a role definition with the provided pkiRoleName.
        /// if the role already exists, it will overwrite it with all values (including elided parameters)
        /// To update a role with only the provided parameters, use 'PatchRoleAsync'
        /// </summary>
        /// <param name="pkiRoleName">
        /// The name of the role to be retrieved.
        /// </param>
        /// <param name="pkiRoleDetails">
        /// The PKIRole object containing the values.
        /// </param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the role data
        /// </returns>
        Task<Secret<PKIRole>> WriteRoleAsync(string pkiRoleName, PKIRole pkiRoleDetails, string pkiBackendMountPoint = null);

        /// <summary>
        /// Patches an existing role definition with the provided pkiRoleName.
        /// Use this method for updating (not replacing) a role definition with the non-null values provided in the pkiRoleDetails object.
        /// This method requires the role definition to already exist, and will update it with any non-null values set in pkiRoleDetails.
        /// To replace a role definition, use the 'WriteRoleAsync' method.
        /// </summary>
        /// <param name="pkiRoleName">
        /// The name of the role to be retrieved.
        /// </param>
        /// <param name="pkiRoleDetails">
        /// The PKIRole object containing the values.
        /// </param>
        /// <param name="pkiBackendMountPoint">
        /// The mount point for the PKI backend. Defaults to <see cref="SecretsEngineMountPoints.PKI" />
        /// Provide a value only if you have customized the PKI mount point.
        /// </param>
        /// <returns>
        /// The secret with the role data
        /// </returns>
        Task<Secret<PKIRole>> PatchRoleAsync(string pkiRoleName, PKIRole pkiRoleDetails, string pkiBackendMountPoint = null);
    }
}