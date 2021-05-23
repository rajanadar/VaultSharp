using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    internal class GoogleCloudSecretsEngineProvider : IGoogleCloudSecretsEngine
    {
        private readonly Polymath _polymath;

        public GoogleCloudSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<GoogleCloudOAuth2Token>> GetOAuth2TokenAsync(string roleset, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleset, "roleset");

            return await _polymath.MakeVaultApiRequest<Secret<GoogleCloudOAuth2Token>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloud, "/token/" + roleset.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<GoogleCloudServiceAccountKey>> GetServiceAccountKeyAsync(string roleset, ServiceAccountKeyAlgorithm keyAlgorithm = ServiceAccountKeyAlgorithm.KEY_ALG_RSA_2048, ServiceAccountPrivateKeyType privateKeyType = ServiceAccountPrivateKeyType.TYPE_GOOGLE_CREDENTIALS_FILE, string timeToLive = "", string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleset, "roleset");

            var requestData = new
            {
                key_algorithm = keyAlgorithm.ToString(),
                key_type = ServiceAccountPrivateKeyType.TYPE_GOOGLE_CREDENTIALS_FILE.ToString(),
                ttl = timeToLive
            };

            return await _polymath.MakeVaultApiRequest<Secret<GoogleCloudServiceAccountKey>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloud, "/key/" + roleset.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}