using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Azure
{
    internal class AzureSecretsEngineProvider : IAzureSecretsEngine
    {
        private readonly Polymath _polymath;

        public AzureSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<AzureCredentials>> GetCredentialsAsync(string azureRoleName, string azureBackendMountPoint = SecretsEngineDefaultPaths.Azure, string wrapTimeToLive = null)
        {
            Checker.NotNull(azureBackendMountPoint, "azureBackendMountPoint");
            Checker.NotNull(azureRoleName, "azureRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AzureCredentials>>("v1/" + azureBackendMountPoint.Trim('/') + "/creds/" + azureRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}