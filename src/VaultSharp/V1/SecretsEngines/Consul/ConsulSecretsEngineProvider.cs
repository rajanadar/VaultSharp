using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    internal class ConsulSecretsEngineProvider : IConsulSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.Consul), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.Consul;
            }
        }

        public ConsulSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<ConsulCredentials>> GetCredentialsAsync(string consulRoleName, string consulBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulRoleName, "consulRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<ConsulCredentials>>(consulBackendMountPoint ?? MountPoint, "/creds/" + consulRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}