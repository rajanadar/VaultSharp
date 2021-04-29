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

        public async Task<Secret<TOTPCode>> GetCodeAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPCode>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/code/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPCodeValidity>> ValidateCodeAsync(string keyName, string code, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(code, "code");

            var requestData = new { code = code };
            return await _polymath.MakeVaultApiRequest<Secret<TOTPCodeValidity>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/code/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
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

            return await _polymath.MakeVaultApiRequest<Secret<TOTPCreateKeyResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/keys/" + keyName.Trim('/'), HttpMethod.Post, request).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TOTPKey>> ReadKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<TOTPKey>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/keys/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllKeysAsync(string mountPoint = null, string wrapTimeToLive = null)
        {

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/keys?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.TOTP, "/keys/" + keyName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}