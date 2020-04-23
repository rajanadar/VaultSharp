using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    internal class TOTPSecretsEngineProvider : ITOTPSecretsEngine
    {
        private readonly Polymath _polymath;

        public TOTPSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<TOTPProvider>> CreateTOTPProviderKeyAsync(string keyName, string issuer, string accountName, string mountPoint = SecretsEngineDefaultPaths.TOTP)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(issuer, "issuer");
            Checker.NotNull(accountName, "accountName");

            var values = new Dictionary<string, object>()
            {
                ["generate"] = true,
                ["issuer"] = issuer,
                ["account_name"] = accountName
            };

            return await _polymath.MakeVaultApiRequest<Secret<TOTPProvider>>("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Post, requestData: values).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteKeyAsync(string keyName, string mountPoint = "totp")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest<Secret<TOTPProvider>>("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPCode>> GetCodeAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPCode>>("v1/" + mountPoint.Trim('/') + "/code/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPKey>> ReadKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPKey>>("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ListAllKeysAsync(string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/keys", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPCodeValidity>> ValidateCodeAsync(string keyName, string code, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(code, "code");

            var requestData = new { code = code };
            return await _polymath.MakeVaultApiRequest<Secret<TOTPCodeValidity>>("v1/" + mountPoint.Trim('/') + "/code/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}