using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using VaultSharp.V1.AuthMethods;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend.Enterprise;
using VaultSharp.V1.SystemBackend.MFA;
using VaultSharp.V1.SystemBackend.Plugin;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace VaultSharp.V1.SystemBackend
{
    internal class SystemBackendProvider : ISystemBackend
    {
        private readonly Polymath _polymath;

        public SystemBackendProvider(Polymath polymath)
        {
            _polymath = polymath;

            Enterprise = new EnterpriseProvider(_polymath);
            MFA = new MFAProvider(_polymath);
            Plugins = new PluginProvider(_polymath);
        }

        public IEnterprise Enterprise { get; }

        public IMFA MFA { get; }

        public IPlugin Plugins { get; }

        public async Task<Secret<Dictionary<string, AbstractAuditBackend>>> GetAuditBackendsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AbstractAuditBackend>>>("v1/sys/audit", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.MountPoint = kv.Key;
            }

            return response;
        }

        public async Task MountAuditBackendAsync(AbstractAuditBackend abstractAuditBackend, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(abstractAuditBackend.MountPoint))
            {
                abstractAuditBackend.MountPoint = abstractAuditBackend.Type.Value;
            }

            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + abstractAuditBackend.MountPoint.Trim('/'), HttpMethod.Post, abstractAuditBackend, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuditBackendAsync(string path, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + path.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AuditHash>> AuditHashAsync(string path, string inputToHash, CancellationToken cancellationToken = default)
        {
            var requestData = new InputRequest { Input = inputToHash };

            return await _polymath.MakeVaultApiRequest<Secret<AuditHash>>("v1/sys/audit-hash/" + path.Trim('/'), HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, AuthMethod>>> GetAuthBackendsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AuthMethod>>>("v1/sys/auth", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.Path = kv.Key;
            }

            return response;
        }

        public async Task MountAuthBackendAsync(AuthMethod authBackend, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(authBackend.Path))
            {
                authBackend.Path = authBackend.Type.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", authBackend.Path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, authBackend, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuthBackendAsync(string path, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<BackendConfig>> GetAuthBackendConfigAsync(string path, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}/tune", path.Trim('/'));
            return await _polymath.MakeVaultApiRequest<Secret<BackendConfig>>(resourcePath, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureAuthBackendAsync(string path, BackendConfig backendConfig, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}/tune", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, backendConfig, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetTokenCapabilitiesAsync(string path, string token, CancellationToken cancellationToken = default)
        {
            var requestData = new PathTokenRequest { Path = path, Token = token };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetTokenCapabilitiesByAcessorAsync(string path, string tokenAccessor, CancellationToken cancellationToken = default)
        {
            var requestData = new PathAccessorRequest { Path = path, Accessor = tokenAccessor };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities-accessor", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenCapability>> GetCallingTokenCapabilitiesAsync(string path, CancellationToken cancellationToken = default)
        {
            var requestData = new PathRequest { Path = path };
            return await _polymath.MakeVaultApiRequest<Secret<TokenCapability>>("v1/sys/capabilities-self", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RequestHeaderSet>> GetAuditRequestHeadersAsync(CancellationToken cancellationToken = default)
        {
            var response = new RequestHeaderSet();

            var result = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>>("v1/sys/config/auditing/request-headers", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

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

        public async Task<Secret<RequestHeader>> GetAuditRequestHeaderAsync(string name, CancellationToken cancellationToken = default)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, Dictionary<string, bool>>>>("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

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

        public async Task PutAuditRequestHeaderAsync(string name, bool hmac = false, CancellationToken cancellationToken = default)
        {
            var requestData = new HmacRequest { Hmac = hmac };

            await _polymath.MakeVaultApiRequest("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteAuditRequestHeaderAsync(string name, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/auditing/request-headers/" + name, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CORSConfig>> GetCORSConfigAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CORSConfig>>("v1/sys/config/cors", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureCORSAsync(CORSConfig corsConfig, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/cors", HttpMethod.Put, corsConfig, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteCORSConfigAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/cors", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/attempt", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(string base64EncodedOneTimePassword, string pgpKey, CancellationToken cancellationToken = default)
        {
            var requestData = new OtpPgpKeyRequest { Otp = base64EncodedOneTimePassword, PgpKey = pgpKey };
            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/attempt", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CancelRootTokenGenerationAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/generate-root/attempt", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey, string nonce, CancellationToken cancellationToken = default)
        {
            var requestData = new KeyNonceRequest { Key = masterShareKey, Nonce = nonce };

            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/update", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] thresholdMasterShareKeys, string nonce, CancellationToken cancellationToken = default)
        {
            RootTokenGenerationStatus finalStatus = null;

            foreach (var masterShareKey in thresholdMasterShareKeys)
            {
                finalStatus = await ContinueRootTokenGenerationAsync(masterShareKey, nonce, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // don't continue, once threshold keys are achieved.
                if (finalStatus.Complete)
                {
                    break;
                }
            }

            return finalStatus;
        }

        public async Task<HealthStatus> GetHealthStatusAsync(bool standbyOk = false,
            int activeStatusCode = (int)HttpStatusCode.OK, int standbyStatusCode = 429,
            int sealedStatusCode = (int)HttpStatusCode.ServiceUnavailable,
            int uninitializedStatusCode = (int)HttpStatusCode.NotImplemented, HttpMethod queryHttpMethod = null, CancellationToken cancellationToken = default)
        {
            if (queryHttpMethod != HttpMethod.Head)
            {
                queryHttpMethod = HttpMethod.Get;
            }

            var queryStringBuilder = new List<string>();

            if (standbyOk)
            {
                queryStringBuilder.Add("standbyok=true");
            }

            if (activeStatusCode != (int)HttpStatusCode.OK)
            {
                queryStringBuilder.Add("activecode=" + activeStatusCode);
            }

            if (standbyStatusCode != 429)
            {
                queryStringBuilder.Add("standbycode=" + standbyStatusCode);
            }

            if (sealedStatusCode != (int)HttpStatusCode.ServiceUnavailable)
            {
                queryStringBuilder.Add("sealedcode=" + sealedStatusCode);
            }

            if (uninitializedStatusCode != (int)HttpStatusCode.NotImplemented)
            {
                queryStringBuilder.Add("uninitcode=" + uninitializedStatusCode);
            }

            var resourcePath = "v1/sys/health" + (queryStringBuilder.Any() ? ("?" + string.Join("&", queryStringBuilder)) : string.Empty);

            try
            {
                // we don't know what status code out of 2xx was returned. hence the delegate.

                int? statusCode = null;
                var healthStatus = await _polymath.MakeVaultApiRequest<HealthStatus>(resourcePath, queryHttpMethod, postResponseAction: message => statusCode = (int)message.StatusCode, sendNamespaceHeader: false, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
                healthStatus.HttpStatusCode = statusCode;
                return healthStatus;
            }
            catch (VaultApiException vaultApiException)
            {
                // if response body is empty, don't construct misleading POCO.
                if (string.IsNullOrWhiteSpace(vaultApiException.Message))
                {
                    throw;
                }

                // for head calls, the response is empty. So return a null object, to avoid misleading callers.
#if NET8_0_OR_GREATER
                var healthStatus = (HealthStatus)JsonSerializer.Deserialize(vaultApiException.Message, _polymath.JsonSerializerOptions.GetTypeInfo(typeof(HealthStatus)));
#else
                var healthStatus = JsonSerializer.Deserialize<HealthStatus>(vaultApiException.Message, _polymath.JsonSerializerOptions);
#endif
                healthStatus.HttpStatusCode = vaultApiException.StatusCode;

                return healthStatus;
            }
        }

        public async Task<bool> GetInitStatusAsync(CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<JsonObject>("v1/sys/init", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response["initialized"].GetValue<bool>();
        }

        public async Task<MasterCredentials> InitAsync(InitOptions initOptions, CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<MasterCredentials>("v1/sys/init", HttpMethod.Put, initOptions, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<Secret<EncryptionKeyStatus>> GetKeyStatusAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<EncryptionKeyStatus>>("v1/sys/key-status", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Leader> GetLeaderAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Leader>("v1/sys/leader", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Lease>> GetLeaseAsync(string leaseId, CancellationToken cancellationToken = default)
        {
            var requestData = new LeaseIdRequest { LeaseId = leaseId };

            return await _polymath.MakeVaultApiRequest<Secret<Lease>>("v1/sys/leases/lookup", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetAllLeasesAsync(string prefix, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/leases/lookup/" + prefix.TrimStart('/'), _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RenewedLease>> RenewLeaseAsync(string leaseId, int incrementSeconds, CancellationToken cancellationToken = default)
        {
            var requestData = new LeaseRenewRequest { LeaseId = leaseId, Increment = incrementSeconds };

            return await _polymath.MakeVaultApiRequest<Secret<RenewedLease>>("v1/sys/leases/renew", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeLeaseAsync(string leaseId, CancellationToken cancellationToken = default)
        {
            var requestData = new LeaseIdRequest { LeaseId = leaseId };

            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ForceRevokeLeaseAsync(string prefix, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke-force/" + prefix.TrimStart('/'), HttpMethod.Put, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevokePrefixLeaseAsync(string prefix, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke-prefix/" + prefix.TrimStart('/'), HttpMethod.Put, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> GetVerbosityLevelOfAllLoggersAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/sys/loggers", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> GetVerbosityLevelOfLoggerAsync(string loggerName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/sys/loggers/" + loggerName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ModifyVerbosityLevelForAllLoggersAsync(LogVerbosityLevel logVerbosityLevel, CancellationToken cancellationToken = default)
        {
            var requestData = new LevelRequest { Level = logVerbosityLevel.ToString() };

            await _polymath.MakeVaultApiRequest("v1/sys/loggers", HttpMethod.Post, requestData: requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ModifyVerbosityLevelForLoggerAsync(string loggerName, LogVerbosityLevel logVerbosityLevel, CancellationToken cancellationToken = default)
        {
            var requestData = new LevelRequest { Level = logVerbosityLevel.ToString() };

            await _polymath.MakeVaultApiRequest("v1/sys/loggers/" + loggerName, HttpMethod.Post, requestData: requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevertVerbosityLevelForAllLoggersAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/loggers", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevertVerbosityLevelForLoggerAsync(string loggerName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/loggers/" + loggerName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretsEngine>> GetSecretBackendAsync(string mountPoint, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}", mountPoint.Trim('/'));
            return await _polymath.MakeVaultApiRequest<Secret<SecretsEngine>>(resourcePath, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);            
        }

        public async Task<Secret<Dictionary<string, SecretsEngine>>> GetSecretBackendsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, SecretsEngine>>>("v1/sys/mounts", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.Path = kv.Key;
            }

            return response;
        }

        public async Task MountSecretBackendAsync(SecretsEngine secretBackend, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(secretBackend.Path))
            {
                secretBackend.Path = secretBackend.Type.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}", secretBackend.Path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, secretBackend, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountSecretBackendAsync(string path, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<BackendConfig>> GetSecretBackendConfigAsync(string path, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}/tune", path.Trim('/'));
            return await _polymath.MakeVaultApiRequest<Secret<BackendConfig>>(resourcePath, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureSecretBackendAsync(string path, BackendConfig backendConfig, CancellationToken cancellationToken = default)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}/tune", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, backendConfig, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetPoliciesAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policy", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Policy>> GetPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Policy>>("v1/sys/policy/" + policyName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WritePolicyAsync(Policy policy, CancellationToken cancellationToken = default)
        {
            var requestData = new RulesRequest { Rules = policy.Rules };

            await _polymath.MakeVaultApiRequest("v1/sys/policy/" + policy.Name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeletePolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policy/" + policyName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetACLPoliciesAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policies/acl", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ACLPolicy>> GetACLPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ACLPolicy>>("v1/sys/policies/acl/" + policyName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteACLPolicyAsync(ACLPolicy policy, CancellationToken cancellationToken = default)
        {
            var requestData = new PolicyTextRequest { Policy = policy.Policy };

            await _polymath.MakeVaultApiRequest("v1/sys/policies/acl/" + policy.Name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteACLPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policies/acl/" + policyName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        // raja todo: add password policy configuration apis
        
        public async Task<Secret<string>> GeneratePasswordFromPasswordPolicyAsync(string passwordPolicyName, CancellationToken cancellationToken = default)
        {
            var response = await _polymath
                .MakeVaultApiRequest<Secret<JsonObject>>("v1/sys/policies/password/" + passwordPolicyName + "/generate", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            var result = _polymath.GetMappedSecret(response, response.Data["password"].ToString());
            return result;
        }

        public async Task<Secret<ListInfo>> GetRawSecretKeysAsync(string storagePathPrefix, CancellationToken cancellationToken = default)
        {
            return await _polymath
                .MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/raw/" + storagePathPrefix.TrimStart('/'), _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadRawSecretAsync(string storagePath, CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<JsonObject>>("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            string value = response.Data["value"].ToString();
#if NET8_0_OR_GREATER
            var data = (Dictionary<string, object>)JsonSerializer.Deserialize(value, _polymath.JsonSerializerOptions.GetTypeInfo(typeof(Dictionary<string, object>)));
#else
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(value, _polymath.JsonSerializerOptions);
#endif

            return _polymath.GetMappedSecret(response, data);
        }

        public async Task WriteRawSecretAsync(string storagePath, Dictionary<string, object> values, CancellationToken cancellationToken = default)
        {
#if NET8_0_OR_GREATER
            var requestData = new ValueRequest { Value = JsonSerializer.Serialize(values, _polymath.JsonSerializerOptions.GetTypeInfo(typeof(Dictionary<string, object>))) };
#else
            var requestData = new ValueRequest { Value = JsonSerializer.Serialize(values, _polymath.JsonSerializerOptions) };
#endif

            await _polymath.MakeVaultApiRequest("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRawSecretAsync(string storagePath, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyStatus> GetRekeyStatusAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<RekeyStatus>("v1/sys/rekey/init", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyStatus> InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false, CancellationToken cancellationToken = default)
        {
            var requestData = new RekeyInitRequest { SecretShares = secretShares, SecretThreshold = secretThreshold, PgpKeys = pgpKeys, Backup = backup };
            return await _polymath.MakeVaultApiRequest<RekeyStatus>("v1/sys/rekey/init", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CancelRekeyAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/rekey/init", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RekeyBackupInfo>> GetRekeyBackupKeysAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<RekeyBackupInfo>>("v1/sys/rekey/backup", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRekeyBackupKeysAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/rekey/backup", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyProgress> ContinueRekeyAsync(string masterShareKey, string rekeyNonce, CancellationToken cancellationToken = default)
        {
            var requestData = new KeyNonceRequest { Key = masterShareKey, Nonce = rekeyNonce };

            return await _polymath.MakeVaultApiRequest<RekeyProgress>("v1/sys/rekey/update", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyProgress> QuickRekeyAsync(string[] allMasterShareKeys, string rekeyNonce, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            RekeyProgress finalRekeyProgress = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalRekeyProgress = await ContinueRekeyAsync(masterShareKey, rekeyNonce, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // don't continue, once threshold keys are achieved.
                if (finalRekeyProgress.Complete)
                {
                    break;
                }
            }

            return finalRekeyProgress;
        }

        public async Task SealAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/seal", HttpMethod.Put, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> GetSealStatusAsync(CancellationToken cancellationToken = default)
        {
            var response = await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/seal-status", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false, CancellationToken cancellationToken = default)
        {
            var requestData = new KeyResetRequest { Key = masterShareKey, Reset = resetCompletely };

            return await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/unseal", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys, CancellationToken cancellationToken = default)
        {
            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await UnsealAsync(masterShareKey, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // don't continue, once threshold keys are achieved.
                if (!finalStatus.Sealed)
                {
                    break;
                }
            }

            return finalStatus;
        }

        public async Task<Secret<TokenWrapData>> LookupTokenWrapInfoAsync(string tokenId, CancellationToken cancellationToken = default)
        {
            var requestData = new TokenRequest { Token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<TokenWrapData>>("v1/sys/wrapping/lookup", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> RewrapWrappedResponseDataAsync(string tokenId, CancellationToken cancellationToken = default)
        {
            var requestData = new TokenRequest { Token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<object>>("v1/sys/wrapping/rewrap", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TData>> UnwrapWrappedResponseDataAsync<TData>(string tokenId, CancellationToken cancellationToken = default)
        {
            var requestData = new TokenRequest { Token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<TData>>("v1/sys/wrapping/unwrap", HttpMethod.Post, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> WrapResponseDataAsync(Dictionary<string, object> data, string wrapTimeToLive, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<object>>("v1/sys/wrapping/wrap", HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
