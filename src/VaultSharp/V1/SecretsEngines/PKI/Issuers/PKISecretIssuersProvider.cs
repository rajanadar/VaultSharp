using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    internal class PKISecretIssuersProvider : IPKIIssuers
    {
        private readonly Polymath _polymath;

        public PKISecretIssuersProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<KeysData>> ListIssuers(string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<KeysData>>(pkiBackendMountPoint ?? 
                _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, 
                "/issuers/?list=true", HttpMethod.Get, 
                null, 
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<IssuerResponseData>> AddIssuer(IssuerData createIssuerOptions, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(createIssuerOptions, "createIssuerOptions");

            var result = await _polymath.MakeVaultApiRequest<Secret<IssuerResponseData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/root/generate/internal", HttpMethod.Post, createIssuerOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<IssuerConfigResponseData>> GetIssuerConfigData(string issuerName,
            string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(issuerName, "issuerName");
            var result = await _polymath.MakeVaultApiRequest<Secret<IssuerConfigResponseData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/issuer/" + issuerName, HttpMethod.Get, null, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<IssuerConfigResponseData>> ConfigureIssuer(IssuerConfigRequestData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(data, "data");

            var result = await _polymath.MakeVaultApiRequest<Secret<IssuerConfigResponseData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/issuer/" + data.IssuerName, HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task DeleteIssuer(string issuerName, string pkiBackendMountPoint = null,
            string wrapTimeToLive = null)
        {
            Checker.NotNull(issuerName, "issuerName");
            await _polymath.MakeVaultApiRequest<Secret<IssuerData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/issuer/" + issuerName, HttpMethod.Delete, null, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}