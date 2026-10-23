using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.PKI.Issuers;
using VaultSharp.V1.SecretsEngines.PKI.Keys;
using VaultSharp.V1.SecretsEngines.PKI.Roles;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    internal class PKISecretsEngineProvider : IPKISecretsEngine
    {
        private readonly Polymath _polymath;

        public PKISecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
            this.Issuers = new PKISecretIssuersProvider(polymath);
            this.Keys = new PKISecretKeysProvider(polymath);
            this.Roles = new PKISecretRolesProvider(polymath);
        }

        public IPKIIssuers Issuers { get; }

        public IPKIKeys Keys { get; }

        public IPKIRoles Roles { get; }

        public async Task<Secret<CertificateCredentials>> GetCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(certificateCredentialRequestOptions, "certificateCredentialRequestOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateCredentials>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/issue/" + pkiRoleName, HttpMethod.Post, certificateCredentialRequestOptions, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = certificateCredentialRequestOptions.CertificateFormat;

            return result;
        }
        
        public async Task<Secret<SignedCertificateData>> SignCertificateAsync(string pkiRoleName, SignCertificatesRequestOptions signCertificatesRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(signCertificatesRequestOptions, "signCertificatesRequestOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<SignedCertificateData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/sign/" + pkiRoleName, HttpMethod.Post, signCertificatesRequestOptions, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = signCertificatesRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<RevokeCertificateResponse>> RevokeCertificateAsync(string serialNumber, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(serialNumber, "serialNumber");

            return await _polymath.MakeVaultApiRequest<Secret<RevokeCertificateResponse>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/revoke", HttpMethod.Post, new SerialNumberRequest { SerialNumber = serialNumber }, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task TidyAsync(CertificateTidyRequest certificateTidyRequest = null, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            var newRequest = certificateTidyRequest ?? new CertificateTidyRequest();

            await _polymath.MakeVaultApiRequest(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/tidy", HttpMethod.Post, newRequest, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task AutoTidyAsync(CertificateAutoTidyRequest certificateAutoTidyRequest = null, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            var newRequest = certificateAutoTidyRequest ?? new CertificateAutoTidyRequest();

            await _polymath.MakeVaultApiRequest(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/config/auto-tidy", HttpMethod.Post, newRequest, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CertificateTidyStatus>> GetTidyStatusAsync(string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CertificateTidyStatus>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/tidy-status", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CertificateTidyStatus>> CancelTidyAsync(string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CertificateTidyStatus>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/tidy-cancel", HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RawCertificateData> ReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            var format = certificateFormat == CertificateFormat.pem ? "/" + CertificateFormat.pem : string.Empty;
            var outputFormat = certificateFormat == CertificateFormat.pem
                ? CertificateFormat.pem
                : CertificateFormat.der;

            var result = await _polymath.MakeVaultApiRequest<string>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/ca" + format, HttpMethod.Get, rawResponse: true, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            
            return new RawCertificateData
            {
                CertificateContent = result,
                EncodedCertificateFormat = outputFormat
            };
        }

        public async Task<Secret<RawCertificateData>> ReadCertificateAsync(string serialNumber, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(serialNumber, "serialNumber");

            var certificateDataSecret = await _polymath.MakeVaultApiRequest<Secret<RawCertificateData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/cert/" + serialNumber , HttpMethod.Get, unauthenticated: true, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return certificateDataSecret;
        }

        public async Task<Secret<CertificateKeys>> ListCertificatesAsync(string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateKeys>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/certs", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return result;
        }

        public async Task<Secret<CertificateKeys>> ListRevokedCertificatesAsync(string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateKeys>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/certs/revoked", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return result;
        }
        
        public async Task<Secret<CertificateData>> ReadDefaultIssuerCertificateChainAsync(CertificateFormat certificateFormat, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default)
        {
            if (certificateFormat != CertificateFormat.json
                && certificateFormat != CertificateFormat.pem)
            {
                throw new ArgumentException("Certificate format should be json or pem.");
            }

            // json
            string path = "/cert/ca_chain";

            if (certificateFormat == CertificateFormat.pem)
            {
                path = "/ca_chain";
            }

            var certificateDataSecret = await _polymath.MakeVaultApiRequest<Secret<CertificateData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, path, HttpMethod.Get, unauthenticated: true, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return certificateDataSecret;
        }
    }
}