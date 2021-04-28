using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    internal class SSHSecretsEngineProvider : ISSHSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.SSH), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.SSH;
            }
        }

        public SSHSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<SSHCredentials>> GetCredentialsAsync(string roleName, string ipAddress, string username = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { ip = ipAddress, username = username };

            return await _polymath.MakeVaultApiRequest<Secret<SSHCredentials>>(mountPoint ?? MountPoint, "/creds/" + roleName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SignedKeyResponse>> SignKeyAsync(string roleName, SignKeyRequest signKeyRequest, string mountPoint = null)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(signKeyRequest, "signKeyRequest");

            return await _polymath.MakeVaultApiRequest<Secret<SignedKeyResponse>>(mountPoint ?? MountPoint, "/sign/" + roleName.Trim('/'), HttpMethod.Post, signKeyRequest).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}