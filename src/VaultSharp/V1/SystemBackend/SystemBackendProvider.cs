using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.V1.AuthMethods;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend.Enterprise;
using VaultSharp.V1.SystemBackend.MFA;
using VaultSharp.V1.SystemBackend.Plugin;
using Newtonsoft.Json.Linq;

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

        public async Task<Secret<Dictionary<string, AuthMethod>>> GetAuthBackendsAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AuthMethod>>>("v1/sys/auth", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.Path = kv.Key;
            }

            return response;
        }

        public async Task MountAuthBackendAsync(AuthMethod authBackend)
        {
            if (string.IsNullOrWhiteSpace(authBackend.Path))
            {
                authBackend.Path = authBackend.Type.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", authBackend.Path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, authBackend).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuthBackendAsync(string path)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/auth/{0}", path.Trim('/'));
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

        public async Task<Secret<CORSConfig>> GetCORSConfigAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<CORSConfig>>("v1/sys/config/cors", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureCORSAsync(CORSConfig corsConfig)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/cors", HttpMethod.Put, corsConfig).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteCORSConfigAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/cors", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync()
        {
            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/attempt", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(string base64EncodedOneTimePassword, string pgpKey)
        {
            var requestData = new { otp = base64EncodedOneTimePassword, pgpKey = pgpKey };
            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/attempt", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CancelRootTokenGenerationAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/generate-root/attempt", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey, string nonce)
        {
            var requestData = new
            {
                key = masterShareKey,
                nonce = nonce
            };

            return await _polymath.MakeVaultApiRequest<RootTokenGenerationStatus>("v1/sys/generate-root/update", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] thresholdMasterShareKeys, string nonce)
        {
            RootTokenGenerationStatus finalStatus = null;

            foreach (var masterShareKey in thresholdMasterShareKeys)
            {
                finalStatus = await ContinueRootTokenGenerationAsync(masterShareKey, nonce).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

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
            int uninitializedStatusCode = (int)HttpStatusCode.NotImplemented, HttpMethod queryHttpMethod = null)
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
                var healthStatus = await _polymath.MakeVaultApiRequest<HealthStatus>(resourcePath, queryHttpMethod, postResponseAction: message => statusCode = (int)message.StatusCode).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
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
                var healthStatus = JsonConvert.DeserializeObject<HealthStatus>(vaultApiException.Message);
                healthStatus.HttpStatusCode = vaultApiException.StatusCode;

                return healthStatus;
            }
        }

        public async Task<bool> GetInitStatusAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<JToken>("v1/sys/init", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response["initialized"].Value<bool>();
        }

        public async Task<MasterCredentials> InitAsync(InitOptions initOptions)
        {
            var response = await _polymath.MakeVaultApiRequest<MasterCredentials>("v1/sys/init", HttpMethod.Put, initOptions).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<Secret<EncryptionKeyStatus>> GetKeyStatusAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<EncryptionKeyStatus>>("v1/sys/key-status", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Leader> GetLeaderAsync()
        {
            return await _polymath.MakeVaultApiRequest<Leader>("v1/sys/leader", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Lease>> GetLeaseAsync(string leaseId)
        {
            var requestData = new
            {
                lease_id = leaseId
            };

            return await _polymath.MakeVaultApiRequest<Secret<Lease>>("v1/sys/leases/lookup", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetAllLeasesAsync(string prefix)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/leases/lookup/" + prefix.TrimStart('/') + "?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RenewedLease>> RenewLeaseAsync(string leaseId, int incrementSeconds)
        {
            var requestData = new
            {
                lease_id = leaseId,
                increment = incrementSeconds
            };

            return await _polymath.MakeVaultApiRequest<Secret<RenewedLease>>("v1/sys/leases/renew", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeLeaseAsync(string leaseId)
        {
            var requestData = new
            {
                lease_id = leaseId
            };

            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ForceRevokeLeaseAsync(string prefix)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke-force/" + prefix.TrimStart('/'), HttpMethod.Put).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RevokePrefixLeaseAsync(string prefix)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/leases/revoke-prefix/" + prefix.TrimStart('/'), HttpMethod.Put).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, SecretsEngine>>> GetSecretBackendsAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, SecretsEngine>>>("v1/sys/mounts", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.Path = kv.Key;
            }

            return response;
        }

        public async Task MountSecretBackendAsync(SecretsEngine secretBackend)
        {
            if (string.IsNullOrWhiteSpace(secretBackend.Path))
            {
                secretBackend.Path = secretBackend.Type.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}", secretBackend.Path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, secretBackend).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountSecretBackendAsync(string path)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<BackendConfig>> GetSecretBackendConfigAsync(string path)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}/tune", path.Trim('/'));
            return await _polymath.MakeVaultApiRequest<Secret<BackendConfig>>(resourcePath, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureSecretBackendAsync(string path, BackendConfig backendConfig)
        {
            var resourcePath = string.Format(CultureInfo.InvariantCulture, "v1/sys/mounts/{0}/tune", path.Trim('/'));
            await _polymath.MakeVaultApiRequest(resourcePath, HttpMethod.Post, backendConfig).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetPoliciesAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policy", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Policy>> GetPolicyAsync(string policyName)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Policy>>("v1/sys/policy/" + policyName, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WritePolicyAsync(Policy policy)
        {
            var requestData = new
            {
                rules = policy.Rules
            };

            await _polymath.MakeVaultApiRequest("v1/sys/policy/" + policy.Name, HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeletePolicyAsync(string policyName)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policy/" + policyName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetACLPoliciesAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policies/acl?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ACLPolicy>> GetACLPolicyAsync(string policyName)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ACLPolicy>>("v1/sys/policies/acl/" + policyName, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteACLPolicyAsync(ACLPolicy policy)
        {
            var requestData = new
            {
                policy = policy.Policy
            };

            await _polymath.MakeVaultApiRequest("v1/sys/policies/acl/" + policy.Name, HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteACLPolicyAsync(string policyName)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policies/acl/" + policyName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetRawSecretKeysAsync(string storagePathPrefix)
        {
            return await _polymath
                .MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/raw/" + storagePathPrefix.TrimStart('/') + "?list=true",
                    HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadRawSecretAsync(string storagePath)
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<JToken>>("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            string value = response.Data["value"].Value<string>();
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

            return _polymath.GetMappedSecret(response, data);
        }

        public async Task WriteRawSecretAsync(string storagePath, Dictionary<string, object> values)
        {
            var requestData = new
            {
                value = JsonConvert.SerializeObject(values)
            };

            await _polymath.MakeVaultApiRequest("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRawSecretAsync(string storagePath)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/raw/" + storagePath.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyStatus> GetRekeyStatusAsync()
        {
            return await _polymath.MakeVaultApiRequest<RekeyStatus>("v1/sys/rekey/init", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyStatus> InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false)
        {
            var requestData = new { secret_shares = secretShares, secret_threshold = secretThreshold, pgp_keys = pgpKeys, backup = backup };
            return await _polymath.MakeVaultApiRequest<RekeyStatus>("v1/sys/rekey/init", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CancelRekeyAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/rekey/init", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RekeyBackupInfo>> GetRekeyBackupKeysAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<RekeyBackupInfo>>("v1/sys/rekey/backup", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRekeyBackupKeysAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/rekey/backup", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyProgress> ContinueRekeyAsync(string masterShareKey, string rekeyNonce)
        {
            var requestData = new
            {
                key = masterShareKey,
                nonce = rekeyNonce
            };

            return await _polymath.MakeVaultApiRequest<RekeyProgress>("v1/sys/rekey/update", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyProgress> QuickRekeyAsync(string[] allMasterShareKeys, string rekeyNonce)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            RekeyProgress finalRekeyProgress = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalRekeyProgress = await ContinueRekeyAsync(masterShareKey, rekeyNonce).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // don't continue, once threshold keys are achieved.
                if (finalRekeyProgress.Complete)
                {
                    break;
                }
            }

            return finalRekeyProgress;
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

            return await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/unseal", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys)
        {
            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await UnsealAsync(masterShareKey).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // don't continue, once threshold keys are achieved.
                if (!finalStatus.Sealed)
                {
                    break;
                }
            }

            return finalStatus;
        }

        public async Task<Secret<TokenWrapData>> LookupTokenWrapInfoAsync(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<TokenWrapData>>("v1/sys/wrapping/lookup", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> RewrapWrappedResponseDataAsync(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<object>>("v1/sys/wrapping/rewrap", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TData>> UnwrapWrappedResponseDataAsync<TData>(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await _polymath.MakeVaultApiRequest<Secret<TData>>("v1/sys/wrapping/unwrap", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> WrapResponseDataAsync(Dictionary<string, object> data, string wrapTimeToLive)
        {
            return await _polymath.MakeVaultApiRequest<Secret<object>>("v1/sys/wrapping/wrap", HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            throw new NotImplementedException();
        }
    }
}
