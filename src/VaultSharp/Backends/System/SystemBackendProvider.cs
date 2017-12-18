using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Auth;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    internal class SystemBackendProvider : ISystemBackend
    {
        private readonly Polymath _polymath;

        public SystemBackendProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<Dictionary<string, AbstractAuditBackend>>> GetAuditBackendsAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AbstractAuditBackend>>>("v1/sys/audit", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.MountPoint = kv.Key;
            }

            return response;
        }

        public async Task MountAuditBackendAsync(AbstractAuditBackend abstractAuditBackend)
        {
            if (string.IsNullOrWhiteSpace(abstractAuditBackend.MountPoint))
            {
                abstractAuditBackend.MountPoint = abstractAuditBackend.Type.Value;
            }

            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + abstractAuditBackend.MountPoint.Trim('/'), HttpMethod.Put, abstractAuditBackend).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuditBackendAsync(string mountPoint)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + mountPoint.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AuditHash>> AuditHashAsync(string mountPoint, string inputToHash)
        {
            var requestData = new { input = inputToHash };
            return await _polymath.MakeVaultApiRequest<Secret<AuditHash>>("v1/sys/audit-hash/" + mountPoint.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, AuthBackend>>> GetAuthBackendsAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AuthBackend>>>("v1/sys/auth", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.Path = kv.Key;
            }

            return response;
        }

        public async Task MountAuthBackendAsync(AuthBackend authBackend)
        {
            if (string.IsNullOrWhiteSpace(authBackend.Path))
            {
                authBackend.Path = authBackend.Type.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", authBackend.Path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, authBackend).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuthBackendAsync(string mountPoint)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", mountPoint.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<BackendConfig>> GetAuthBackendConfigAsync(string path)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}/tune", path.Trim('/'));
            return await _polymath.MakeVaultApiRequest<Secret<BackendConfig>>(resourcePath, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureAuthBackendAsync(string path, BackendConfig backendConfig)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}/tune", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, backendConfig).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetTokenCapabilitiesAsync(string path, string token)
        {
            var requestData = new { path = path, token = token };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetTokenCapabilitiesByAcessorAsync(string path, string tokenAccessor)
        {
            var requestData = new { path = path, accessor = tokenAccessor };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities-accessor", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetCallingTokenCapabilitiesAsync(string path)
        {
            var requestData = new { path = path };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities-self", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RequestHeaderSet>> GetAuditRequestHeadersAsync()
        {
            var response = new RequestHeaderSet();

            var result = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>>("v1/sys/config/auditing/request-headers", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            if (result.Data != null && result.Data.Count == 1)
            {
                foreach (var keyValuePair in result.Data.First().Value)
                {
                    var header = new RequestHeader
                    {
                        Name = keyValuePair.Key,
                        HMAC = keyValuePair.Value.First().Value
                    };

                    response.Headers.Add(header);
                }
            }


            return _polymath.GetMappedSecret(result, response);
        }

        public async Task<Secret<RequestHeader>> GetAuditRequestHeaderAsync(string name)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, Dictionary<string, bool>>>>("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            if (result.Data != null && result.Data.Count == 1)
            {
                return _polymath.GetMappedSecret(result, new RequestHeader
                {
                    Name = result.Data.First().Key,
                    HMAC = result.Data.First().Value.First().Value
                });
            }

            return null;
        }

        public async Task PutAuditRequestHeaderAsync(string name, bool hmac = false)
        {
            var requestData = new
            {
                hmac = hmac
            };

            await _polymath.MakeVaultApiRequest("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteAuditRequestHeaderAsync(string name)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ControlGroup>> GetControlGroupConfigAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroup>>("v1/sys/config/control-group", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureControlGroupAsync(string maxTimeToLive)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Put, new { max_ttl = maxTimeToLive }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteControlGroupConfigAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<bool> GetInitStatusAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<dynamic>("v1/sys/init", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response.initialized;
        }

        public async Task<MasterCredentials> InitAsync(InitOptions initOptions)
        {
            var response = await _polymath.MakeVaultApiRequest<MasterCredentials>("v1/sys/init", HttpMethod.Put, initOptions).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task SealAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/seal", HttpMethod.Put).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/seal-status", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false)
        {
            var requestData = new
            {
                key = masterShareKey,
                reset = resetCompletely
            };

            var response = await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/unseal", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys)
        {
            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await UnsealAsync(masterShareKey);
            }

            return finalStatus;
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            throw new NotImplementedException();
        }

        public Task<Secret<CORSConfig>> GetCORSConfigAsync()
        {
            throw new NotImplementedException();
        }

        public Task ConfigureCORSAsync()
        {
            throw new NotImplementedException();
        }
    }
}
