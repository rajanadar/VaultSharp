using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    internal class AWSSecretsEngineProvider : IAWSSecretsEngine
    {
        private readonly Polymath _polymath;

        public AWSSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<AWSCredentials>> GetCredentialsAsync(string awsRoleName, string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/creds/" + awsRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> GenerateSTSCredentialsAsync(string awsRoleName, string timeToLive = "1h", string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            object requestData = string.IsNullOrWhiteSpace(timeToLive) ? null : new { ttl = timeToLive };

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/sts/" + awsRoleName.Trim('/'), HttpMethod.Get, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string awsMountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/roles?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSRoleModel>> ReadRoleAsync(string awsRoleName, string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AWSRoleModel>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/roles/" + awsRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
