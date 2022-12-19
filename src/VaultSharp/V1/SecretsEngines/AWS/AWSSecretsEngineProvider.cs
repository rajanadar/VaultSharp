using System.Collections.Generic;
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

        public async Task ConfigureRootIAMCredentialsAsync(ConfigureRootIAMCredentialsModel configureRootIAMCredentialModel, string awsMountPoint = null)
        {
            await _polymath.MakeVaultApiRequest(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/config/root", HttpMethod.Post,
               requestData: configureRootIAMCredentialModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RootIAMCredentialsConfigModel>> GetRootIAMCredentialsConfigAsync(string awsMountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<RootIAMCredentialsConfigModel>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/config/root", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RotateRootIAMCredentialsResponseModel>> RotateRootIAMCredentialsAsync(string awsMountPoint = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<RotateRootIAMCredentialsResponseModel>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/config/rotate-root", HttpMethod.Post).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureLeaseAsync(AWSLeaseConfigModel leaseConfigModel, string awsMountPoint = null)
        {
            await _polymath.MakeVaultApiRequest(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/config/lease", HttpMethod.Post,
               requestData: leaseConfigModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSLeaseConfigModel>> GetLeaseConfigAsync(string awsMountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<AWSLeaseConfigModel>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string awsRoleName, CreateAWSRoleModel awsRoleModel, string awsMountPoint = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            await _polymath.MakeVaultApiRequest(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/roles/" + awsRoleName.Trim('/'), HttpMethod.Post, requestData: awsRoleModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSRoleModel>> ReadRoleAsync(string awsRoleName, string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AWSRoleModel>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/roles/" + awsRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string awsMountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/roles?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> GetCredentialsAsync(string awsRoleName, string roleARN = null, string roleSessionName = null, string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            var queryStrings = new List<string>();

            if (!string.IsNullOrWhiteSpace(roleARN))
            {
                queryStrings.Add("role_arn=" + roleARN);
            }

            if (!string.IsNullOrWhiteSpace(roleSessionName))
            {
                queryStrings.Add("role_session_name=" + roleSessionName);
            }

            var queryString = "";

            if (queryStrings.Count > 0)
            {
                queryString = "?" + string.Join("&", queryStrings);
            }

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/creds/" + awsRoleName.Trim('/') + queryString, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> GenerateSTSCredentialsAsync(string awsRoleName, string roleARN = null, string roleSessionName = null, string timeToLive = "1h", string awsMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsRoleName, "awsRoleName");

            var requestData = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(roleARN))
            {
                requestData.Add("role_arn", roleARN);
            }

            if (!string.IsNullOrWhiteSpace(roleSessionName))
            {
                requestData.Add("role_session_name", roleSessionName);
            }

            if (!string.IsNullOrWhiteSpace(timeToLive))
            {
                requestData.Add("ttl", timeToLive);
            }

            return await _polymath.MakeVaultApiRequest<Secret<AWSCredentials>>(awsMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AWS, "/sts/" + awsRoleName.Trim('/'), HttpMethod.Post, requestData.Count == 0 ? null : requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
