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

        public async Task<Secret<AWSCredentials>> GetCredentialsAsync(string awsRoleName, string awsMountPoint = SecretsEngineDefaultPaths.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsMountPoint, "awsMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>("v1/" + awsMountPoint.Trim('/') + "/creds/" + awsRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> GenerateSTSCredentialsAsync(string awsRoleName, string timeToLive = "1h", string awsMountPoint = SecretsEngineDefaultPaths.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsMountPoint, "awsMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            object requestData = string.IsNullOrWhiteSpace(timeToLive) ? null : new { ttl = timeToLive };

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>("v1/" + awsMountPoint.Trim('/') + "/sts/" + awsRoleName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string awsMountPoint = SecretsEngineDefaultPaths.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsMountPoint, "awsMountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + awsMountPoint.Trim('/') + "/roles?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSRoleModel>> ReadRoleAsync(string awsRoleName, string awsMountPoint = SecretsEngineDefaultPaths.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsMountPoint, "awsMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AWSRoleModel>>("v1/" + awsMountPoint.Trim('/') + "/roles/" + awsRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}