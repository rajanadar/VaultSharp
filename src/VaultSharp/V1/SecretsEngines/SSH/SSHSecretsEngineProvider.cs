using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    internal class SSHSecretsEngineProvider : ISSHSecretsEngine
    {
        private readonly Polymath _polymath;

        public SSHSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<SSHCredentials>> GetCredentialsAsync(string roleName, string ipAddress, string username = null, string mountPoint = "ssh", string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { ip = ipAddress, username = username };

            return await _polymath.MakeVaultApiRequest<Secret<SSHCredentials>>("v1/" + mountPoint.Trim('/') + "/creds/" + roleName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SignedKeyResponse>> SignKeyAsync(string roleName, SignKeyRequest signKeyRequest, string mountPoint = "ssh")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(signKeyRequest, "signKeyRequest");

            return await _polymath.MakeVaultApiRequest<Secret<SignedKeyResponse>>("v1/" + mountPoint.Trim('/') + "/sign/" + roleName.Trim('/'), HttpMethod.Post, signKeyRequest).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}