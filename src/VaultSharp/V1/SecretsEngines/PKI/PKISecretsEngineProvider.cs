using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    internal class PKISecretsEngineProvider : IPKISecretsEngine
    {
        private readonly Polymath _polymath;

        public PKISecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<CertificateCredentials>> GetCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(certificateCredentialRequestOptions, "certificateCredentialRequestOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateCredentials>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/issue/" + pkiRoleName, HttpMethod.Post, certificateCredentialRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = certificateCredentialRequestOptions.CertificateFormat;

            return result;
        }
        
        public async Task<Secret<SignedCertificateData>> SignCertificateAsync(string pkiRoleName, SignCertificatesRequestOptions signCertificatesRequestOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(signCertificatesRequestOptions, "signCertificatesRequestOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<SignedCertificateData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/sign/" + pkiRoleName, HttpMethod.Post, signCertificatesRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = signCertificatesRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<RevokeCertificateResponse>> RevokeCertificateAsync(string serialNumber, string pkiBackendMountPoint = null)
        {
            Checker.NotNull(serialNumber, "serialNumber");

            return await _polymath.MakeVaultApiRequest<Secret<RevokeCertificateResponse>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/revoke", HttpMethod.Post, new { serial_number = serialNumber }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task TidyAsync(CertificateTidyRequest certificateTidyRequest = null, string pkiBackendMountPoint = null)
        {
            var newRequest = certificateTidyRequest ?? new CertificateTidyRequest();

            await _polymath.MakeVaultApiRequest(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/tidy", HttpMethod.Post, newRequest).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RawCertificateData> ReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = null)
        {
            var format = certificateFormat == CertificateFormat.pem ? "/" + CertificateFormat.pem : string.Empty;
            var outputFormat = certificateFormat == CertificateFormat.pem
                ? CertificateFormat.pem
                : CertificateFormat.der;

            var result = await _polymath.MakeVaultApiRequest<string>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/ca" + format, HttpMethod.Get, rawResponse: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            
            return new RawCertificateData
            {
                CertificateContent = result,
                EncodedCertificateFormat = outputFormat
            };
        }

        public async Task<CertificateData> ReadCertificateAsync(string serialNumber, string pkiBackendMountPoint = null)
        {
            Checker.NotNull(serialNumber, "serialNumber");

            var certificateDataSecret = await _polymath.MakeVaultApiRequest<Secret<CertificateData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/cert/" + serialNumber , HttpMethod.Get, unauthenticated: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            var certificateData = certificateDataSecret.Data;

            if (string.IsNullOrEmpty(certificateData.SerialNumber))
            {
                certificateData.SerialNumber = serialNumber.ToLower().Replace("-", ":"); // To be consistent with serial number format returned in other endpoints by Vault (even though it accepts dashes as input as well)
            }

            certificateData.CertificateFormat = CertificateFormat.pem;

            return certificateData;
        }

        public async Task<List<string>> ListCertificatesAsync(string pkiBackendMountPoint = null)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateKeys>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/certs", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return result.Data.Keys;
        }
    }
}