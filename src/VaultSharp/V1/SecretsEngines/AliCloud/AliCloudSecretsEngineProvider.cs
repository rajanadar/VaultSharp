using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    internal class AliCloudSecretsEngineProvider : IAliCloudSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.AliCloud), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.AliCloud;
            }
        }

        public AliCloudSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<AliCloudCredentials>> GetCredentialsAsync(string aliCloudRoleName, string aliCloudMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(aliCloudRoleName, "aliCloudRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AliCloudCredentials>>(aliCloudMountPoint ?? MountPoint, "/creds/" + aliCloudRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}