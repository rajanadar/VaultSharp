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

        public async Task<Secret<GoogleCloudOAuth2Token>> GetOAuth2TokenAsync(string roleset, string mountPoint = SecretsEngineDefaultPaths.GoogleCloud, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleset, "roleset");
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<GoogleCloudOAuth2Token>>("v1/" + mountPoint.Trim('/') + "/token/" + roleset.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<GoogleCloudServiceAccountKey>> GetServiceAccountKeyAsync(string roleset, ServiceAccountKeyAlgorithm keyAlgorithm = ServiceAccountKeyAlgorithm.KEY_ALG_RSA_2048, ServiceAccountPrivateKeyType privateKeyType = ServiceAccountPrivateKeyType.TYPE_GOOGLE_CREDENTIALS_FILE, string timeToLive = "", string mountPoint = SecretsEngineDefaultPaths.GoogleCloud, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleset, "roleset");
            Checker.NotNull(mountPoint, "mountPoint");

            var requestData = new
            {
                key_algorithm = keyAlgorithm.ToString(),
                key_type = ServiceAccountPrivateKeyType.TYPE_GOOGLE_CREDENTIALS_FILE.ToString(),
                ttl = timeToLive
            };

            return await _polymath.MakeVaultApiRequest<Secret<GoogleCloudServiceAccountKey>>("v1/" + mountPoint.Trim('/') + "/key/" + roleset.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}