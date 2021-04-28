using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Azure
{
    internal class AzureSecretsEngineProvider : IAzureSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.Azure), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.Azure;
            }
        }

        public AzureSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<AzureCredentials>> GetCredentialsAsync(string azureRoleName, string azureBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(azureRoleName, "azureRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AzureCredentials>>(azureBackendMountPoint ?? MountPoint, "/creds/" + azureRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}