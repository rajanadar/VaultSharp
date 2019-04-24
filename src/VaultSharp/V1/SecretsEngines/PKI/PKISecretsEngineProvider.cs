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

        public async Task<Secret<CertificateCredentials>> GetCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = SecretsEngineDefaultPaths.PKI, string wrapTimeToLive = null)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(certificateCredentialRequestOptions, "certificateCredentialRequestOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<CertificateCredentials>>("v1/" + pkiBackendMountPoint.Trim('/') + "/issue/" + pkiRoleName, HttpMethod.Post, certificateCredentialRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = certificateCredentialRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<CertificateCredentialRevoke>> RevokeCredentialAsync(string serialNumber, string pkiBackendMountPoint = "pki")
        {
            Checker.NotNull(serialNumber, "serialNumber");
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<CertificateCredentialRevoke>>("v1/" + pkiBackendMountPoint.Trim('/') + "/revoke", HttpMethod.Post, new { serial_number = serialNumber }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}