using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    internal class AliCloudSecretsEngineProvider : IAliCloudSecretsEngine
    {
        private readonly Polymath _polymath;

        public AliCloudSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<AliCloudCredentials>> GetCredentialsAsync(string aliCloudRoleName, string aliCloudMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(aliCloudRoleName, "aliCloudRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AliCloudCredentials>>(aliCloudMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/creds/" + aliCloudRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}