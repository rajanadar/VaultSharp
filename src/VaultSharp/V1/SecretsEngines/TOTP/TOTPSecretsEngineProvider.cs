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

        public async Task<Secret<TOTPCode>> GetCodeAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPCode>>("v1/" + mountPoint.Trim('/') + "/code/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPCodeValidity>> ValidateCodeAsync(string keyName, string code, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(code, "code");

            var requestData = new { code = code };
            return await _polymath.MakeVaultApiRequest<Secret<TOTPCodeValidity>>("v1/" + mountPoint.Trim('/') + "/code/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPCreateKeyResponse>> CreateKeyAsync(string keyName, TOTPCreateKeyRequest createKeyRequest, string mountPoint)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(createKeyRequest, "createKeyRequest");
            Checker.NotNull(createKeyRequest.KeyGenerationOption, "createKeyRequest.KeyGenerationOption");

            object request;

            if (createKeyRequest.KeyGenerationOption is TOTPVaultBasedKeyGeneration vaultBasedOption)
            {
                request = new
                {
                    generate = true,
                    exported = vaultBasedOption.Exported,
                    key_size = vaultBasedOption.KeySize,

                    issuer = createKeyRequest.Issuer,
                    account_name = createKeyRequest.AccountName,
                    period = createKeyRequest.Period,
                    algorithm = createKeyRequest.Algorithm,
                    digits = createKeyRequest.Digits,

                    skew = vaultBasedOption.Skew,
                    qr_size = vaultBasedOption.QRSize
                };
            }
            else
            {
                var nonVaultOption = createKeyRequest.KeyGenerationOption as TOTPNonVaultBasedKeyGeneration;

                request = new
                {
                    url = nonVaultOption.Url,
                    key = nonVaultOption.Key,

                    issuer = createKeyRequest.Issuer,
                    account_name = createKeyRequest.AccountName,
                    period = createKeyRequest.Period,
                    algorithm = createKeyRequest.Algorithm,
                    digits = createKeyRequest.Digits
                };
            }

            return await _polymath.MakeVaultApiRequest<Secret<TOTPCreateKeyResponse>>("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Post, request).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPKey>> ReadKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPKey>>("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllKeysAsync(string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/keys", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteKeyAsync(string keyName, string mountPoint = "totp")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}