using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Backends.Audit.Models;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.AwsEc2;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Authentication.Providers;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.Secret.Models.AWS;
using VaultSharp.Backends.Secret.Models.Cassandra;
using VaultSharp.Backends.Secret.Models.Consul;
using VaultSharp.Backends.Secret.Models.MicrosoftSql;
using VaultSharp.Backends.Secret.Models.MongoDb;
using VaultSharp.Backends.Secret.Models.MySql;
using VaultSharp.Backends.Secret.Models.PKI;
using VaultSharp.Backends.Secret.Models.PostgreSql;
using VaultSharp.Backends.Secret.Models.RabbitMQ;
using VaultSharp.Backends.Secret.Models.SSH;
using VaultSharp.Backends.Secret.Models.Transit;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp
{
    internal sealed class VaultClient : IVaultClient
    {
        private const string VaultTokenHeaderKey = "X-Vault-Token";

        private const string VaultWrapTimeToLiveHeaderKey = "X-Vault-Wrap-TTL";

        private readonly IDataAccessManager _dataAccessManager;

        // leave it at instance level to avoid any garbage collection scenarios.
        private readonly IAuthenticationProvider _authenticationProvider;

        private readonly Lazy<Task<string>> _lazyVaultToken;

        private readonly bool _continueAsyncTasksOnCapturedContext;

        public VaultClient(Uri vaultServerUriWithPort, IAuthenticationInfo authenticationInfo, bool continueAsyncTasksOnCapturedContext = false, TimeSpan? serviceTimeout = null, IDataAccessManager dataAccessManager = null, Action<HttpClient> postHttpClientInitializeAction = null)
        {
            Checker.NotNull(vaultServerUriWithPort, "vaultServerUriWithPort");

            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;

            var vaultBaseAddress = new Uri(vaultServerUriWithPort, "v1/");

            // some operations can happen without the need to pass any authentication info. (unauthenticated endpoints)
            if (authenticationInfo != null)
            {
                _authenticationProvider = AuthenticationProviderFactory.CreateAuthenticationProvider(
                    authenticationInfo, vaultBaseAddress, serviceTimeout, continueAsyncTasksOnCapturedContext, postHttpClientInitializeAction);

                _lazyVaultToken = new Lazy<Task<string>>(_authenticationProvider.GetTokenAsync);
            }

            _dataAccessManager = dataAccessManager ??
                                 new HttpDataAccessManager(vaultBaseAddress, serviceTimeout: serviceTimeout, postHttpClientInitializeAction: postHttpClientInitializeAction);
        }

        public async Task<bool> GetInitializationStatusAsync()
        {
            var response = await MakeVaultApiRequest<dynamic>("sys/init", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response.initialized;
        }

        public async Task<MasterCredentials> InitializeAsync(InitializeOptions initializeOptions)
        {
            var response = await MakeVaultApiRequest<MasterCredentials>("sys/init", HttpMethod.Put, initializeOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync()
        {
            var response = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/attempt", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(string base64EncodedOneTimePassword = null, string pgpKey = null)
        {
            var requestData = new { otp = base64EncodedOneTimePassword, pgpKey = pgpKey };

            var response = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/attempt", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task CancelRootTokenGenerationAsync()
        {
            await MakeVaultApiRequest("sys/generate-root/attempt", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey, string nonce)
        {
            var requestData = new
            {
                key = masterShareKey,
                nonce = nonce
            };

            var progress = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/update", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return progress;
        }

        public async Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] allMasterShareKeys, string nonce)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            RootTokenGenerationStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await ContinueRootTokenGenerationAsync(masterShareKey, nonce);
            }

            return finalStatus;
        }

        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await MakeVaultApiRequest<SealStatus>("sys/seal-status", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task SealAsync()
        {
            await MakeVaultApiRequest("sys/seal", HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false)
        {
            var requestData = new
            {
                key = masterShareKey,
                reset = resetCompletely
            };

            var response = await MakeVaultApiRequest<SealStatus>("sys/unseal", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await UnsealAsync(masterShareKey);
            }

            return finalStatus;
        }

        public async Task<Secret<IEnumerable<SecretBackend>>> GetAllMountedSecretBackendsAsync()
        {
            var response = await MakeVaultApiRequest<Secret<Dictionary<string, SecretBackend>>>("sys/mounts", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.MountPoint = kv.Key;
            }

            return GetMappedSecret(response, response.Data.Values.AsEnumerable());
        }

        public async Task MountSecretBackendAsync(SecretBackend secretBackend)
        {
            Checker.NotNull(secretBackend, "secretBackend");

            if (string.IsNullOrWhiteSpace(secretBackend.MountPoint))
            {
                secretBackend.MountPoint = secretBackend.BackendType.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}", secretBackend.MountPoint.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Post, secretBackend).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task QuickMountSecretBackendAsync(SecretBackendType secretBackendType)
        {
            Checker.NotNull(secretBackendType, "secretBackendType");

            await MountSecretBackendAsync(new SecretBackend
            {
                BackendType = secretBackendType,
            });
        }

        public async Task UnmountSecretBackendAsync(string mountPoint)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}", mountPoint.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task QuickUnmountSecretBackendAsync(SecretBackendType secretBackendType)
        {
            Checker.NotNull(secretBackendType, "secretBackendType");
            await UnmountSecretBackendAsync(secretBackendType.Type);
        }

        public async Task<Secret<MountConfiguration>> GetMountedSecretBackendConfigurationAsync(string mountPoint)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}/tune", mountPoint.Trim('/'));

            var response = await MakeVaultApiRequest<Secret<MountConfiguration>>(resourcePath, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task TuneSecretBackendConfigurationAsync(string mountPoint, MountConfiguration mountConfiguration = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            if (mountConfiguration == null)
            {
                mountConfiguration = new MountConfiguration
                {
                    DefaultLeaseTtl = "0",
                    MaximumLeaseTtl = "0"
                };
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}/tune", mountPoint.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Post, mountConfiguration).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RemountSecretBackendAsync(string previousMountPoint, string newMountPoint)
        {
            var requestData = new
            {
                from = previousMountPoint,
                to = newMountPoint
            };

            await MakeVaultApiRequest("sys/remount", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<IEnumerable<AuthenticationBackend>>> GetAllEnabledAuthenticationBackendsAsync()
        {
            var response = await MakeVaultApiRequest<Secret<Dictionary<string, AuthenticationBackend>>>("sys/auth", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.AuthenticationPath = kv.Key;
            }

            return GetMappedSecret(response, response.Data.Values.AsEnumerable());
        }

        public async Task EnableAuthenticationBackendAsync(AuthenticationBackend authenticationBackend)
        {
            Checker.NotNull(authenticationBackend, "authenticationBackend");

            if (string.IsNullOrWhiteSpace(authenticationBackend.AuthenticationPath))
            {
                authenticationBackend.AuthenticationPath = authenticationBackend.BackendType.Type;
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/auth/{0}", authenticationBackend.AuthenticationPath.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Post, authenticationBackend).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task QuickEnableAuthenticationBackendAsync(AuthenticationBackendType authenticationBackendType)
        {
            var authenticationBackend = new AuthenticationBackend
            {
                BackendType = authenticationBackendType
            };

            await EnableAuthenticationBackendAsync(authenticationBackend);
        }

        public async Task DisableAuthenticationBackendAsync(string authenticationPath)
        {
            Checker.NotNull(authenticationPath, "authenticationPath");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/auth/{0}", authenticationPath.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<MountConfiguration> GetMountedAuthenticationBackendConfigurationAsync(string authenticationPath)
        {
            Checker.NotNull(authenticationPath, "authenticationPath");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/auth/{0}/tune", authenticationPath.Trim('/'));

            var response = await MakeVaultApiRequest<MountConfiguration>(resourcePath, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task TuneAuthenticationBackendConfigurationAsync(string authenticationPath, MountConfiguration mountConfiguration = null)
        {
            Checker.NotNull(authenticationPath, "authenticationPath");

            if (mountConfiguration == null)
            {
                mountConfiguration = new MountConfiguration
                {
                    DefaultLeaseTtl = "0",
                    MaximumLeaseTtl = "0"
                };
            }

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/auth/{0}/tune", authenticationPath.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Post, mountConfiguration).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<IEnumerable<string>> GetAllPoliciesAsync()
        {
            var response = await MakeVaultApiRequest<dynamic>("sys/policy", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null && response.policies != null)
            {
                return response.policies.ToObject<List<string>>();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<Policy> GetPolicyAsync(string policyName)
        {
            Checker.NotNull(policyName, "policyName");

            var policy = await MakeVaultApiRequest<Policy>("sys/policy/" + policyName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return policy;
        }

        public async Task WritePolicyAsync(Policy policy)
        {
            Checker.NotNull(policy, "policy");
            Checker.NotNull(policy.Name, "policy.Name");

            var requestData = new
            {
                rules = policy.Rules
            };

            await MakeVaultApiRequest("sys/policy/" + policy.Name, HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DeletePolicyAsync(string policyName)
        {
            Checker.NotNull(policyName, "policyName");

            await MakeVaultApiRequest("sys/policy/" + policyName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<IEnumerable<string>> GetTokenCapabilitiesAsync(string token, string path)
        {
            var requestData = new { token = token, path = path };
            var response = await MakeVaultApiRequest<dynamic>("sys/capabilities", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null && response.capabilities != null)
            {
                return response.capabilities.ToObject<List<string>>();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> GetCallingTokenCapabilitiesAsync(string path)
        {
            var requestData = new { path = path };
            var response = await MakeVaultApiRequest<dynamic>("sys/capabilities-self", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null && response.capabilities != null)
            {
                return response.capabilities.ToObject<List<string>>();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> GetTokenAccessorCapabilitiesAsync(string tokenAccessor, string path)
        {
            var requestData = new { accessor = tokenAccessor, path = path };
            var response = await MakeVaultApiRequest<dynamic>("sys/capabilities-accessor", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null && response.capabilities != null)
            {
                return response.capabilities.ToObject<List<string>>();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<Secret<IEnumerable<AuditBackend>>> GetAllEnabledAuditBackendsAsync()
        {
            var response = await MakeVaultApiRequest<Secret<Dictionary<string, AuditBackend>>>("sys/audit", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.MountPoint = kv.Key;
            }

            return GetMappedSecret(response, response.Data.Values.AsEnumerable());
        }

        public async Task EnableAuditBackendAsync(AuditBackend auditBackend)
        {
            Checker.NotNull(auditBackend, "auditBackend");

            if (string.IsNullOrWhiteSpace(auditBackend.MountPoint))
            {
                auditBackend.MountPoint = auditBackend.BackendType.Type;
            }

            await MakeVaultApiRequest("sys/audit/" + auditBackend.MountPoint.Trim('/'), HttpMethod.Put, auditBackend).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DisableAuditBackendAsync(string mountPoint)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            await MakeVaultApiRequest("sys/audit/" + mountPoint.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var requestData = new { input = inputToHash };

            var response = await MakeVaultApiRequest<dynamic>("sys/audit-hash/" + mountPoint.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response != null ? response.hash : null;
        }

        public async Task<Secret<Dictionary<string, object>>> RenewSecretAsync(string leaseId, int? incrementSeconds = null)
        {
            var requestData = new Dictionary<string, object>
            {
                {"lease_id", leaseId}
            };

            if (incrementSeconds.HasValue)
            {
                requestData.Add("increment", incrementSeconds.Value);
            }

            var response = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>("sys/renew", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task RevokeSecretAsync(string leaseId)
        {
            Checker.NotNull(leaseId, "leaseId");

            await MakeVaultApiRequest("sys/revoke/" + leaseId, HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeAllSecretsOrTokensUnderPrefixAsync(string pathPrefix)
        {
            Checker.NotNull(pathPrefix, "pathPrefix");

            await MakeVaultApiRequest("sys/revoke-prefix/" + pathPrefix.Trim('/'), HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task ForceRevokeAllSecretsOrTokensUnderPrefixAsync(string pathPrefix)
        {
            Checker.NotNull(pathPrefix, "pathPrefix");

            await MakeVaultApiRequest("sys/revoke-force/" + pathPrefix.Trim('/'), HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenWrapInfo>> LookupTokenWrapInfoAsync(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await MakeVaultApiRequest<Secret<TokenWrapInfo>>("sys/wrapping/lookup", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> RewrapWrappedResponseDataAsync(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await MakeVaultApiRequest<Secret<object>>("sys/wrapping/rewrap", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> UnwrapWrappedResponseDataAsync(string tokenId)
        {
            return await UnwrapWrappedResponseDataAsync<Dictionary<string, object>>(tokenId);
        }

        public async Task<Secret<TData>> UnwrapWrappedResponseDataAsync<TData>(string tokenId)
        {
            var requestData = new { token = tokenId };
            return await MakeVaultApiRequest<Secret<TData>>("sys/wrapping/unwrap", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> WrapResponseDataAsync(Dictionary<string, object> data, string wrapTimeToLive)
        {
            return await MakeVaultApiRequest<Secret<object>>("sys/wrapping/wrap", HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Leader> GetLeaderAsync()
        {
            var leader = await MakeVaultApiRequest<Leader>("sys/leader", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return leader;
        }

        public async Task StepDownActiveNodeAsync()
        {
            await MakeVaultApiRequest("sys/step-down", HttpMethod.Put).ConfigureAwait(_continueAsyncTasksOnCapturedContext);
        }

        public async Task<EncryptionKeyStatus> GetEncryptionKeyStatusAsync()
        {
            var keyStatus = await MakeVaultApiRequest<EncryptionKeyStatus>("sys/key-status", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return keyStatus;
        }

        public async Task<RekeyStatus> GetRekeyStatusAsync()
        {
            var rekeyStatus = await MakeVaultApiRequest<RekeyStatus>("sys/rekey/init", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return rekeyStatus;
        }

        public async Task<RekeyStatus> InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false)
        {
            var requestData = new { secret_shares = secretShares, secret_threshold = secretThreshold, pgp_keys = pgpKeys, backup = backup };
            return await MakeVaultApiRequest<RekeyStatus>("sys/rekey/init", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task CancelRekeyAsync()
        {
            await MakeVaultApiRequest("sys/rekey/init", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyBackupInfo> GetRekeyBackupKeysAsync()
        {
            var rekeyBackupInfo = await MakeVaultApiRequest<RekeyBackupInfo>("sys/rekey/backup", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return rekeyBackupInfo;
        }

        public async Task DeleteRekeyBackupKeysAsync()
        {
            await MakeVaultApiRequest("sys/rekey/backup", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<RekeyProgress> ContinueRekeyAsync(string masterShareKey, string rekeyNonce)
        {
            var requestData = new
            {
                key = masterShareKey,
                nonce = rekeyNonce
            };

            var rekeyProgress = await MakeVaultApiRequest<RekeyProgress>("sys/rekey/update", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return rekeyProgress;
        }

        public async Task<RekeyProgress> QuickRekeyAsync(string[] allMasterShareKeys, string rekeyNonce)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            RekeyProgress finalRekeyProgress = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalRekeyProgress = await ContinueRekeyAsync(masterShareKey, rekeyNonce);
            }

            return finalRekeyProgress;
        }

        public async Task RotateEncryptionKeyAsync()
        {
            await MakeVaultApiRequest("sys/rotate", HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RawData>> ReadRawSecretAsync(string storagePath)
        {
            Checker.NotNull(storagePath, "storagePath");

            return await MakeVaultApiRequest<Secret<RawData>>("sys/raw/" + storagePath.Trim('/'), HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRawSecretAsync(string storagePath, IDictionary<string, object> values)
        {
            Checker.NotNull(storagePath, "storagePath");

            var requestData = new
            {
                value = JsonConvert.SerializeObject(values)
            };

            await MakeVaultApiRequest("sys/raw/" + storagePath.Trim('/'), HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRawSecretAsync(string storagePath)
        {
            Checker.NotNull(storagePath, "storagePath");

            await MakeVaultApiRequest("sys/raw/" + storagePath.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        // this API is a bit quirky/hacky since it is aware of HttpResponseMessage
        // eventually, i need to solve this in a cleaner manner. haha.. eventually! funny word in software.
        public async Task<HealthStatus> GetHealthStatusAsync(bool? standbyOk = null, int? activeStatusCode = null, int? standbyStatusCode = null, int? sealedStatusCode = null, int? uninitializedStatusCode = null, HttpMethod queryHttpMethod = null)
        {
            var failureHealthStatus = new HealthStatus();
            var expectedFailure = false;

            try
            {
                if (queryHttpMethod != HttpMethod.Head)
                {
                    queryHttpMethod = HttpMethod.Get;
                }

                var queryStringBuilder = new List<string>();

                if (standbyOk == true)
                {
                    queryStringBuilder.Add("standbyok=true");
                }

                if (activeStatusCode == null)
                {
                    activeStatusCode = 200;
                }

                if (standbyStatusCode == null)
                {
                    standbyStatusCode = 429;
                }

                if (sealedStatusCode == null)
                {
                    sealedStatusCode = 503;
                }

                if (uninitializedStatusCode == null)
                {
                    uninitializedStatusCode = 501;
                }

                queryStringBuilder.Add("activecode=" + activeStatusCode.Value);
                queryStringBuilder.Add("standbycode=" + standbyStatusCode.Value);
                queryStringBuilder.Add("sealedcode=" + sealedStatusCode.Value);
                queryStringBuilder.Add("uninitcode=" + uninitializedStatusCode.Value);

                var queryString = string.Join("&", queryStringBuilder);
                var resourcePath = "sys/health?" + queryString;

                var healthStatus =
                    await MakeVaultApiRequest(resourcePath, queryHttpMethod,
                        customProcessor: (statusCode, responseText) =>
                        {
                            if (statusCode == activeStatusCode.Value
                                || (statusCode == standbyStatusCode.Value && standbyOk != true)
                                || statusCode == sealedStatusCode.Value
                                || statusCode == uninitializedStatusCode.Value)
                            {
                                expectedFailure = true;

                                if (!string.IsNullOrWhiteSpace(responseText))
                                {
                                    failureHealthStatus = JsonConvert.DeserializeObject<HealthStatus>(responseText);
                                }

                                return failureHealthStatus;

                                // there is a bad case, of empty response but matching code here.
                                // the status will have default values populated which might confuse the callers.
                                // the workaround is for the callers to mess with the http status codes too much.
                                // see https://github.com/hashicorp/vault/issues/1849
                            }

                            throw new Exception(string.Format(CultureInfo.InvariantCulture,
                                "Http Status Code {0}. {1}",
                                statusCode, responseText));

                        }).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

                healthStatus.HealthCheckSucceeded = !expectedFailure;
                return healthStatus;
            }
            catch (Exception ex)
            {
                failureHealthStatus.HealthCheckSucceeded = false;
                failureHealthStatus.ErrorMessage = expectedFailure
                    ? "The Vault is sealed or uninitialized or in standby. Please check the response to know the exact reason."
                    : ex.Message;

                return failureHealthStatus;
            }
        }

        public async Task AWSConfigureRootCredentialsAsync(AWSRootCredentials awsRootCredentials = null, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/config/root", HttpMethod.Post, awsRootCredentials).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AWSConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AWSWriteNamedRoleAsync(string awsRoleName, AWSRoleDefinition awsRoleDefinition, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Post, awsRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSRoleDefinition>> AWSReadNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            var result =
                await
                    MakeVaultApiRequest<Secret<AWSRoleDefinition>>(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                        .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            return result;
        }

        public async Task AWSDeleteNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> AWSGetRoleListAsync(string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            return await MakeVaultApiRequest<Secret<ListInfo>>(awsBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> AWSGenerateDynamicCredentialsAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            var result = await MakeVaultApiRequest<Secret<AWSCredentials>>(awsBackendMountPoint.Trim('/') + "/creds/" + awsRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<AWSCredentials>> AWSGenerateDynamicCredentialsWithSecurityTokenAsync(string awsRoleName, string timeToLive = "1h", string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            object requestData = string.IsNullOrWhiteSpace(timeToLive) ? null : new { ttl = timeToLive };
            var method = string.IsNullOrWhiteSpace(timeToLive) ? HttpMethod.Get : HttpMethod.Post;

            var result = await MakeVaultApiRequest<Secret<AWSCredentials>>(awsBackendMountPoint.Trim('/') + "/sts/" + awsRoleName, method, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CassandraConfigureConnectionAsync(CassandraConnectionInfo cassandraConnectionInfo, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");

            if (cassandraConnectionInfo == null)
            {
                cassandraConnectionInfo = new CassandraConnectionInfo();
            }

            // https://www.vaultproject.io/docs/secrets/cassandra/index.html
            if (!cassandraConnectionInfo.UseTLS &&
                (cassandraConnectionInfo.InsecureTLS || !string.IsNullOrWhiteSpace(cassandraConnectionInfo.PemBundle) ||
                 !string.IsNullOrWhiteSpace(cassandraConnectionInfo.PemJson)))
            {
                cassandraConnectionInfo.UseTLS = true;
            }

            await MakeVaultApiRequest(cassandraBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, cassandraConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task CassandraWriteNamedRoleAsync(string cassandraRoleName, CassandraRoleDefinition cassandraRoleDefinition, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            await MakeVaultApiRequest(cassandraBackendMountPoint.Trim('/') + "/roles/" + cassandraRoleName, HttpMethod.Post, cassandraRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CassandraRoleDefinition>> CassandraReadNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra, string wrapTimeToLive = null)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            var result = await MakeVaultApiRequest<Secret<CassandraRoleDefinition>>(cassandraBackendMountPoint.Trim('/') + "/roles/" + cassandraRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CassandraDeleteNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            await MakeVaultApiRequest(cassandraBackendMountPoint.Trim('/') + "/roles/" + cassandraRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> CassandraGenerateDynamicCredentialsAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra, string wrapTimeToLive = null)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(cassandraBackendMountPoint.Trim('/') + "/creds/" + cassandraRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task ConsulConfigureAccessAsync(ConsulAccessInfo consulAccessInfo, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/config/access", HttpMethod.Post, consulAccessInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task ConsulWriteNamedRoleAsync(string consulRoleName, ConsulRoleDefinition consulRoleDefinition, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Post, consulRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulRoleDefinition>> ConsulReadNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            var result = await MakeVaultApiRequest<Secret<ConsulRoleDefinition>>(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> ConsulReadRoleListAsync(string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            return await MakeVaultApiRequest<Secret<ListInfo>>(consulBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task ConsulDeleteNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulCredentials>> ConsulGenerateDynamicCredentialsAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            var result = await MakeVaultApiRequest<Secret<ConsulCredentials>>(consulBackendMountPoint.Trim('/') + "/creds/" + consulRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<Dictionary<string, object>>> CubbyholeReadSecretAsync(string locationPath, string wrapTimeToLive = null)
        {
            Checker.NotNull(locationPath, "locationPath");

            var result = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(SecretBackendType.CubbyHole.Type + "/" + locationPath.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> CubbyholeReadSecretLocationPathListAsync(string locationPath, string wrapTimeToLive = null)
        {
            var result = await MakeVaultApiRequest<Secret<ListInfo>>(SecretBackendType.CubbyHole.Type + "/" + (locationPath ?? string.Empty).Trim('/') + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CubbyholeWriteSecretAsync(string locationPath, IDictionary<string, object> values)
        {
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(SecretBackendType.CubbyHole.Type + "/" + locationPath.Trim('/'), HttpMethod.Put, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task CubbyholeDeleteSecretAsync(string locationPath)
        {
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(SecretBackendType.CubbyHole.Type + "/" + locationPath.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> GenericReadSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic, string wrapTimeToLive = null)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            var result = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> GenericReadSecretLocationPathListAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic, string wrapTimeToLive = null)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(genericBackendMountPoint.Trim('/') + "/" + (locationPath ?? string.Empty).Trim('/') + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task GenericWriteSecretAsync(string locationPath, IDictionary<string, object> values, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Put, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task GenericDeleteSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<object>> MongoDbConfigureConnectionAsync(MongoDbConnectionInfo mongoDbConnectionInfo, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");

            return await MakeVaultApiRequest<Secret<object>>(mongoDbBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, mongoDbConnectionInfo, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MongoDbConnectionInfo>> MongoDbReadConnectionInfoAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            return await MakeVaultApiRequest<Secret<MongoDbConnectionInfo>>(mongoDbBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MongoDbConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");

            await MakeVaultApiRequest(mongoDbBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialTimeToLiveSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CredentialTimeToLiveSettings>> MongoDbReadCredentialLeaseSettingsAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            return await MakeVaultApiRequest<Secret<CredentialTimeToLiveSettings>>(mongoDbBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MongoDbWriteNamedRoleAsync(string mongoDbRoleName, MongoDbRoleDefinition mongoDbRoleDefinition, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            Checker.NotNull(mongoDbRoleName, "mongoDbRoleName");

            await MakeVaultApiRequest(mongoDbBackendMountPoint.Trim('/') + "/roles/" + mongoDbRoleName, HttpMethod.Post, mongoDbRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MongoDbRoleDefinition>> MongoDbReadNamedRoleAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            Checker.NotNull(mongoDbRoleName, "mongoDbRoleName");

            return await MakeVaultApiRequest<Secret<MongoDbRoleDefinition>>(mongoDbBackendMountPoint.Trim('/') + "/roles/" + mongoDbRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> MongoDbReadRoleListAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            return await MakeVaultApiRequest<Secret<ListInfo>>(mongoDbBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MongoDbDeleteNamedRoleAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            Checker.NotNull(mongoDbRoleName, "mongoDbRoleName");

            await MakeVaultApiRequest(mongoDbBackendMountPoint.Trim('/') + "/roles/" + mongoDbRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MongoDbUsernamePasswordCredentials>> MongoDbGenerateDynamicCredentialsAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null)
        {
            Checker.NotNull(mongoDbBackendMountPoint, "mongoDbBackendMountPoint");
            Checker.NotNull(mongoDbRoleName, "mongoDbRoleName");

            return await MakeVaultApiRequest<Secret<MongoDbUsernamePasswordCredentials>>(mongoDbBackendMountPoint.Trim('/') + "/creds/" + mongoDbRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlConfigureConnectionAsync(MicrosoftSqlConnectionInfo microsoftSqlConnectionInfo, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, microsoftSqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MicrosoftSqlConnectionInfo>> MicrosoftSqlReadConnectionInfoAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<MicrosoftSqlConnectionInfo>>(microsoftSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialTimeToLiveSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CredentialTimeToLiveSettings>> MicrosoftSqlReadCredentialLeaseSettingsAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<CredentialTimeToLiveSettings>>(microsoftSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlWriteNamedRoleAsync(string microsoftSqlRoleName, MicrosoftSqlRoleDefinition microsoftSqlRoleDefinition, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Post, microsoftSqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MicrosoftSqlRoleDefinition>> MicrosoftSqlReadNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<MicrosoftSqlRoleDefinition>>(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> MicrosoftSqlReadRoleListAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(microsoftSqlBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MicrosoftSqlDeleteNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> MicrosoftSqlGenerateDynamicCredentialsAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(microsoftSqlBackendMountPoint.Trim('/') + "/creds/" + microsoftSqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MySqlConfigureConnectionAsync(MySqlConnectionInfo mySqlConnectionInfo, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, mySqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        // raja todo.. this is not documented in the Vault site.
        public async Task<Secret<MySqlConnectionInfo>> MySqlReadConnectionInfoAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<MySqlConnectionInfo>>(mySqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MySqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        // raja.. not documented in Vault site.
        public async Task<Secret<CredentialLeaseSettings>> MySqlReadCredentialLeaseSettingsAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<CredentialLeaseSettings>>(mySqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MySqlWriteNamedRoleAsync(string mySqlRoleName, MySqlRoleDefinition mySqlRoleDefinition, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Post, mySqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MySqlRoleDefinition>> MySqlReadNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            var result = await MakeVaultApiRequest<Secret<MySqlRoleDefinition>>(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> MySqlReadRoleListAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(mySqlBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MySqlDeleteNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> MySqlGenerateDynamicCredentialsAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(mySqlBackendMountPoint.Trim('/') + "/creds/" + mySqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<RawCertificateData> PKIReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var format = certificateFormat == CertificateFormat.pem ? "/" + CertificateFormat.pem : string.Empty;
            var outputFormat = certificateFormat == CertificateFormat.pem
                ? CertificateFormat.pem
                : CertificateFormat.der;

            var result = await MakeVaultApiRequest<string>(pkiBackendMountPoint.Trim('/') + "/ca" + format, HttpMethod.Get, rawResponse: true).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return new RawCertificateData
            {
                CertificateContent = result,
                EncodedCertificateFormat = outputFormat
            };
        }

        public async Task<Secret<RawCertificateData>> PKIReadCertificateAsync(string predicate, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(predicate, "predicate");

            var result = await MakeVaultApiRequest<Secret<RawCertificateData>>(pkiBackendMountPoint.Trim('/') + "/cert/" + predicate, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.EncodedCertificateFormat = CertificateFormat.pem;

            return result;
        }

        public async Task<Secret<ListInfo>> PKIReadCertificateListAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(pkiBackendMountPoint.Trim('/') + "/certs/?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PKIConfigureCACertificateAsync(string pemBundle, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var requestData = new { pem_bundle = pemBundle };
            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/config/ca", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ExpiryData>> PKIReadCRLExpirationAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ExpiryData>>(pkiBackendMountPoint.Trim('/') + "/config/crl", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PKIWriteCRLExpirationAsync(string expiry = "72h", string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var requestData = new { expiry = expiry };
            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/config/crl", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CertificateEndpointData>> PKIReadCertificateEndpointsAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<CertificateEndpointData>>(pkiBackendMountPoint.Trim('/') + "/config/urls", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PKIWriteCertificateEndpointsAsync(CertificateEndpointOptions certificateEndpointOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(certificateEndpointOptions, "certificateEndpointOptions");

            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/config/urls", HttpMethod.Post, certificateEndpointOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<RawCertificateData> PKIReadCRLCertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var format = certificateFormat == CertificateFormat.pem ? "/" + CertificateFormat.pem : string.Empty;
            var outputFormat = certificateFormat == CertificateFormat.pem
                ? CertificateFormat.pem
                : CertificateFormat.der;

            var result = await MakeVaultApiRequest<string>(pkiBackendMountPoint.Trim('/') + "/crl" + format, HttpMethod.Get, rawResponse: true).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return new RawCertificateData
            {
                CertificateContent = result,
                EncodedCertificateFormat = outputFormat
            };
        }

        public async Task<Secret<bool>> PKIRotateCRLAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<dynamic>>(pkiBackendMountPoint.Trim('/') + "/crl/rotate", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return GetMappedSecret(result, (bool)result.Data.success);
        }

        public async Task<Secret<RawCertificateSigningRequestData>> PKIGenerateIntermediateCACertificateSigningRequestAsync(CertificateSigningRequestOptions certificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(certificateSigningRequestOptions, "certificateSigningRequestOptions");

            var privateKeyAction = certificateSigningRequestOptions.ExportPrivateKey ? "exported" : "internal";

            var result = await MakeVaultApiRequest<Secret<RawCertificateSigningRequestData>>(pkiBackendMountPoint.Trim('/') + "/intermediate/generate/" + privateKeyAction, HttpMethod.Post, certificateSigningRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.EncodedCertificateFormat = certificateSigningRequestOptions.CertificateFormat;

            return result;
        }

        public async Task PKISetSignedIntermediateCACertificateAsync(string certificateInPemFormat, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var requestData = new { certificate = certificateInPemFormat };
            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/intermediate/set-signed", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CertificateCredentials>> PKIGenerateDynamicCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(certificateCredentialRequestOptions, "certificateCredentialRequestOptions");

            var result = await MakeVaultApiRequest<Secret<CertificateCredentials>>(pkiBackendMountPoint.Trim('/') + "/issue/" + pkiRoleName, HttpMethod.Post, certificateCredentialRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = certificateCredentialRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<RevocationData>> PKIRevokeCertificateAsync(string serialNumber, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var requestData = new { serial_number = serialNumber };

            var result = await MakeVaultApiRequest<Secret<RevocationData>>(pkiBackendMountPoint.Trim('/') + "/revoke", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PKIWriteNamedRoleAsync(string pkiRoleName, CertificateRoleDefinition certificateRoleDefinition = null, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");

            if (certificateRoleDefinition == null)
            {
                certificateRoleDefinition = new CertificateRoleDefinition();
            }

            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/roles/" + pkiRoleName, HttpMethod.Post, certificateRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CertificateRoleDefinition>> PKIReadNamedRoleAsync(string pkiRoleName, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");

            var result = await MakeVaultApiRequest<Secret<CertificateRoleDefinition>>(pkiBackendMountPoint.Trim('/') + "/roles/" + pkiRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> PKIReadRoleListAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(pkiBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PKIDeleteNamedRoleAsync(string pkiRoleName, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");

            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/roles/" + pkiRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RootCertificateData>> PKIGenerateRootCACertificateAsync(RootCertificateRequestOptions rootCertificateRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(rootCertificateRequestOptions, "rootCertificateRequestOptions");

            var privateKeyAction = rootCertificateRequestOptions.ExportPrivateKey ? "exported" : "internal";

            var result = await MakeVaultApiRequest<Secret<RootCertificateData>>(pkiBackendMountPoint.Trim('/') + "/root/generate/" + privateKeyAction, HttpMethod.Post, rootCertificateRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = rootCertificateRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<IntermediateCertificateData>> PKISignIntermediateCACertificateAsync(IntermediateCertificateRequestOptions intermediateCertificateRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(intermediateCertificateRequestOptions, "intermediateCertificateRequestOptions");

            var result = await MakeVaultApiRequest<Secret<IntermediateCertificateData>>(pkiBackendMountPoint.Trim('/') + "/root/sign-intermediate", HttpMethod.Post, intermediateCertificateRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = intermediateCertificateRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<NewCertificateData>> PKISignCertificateAsync(string pkiRoleName, NewCertificateSigningRequestOptions newCertificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(pkiRoleName, "pkiRoleName");
            Checker.NotNull(newCertificateSigningRequestOptions, "newCertificateSigningRequestOptions");

            var result = await MakeVaultApiRequest<Secret<NewCertificateData>>(pkiBackendMountPoint.Trim('/') + "/sign/" + pkiRoleName, HttpMethod.Post, newCertificateSigningRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = newCertificateSigningRequestOptions.CertificateFormat;

            return result;
        }

        public async Task<Secret<NewCertificateData>> PKISignCertificateVerbatimAsync(VerbatimCertificateSigningRequestOptions verbatimCertificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");
            Checker.NotNull(verbatimCertificateSigningRequestOptions, "verbatimCertificateSigningRequestOptions");

            var result = await MakeVaultApiRequest<Secret<NewCertificateData>>(pkiBackendMountPoint.Trim('/') + "/sign-verbatim", HttpMethod.Post, verbatimCertificateSigningRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.CertificateFormat = verbatimCertificateSigningRequestOptions.CertificateFormat;

            return result;
        }

        public async Task PKITidyAsync(TidyRequestOptions tidyRequestOptions = null, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            if (tidyRequestOptions == null)
            {
                tidyRequestOptions = new TidyRequestOptions();
            }

            await MakeVaultApiRequest(pkiBackendMountPoint.Trim('/') + "/tidy", HttpMethod.Post, tidyRequestOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task PostgreSqlConfigureConnectionAsync(PostgreSqlConnectionInfo postgreSqlConnectionInfo, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, postgreSqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<PostgreSqlConnectionInfo>> PostgreSqlReadConnectionInfoAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<PostgreSqlConnectionInfo>>(postgreSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task PostgreSqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CredentialLeaseSettings>> PostgreSqlReadCredentialLeaseSettingsAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");

            return await MakeVaultApiRequest<Secret<CredentialLeaseSettings>>(postgreSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task PostgreSqlWriteNamedRoleAsync(string postgreSqlRoleName, PostgreSqlRoleDefinition postgreSqlRoleDefinition, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Post, postgreSqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<PostgreSqlRoleDefinition>> PostgreSqlReadNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<PostgreSqlRoleDefinition>>(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> PostgreSqlReadRoleListAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(postgreSqlBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PostgreSqlDeleteNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> PostgreSqlGenerateDynamicCredentialsAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(postgreSqlBackendMountPoint.Trim('/') + "/creds/" + postgreSqlRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task RabbitMQConfigureConnectionAsync(RabbitMQConnectionInfo rabbitMQConnectionInfo, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(rabbitMQConnectionInfo, "rabbitMQConnectionInfo");

            await MakeVaultApiRequest(rabbitMQBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, rabbitMQConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        //public async Task<Secret<RabbitMQConnectionInfo>> RabbitMQReadConnectionInfoAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ)
        //{
        //    Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");

        //    return await MakeVaultApiRequest<Secret<RabbitMQConnectionInfo>>(rabbitMQBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        //}

        public async Task RabbitMQConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(credentialTimeToLiveSettings, "credentialTimeToLiveSettings");

            await MakeVaultApiRequest(rabbitMQBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialTimeToLiveSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CredentialTimeToLiveSettings>> RabbitMQReadCredentialLeaseSettingsAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");

            return await MakeVaultApiRequest<Secret<CredentialTimeToLiveSettings>>(rabbitMQBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RabbitMQWriteNamedRoleAsync(string rabbitMQRoleName, RabbitMQRoleDefinition rabbitMQRoleDefinition, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(rabbitMQRoleName, "rabbitMQRoleName");

            await MakeVaultApiRequest(rabbitMQBackendMountPoint.Trim('/') + "/roles/" + rabbitMQRoleName, HttpMethod.Post, rabbitMQRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RabbitMQRoleDefinition>> RabbitMQReadNamedRoleAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(rabbitMQRoleName, "rabbitMQRoleName");

            var result = await MakeVaultApiRequest<Secret<RabbitMQRoleDefinition>>(rabbitMQBackendMountPoint.Trim('/') + "/roles/" + rabbitMQRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        // raja todo: not in doc site.

        public async Task<Secret<ListInfo>> RabbitMQReadRoleListAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(rabbitMQBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task RabbitMQDeleteNamedRoleAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(rabbitMQRoleName, "rabbitMQRoleName");

            await MakeVaultApiRequest(rabbitMQBackendMountPoint.Trim('/') + "/roles/" + rabbitMQRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> RabbitMQGenerateDynamicCredentialsAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null)
        {
            Checker.NotNull(rabbitMQBackendMountPoint, "rabbitMQBackendMountPoint");
            Checker.NotNull(rabbitMQRoleName, "rabbitMQRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(rabbitMQBackendMountPoint.Trim('/') + "/creds/" + rabbitMQRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task SSHWriteNamedKeyAsync(string sshKeyName, string sshPrivateKey, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshKeyName, "sshKeyName");

            var requestData = new { key = sshPrivateKey };
            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/keys/" + sshKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task SSHDeleteNamedKeyAsync(string sshKeyName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshKeyName, "sshKeyName");

            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/keys/" + sshKeyName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task SSHWriteNamedRoleAsync(string sshRoleName, SSHRoleDefinition sshRoleDefinition, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");
            Checker.NotNull(sshRoleDefinition, "sshRoleDefinition");

            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/roles/" + sshRoleName, HttpMethod.Post, sshRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHRoleDefinition>> SSHReadNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");

            var result = await MakeVaultApiRequest<Secret<SSHRoleDefinition>>(sshBackendMountPoint.Trim('/') + "/roles/" + sshRoleName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> SSHReadRoleListAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(sshBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task SSHDeleteNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");

            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/roles/" + sshRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHRoleData>> SSHReadZeroAddressRolesAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<SSHRoleData>>(sshBackendMountPoint.Trim('/') + "/config/zeroaddress", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task SSHConfigureZeroAddressRolesAsync(string roleNames, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(roleNames, "roleNames");

            var requestData = new { roles = roleNames };
            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/config/zeroaddress", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task SSHDeleteZeroAddressRolesAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/config/zeroaddress", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHCredentials>> SSHGenerateDynamicCredentialsAsync(string sshRoleName, string ipAddress, string username = null, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { username = username, ip = ipAddress };
            return await MakeVaultApiRequest<Secret<SSHCredentials>>(sshBackendMountPoint.Trim('/') + "/creds/" + sshRoleName, HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHRoleData>> SSHLookupRolesAsync(string ipAddress, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { ip = ipAddress };
            return await MakeVaultApiRequest<Secret<SSHRoleData>>(sshBackendMountPoint.Trim('/') + "/lookup", HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHOTPVerificationData>> SSHVerifyOTPAsync(string otp, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(otp, "otp");

            var requestData = new { otp = otp };

            var response = await MakeVaultApiRequest<Secret<SSHOTPVerificationData>>(sshBackendMountPoint.Trim('/') + "/verify", HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task TransitCreateEncryptionKeyAsync(string encryptionKeyName, TransitKeyType transitKeyType = TransitKeyType.aes256_gcm96, bool mustUseKeyDerivation = false, bool doConvergentEncryption = false, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            // keep type as enum so that the natural json converter will kick-in for hyphen handling. don't convert to string.
            var requestData = new { type = transitKeyType, derived = mustUseKeyDerivation, convergent_encryption = doConvergentEncryption };
            await MakeVaultApiRequest(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TransitEncryptionKeyInfo>> TransitGetEncryptionKeyInfoAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var result = await MakeVaultApiRequest<Secret<TransitEncryptionKeyInfo>>(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> TransitGetEncryptionKeyListAsync(string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(transitBackendMountPoint.Trim('/') + "/keys?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task TransitDeleteEncryptionKeyAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            await MakeVaultApiRequest(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task TransitConfigureEncryptionKeyAsync(string encryptionKeyName, int minimumDecryptionVersion = 0, bool isDeletionAllowed = false, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { min_decryption_version = minimumDecryptionVersion, deletion_allowed = isDeletionAllowed };
            await MakeVaultApiRequest(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName + "/config", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task TransitRotateEncryptionKeyAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            await MakeVaultApiRequest(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName + "/rotate", HttpMethod.Post).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CipherTextData>> TransitEncryptAsync(string encryptionKeyName, string base64EncodedPlainText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { plaintext = base64EncodedPlainText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<CipherTextData>>(transitBackendMountPoint.Trim('/') + "/encrypt/" + encryptionKeyName, HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<PlainTextData>> TransitDecryptAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { ciphertext = cipherText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<PlainTextData>>(transitBackendMountPoint.Trim('/') + "/decrypt/" + encryptionKeyName, HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<CipherTextData>> TransitRewrapWithLatestEncryptionKeyAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { ciphertext = cipherText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<CipherTextData>>(transitBackendMountPoint.Trim('/') + "/rewrap/" + encryptionKeyName, HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<TransitKeyData>> TransitCreateDataKeyAsync(string encryptionKeyName, bool returnKeyAsPlainText = false, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, int keyBits = 256, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var plainorWrapped = returnKeyAsPlainText ? "plaintext" : "wrapped";
            var requestData = new { context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce, bits = keyBits };

            var result = await MakeVaultApiRequest<Secret<TransitKeyData>>(transitBackendMountPoint.Trim('/') + "/datakey/" + plainorWrapped + "/" + encryptionKeyName, HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<dynamic>> TransitGenerateRandomBytes(int bytesToReturn = 32, string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(format, "format");

            var requestData = new { bytes = bytesToReturn, format = format };

            var result = await MakeVaultApiRequest<Secret<dynamic>>(transitBackendMountPoint.Trim('/') + "/random", HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<dynamic>> TransitHashInput(string base64EncodedInput, string algorithm = "sha2-256", string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(base64EncodedInput, "base64EncodedInput");
            Checker.NotNull(algorithm, "algorithm");
            Checker.NotNull(format, "format");

            var requestData = new { algorithm = algorithm, input = base64EncodedInput, format = format };

            var result = await MakeVaultApiRequest<Secret<dynamic>>(transitBackendMountPoint.Trim('/') + "/hash", HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<dynamic>> TransitDigestInput(string keyName, string base64EncodedInput, string algorithm = "sha2-256", string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(base64EncodedInput, "base64EncodedInput");
            Checker.NotNull(algorithm, "algorithm");
            Checker.NotNull(format, "format");

            var requestData = new { algorithm = algorithm, input = base64EncodedInput, format = format };

            var result = await MakeVaultApiRequest<Secret<dynamic>>(transitBackendMountPoint.Trim('/') + "/hmac/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<dynamic>> TransitSignInput(string keyName, string base64EncodedInput, string algorithm = "sha2-256", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(base64EncodedInput, "base64EncodedInput");
            Checker.NotNull(algorithm, "algorithm");

            var requestData = new { algorithm = algorithm, input = base64EncodedInput };

            var result = await MakeVaultApiRequest<Secret<dynamic>>(transitBackendMountPoint.Trim('/') + "/sign/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<dynamic>> TransitVerifySignature(string keyName, string base64EncodedInput, string signature = null, string hmac = null, string algorithm = "sha2-256", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(base64EncodedInput, "base64EncodedInput");
            Checker.NotNull(algorithm, "algorithm");

            var requestData = new {algorithm = algorithm, input = base64EncodedInput, signature = signature, hmac = hmac};

            var result = await MakeVaultApiRequest<Secret<dynamic>>(transitBackendMountPoint.Trim('/') + "/verify/" + keyName.Trim('/'), HttpMethod.Post, requestData, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task AppIdAuthenticationConfigureAppIdAsync(string appId, string policyValue, string displayName = null, string authenticationPath = AuthenticationBackendDefaultPaths.AppId)
        {
            Checker.NotNull(appId, "appId");
            Checker.NotNull(policyValue, "policyValue");

            var requestData = new { value = policyValue, display_name = displayName };
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/map/app-id/" + appId.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AppIdAuthenticationConfigureUserIdAsync(string userId, string appIdValue, string cidrBlock = null,
            string authenticationPath = AuthenticationBackendDefaultPaths.AppId)
        {
            Checker.NotNull(userId, "userId");
            Checker.NotNull(appIdValue, "appIdValue");

            var requestData = new Dictionary<string, object>
            {
                {"value", appIdValue},
            };

            if (!string.IsNullOrWhiteSpace(cidrBlock))
            {
                requestData.Add("cidr_block", cidrBlock);
            }

            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/map/user-id/" + userId.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> AppRoleAuthenticationGetRolesAsync()
        {
            // raja todo, this should be /roles i think.
            return await MakeVaultApiRequest<Secret<ListInfo>>("/auth/approle/role?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationConfigureClientAccessCredentialsAsync(AwsEc2AccessCredentials awsEc2AccessCredentials = null, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            if (awsEc2AccessCredentials == null)
            {
                awsEc2AccessCredentials = new AwsEc2AccessCredentials();
            }

            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/client", HttpMethod.Post, awsEc2AccessCredentials).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AwsEc2AccessCredentials>> AwsEc2AuthenticationGetClientAccessCredentialsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            return await MakeVaultApiRequest<Secret<AwsEc2AccessCredentials>>("auth/" + authenticationPath.Trim('/') + "/config/client", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationDeleteClientAccessCredentialsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/client", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationRegisterAwsPublicKeyAsync(AwsEc2PublicKeyInfo awsEc2PublicKeyInfo, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            Checker.NotNull(awsEc2PublicKeyInfo, "awsEc2PublicKeyInfo");
            Checker.NotNull(awsEc2PublicKeyInfo.CertificateName, "awsEc2PublicKeyInfo.CertificateName");

            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/certificate/" + awsEc2PublicKeyInfo.CertificateName, HttpMethod.Post, awsEc2PublicKeyInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AwsEc2PublicKeyInfo>> AwsEc2AuthenticationGetAwsPublicKeyAsync(string certificateName, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            Checker.NotNull(certificateName, "certificateName");
            return await MakeVaultApiRequest<Secret<AwsEc2PublicKeyInfo>>("auth/" + authenticationPath.Trim('/') + "/config/certificate/" + certificateName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> AwsEc2AuthenticationGetAwsPublicKeyListAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            return await MakeVaultApiRequest<Secret<ListInfo>>("auth/" + authenticationPath.Trim('/') + "/config/certificates?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationConfigureIdentityWhitelistTidyOptionsAsync(AwsEc2AuthenticationTidyOptions awsEc2AuthenticationTidyOptions, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/tidy/identity-whitelist", HttpMethod.Post, awsEc2AuthenticationTidyOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AwsEc2AuthenticationTidyOptions>> AwsEc2AuthenticationGetIdentityWhitelistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            return await MakeVaultApiRequest<Secret<AwsEc2AuthenticationTidyOptions>>("auth/" + authenticationPath.Trim('/') + "/config/tidy/identity-whitelist", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationDeleteIdentityWhitelistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/tidy/identity-whitelist", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationConfigureRoletagBlacklistTidyOptionsAsync(AwsEc2AuthenticationTidyOptions awsEc2AuthenticationTidyOptions, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/tidy/roletag-blacklist", HttpMethod.Post, awsEc2AuthenticationTidyOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AwsEc2AuthenticationTidyOptions>> AwsEc2AuthenticationGetRoletagBlacklistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            return await MakeVaultApiRequest<Secret<AwsEc2AuthenticationTidyOptions>>("auth/" + authenticationPath.Trim('/') + "/config/tidy/roletag-blacklist", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationDeleteRoletagBlacklistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/config/tidy/roletag-blacklist", HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AwsEc2AuthenticationRegisterRoleAsync(AwsEc2AuthenticationRole awsEc2AuthenticationRole, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2)
        {
            Checker.NotNull(awsEc2AuthenticationRole, "awsEc2AuthenticationRole");
            Checker.NotNull(awsEc2AuthenticationRole.RoleName, "awsEc2AuthenticationRole.RoleName");

            await MakeVaultApiRequest("auth/" + authenticationPath.Trim('/') + "/role/" + awsEc2AuthenticationRole.RoleName, HttpMethod.Post, awsEc2AuthenticationRole).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task EnableMultiFactorAuthenticationAsync(string supportedAuthenticationBackendMountPoint, string mfaType = "duo")
        {
            Checker.NotNull(supportedAuthenticationBackendMountPoint, "supportedAuthenticationBackendMountPoint");

            var requestData = new { type = mfaType };
            await MakeVaultApiRequest("auth/" + supportedAuthenticationBackendMountPoint.Trim('/') + "/mfa_config", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task WriteDuoAccessAsync(string supportedAuthenticationBackendMountPoint, string host, string integrationKey, string secretKey)
        {
            Checker.NotNull(supportedAuthenticationBackendMountPoint, "supportedAuthenticationBackendMountPoint");

            var requestData = new { host = host, ikey = integrationKey, skey = secretKey };
            await MakeVaultApiRequest("auth/" + supportedAuthenticationBackendMountPoint.Trim('/') + "/duo/access", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task WriteDuoConfigurationAsync(string supportedAuthenticationBackendMountPoint, string userAgent, string usernameFormat)
        {
            Checker.NotNull(supportedAuthenticationBackendMountPoint, "supportedAuthenticationBackendMountPoint");

            var requestData = new { user_agent = userAgent, username_format = usernameFormat };
            await MakeVaultApiRequest("auth/" + supportedAuthenticationBackendMountPoint.Trim('/') + "/duo/config", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path)
        {
            Checker.NotNull(path, "path");

            var value = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(path.Trim('/'), HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return value;
        }

        public async Task<Secret<Dictionary<string, object>>> WriteSecretAsync(string path, IDictionary<string, object> values)
        {
            Checker.NotNull(path, "path");

            return await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(path.Trim('/'), HttpMethod.Post, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path)
        {
            Checker.NotNull(path, "path");

            await MakeVaultApiRequest(path.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetTokenAccessorListAsync()
        {
            return await MakeVaultApiRequest<Secret<ListInfo>>("auth/token/accessors?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> CreateTokenAsync(TokenCreationOptions tokenCreationOptions = null)
        {
            if (tokenCreationOptions == null)
            {
                tokenCreationOptions = new TokenCreationOptions();
            }

            var action = tokenCreationOptions.CreateAsOrphan ? "create-orphan" : "create";

            if (!string.IsNullOrWhiteSpace(tokenCreationOptions.RoleName))
            {
                action = "create/" + tokenCreationOptions.RoleName.Trim('/');
            }

            return await MakeVaultApiRequest<Secret<Dictionary<string, object>>>("auth/token/" + action, HttpMethod.Post, tokenCreationOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenInfo>> GetTokenInfoAsync(string token)
        {
            Checker.NotNull(token, "token");

            var requestData = new { token = token };
            return await MakeVaultApiRequest<Secret<TokenInfo>>("auth/token/lookup", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenAccessorInfo>> GetTokenInfoByAccessorAsync(string tokenAccessor)
        {
            Checker.NotNull(tokenAccessor, "tokenAccessor");

            var requestData = new { accessor = tokenAccessor };
            return await MakeVaultApiRequest<Secret<TokenAccessorInfo>>("auth/token/lookup-accessor", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CallingTokenInfo>> GetCallingTokenInfoAsync()
        {
            return await MakeVaultApiRequest<Secret<CallingTokenInfo>>("auth/token/lookup-self", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RenewTokenAsync(string token = null, int? incrementSeconds = null)
        {
            // If the token given for renewal is null or is the same as the client token, 
            // the renew-self endpoint will be used in the API. Given that the default policy (by default) 
            // allows all clients access to the renew-self endpoint, this makes it much more likely that the 
            // intended operation will be successful. [GH-894]
            // https://github.com/hashicorp/vault/pull/894

            if (string.IsNullOrWhiteSpace(token) || string.Equals(token, (await _lazyVaultToken.Value), StringComparison.Ordinal))
            {
                await RenewCallingTokenAsync(incrementSeconds);
            }
            else
            {
                var requestData = new Dictionary<string, object>
                {
                    {"token", token}
                };

                if (incrementSeconds.HasValue)
                {
                    requestData.Add("increment", incrementSeconds.Value);
                }

                await
                    MakeVaultApiRequest("auth/token/renew", HttpMethod.Post, requestData)
                        .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            }
        }

        public async Task RenewCallingTokenAsync(int? incrementSeconds = null)
        {
            var requestData = incrementSeconds.HasValue ? new { increment = incrementSeconds.Value } : null;
            await MakeVaultApiRequest("auth/token/renew-self", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeTokenAsync(string token, bool revokeAllChildTokens)
        {
            Checker.NotNull(token, "token");

            var action = revokeAllChildTokens ? "revoke" : "revoke-orphan";
            var requestData = new { token = token };

            await MakeVaultApiRequest("auth/token/" + action, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeTokenByAccessorAsync(string tokenAccessor)
        {
            Checker.NotNull(tokenAccessor, "tokenAccessor");

            var requestData = new { accessor = tokenAccessor };
            await MakeVaultApiRequest("auth/token/revoke-accessor", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeCallingTokenAsync()
        {
            await MakeVaultApiRequest("auth/token/revoke-self", HttpMethod.Post).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenRoleInfo>> GetTokenRoleInfoAsync(string roleName)
        {
            Checker.NotNull(roleName, "roleName");

            return await MakeVaultApiRequest<Secret<TokenRoleInfo>>("auth/token/roles/" + roleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteTokenRoleAsync(string roleName)
        {
            Checker.NotNull(roleName, "roleName");

            await MakeVaultApiRequest("auth/token/roles/" + roleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetTokenRoleListAsync()
        {
            return await MakeVaultApiRequest<Secret<ListInfo>>("auth/token/roles?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task WriteTokenRoleInfoAsync(TokenRoleDefinition tokenRoleDefinition)
        {
            Checker.NotNull(tokenRoleDefinition, "tokenRoleDefinition");
            Checker.NotNull(tokenRoleDefinition.RoleName, "tokenRoleDefinition.RoleName");

            await MakeVaultApiRequest("auth/token/roles/" + tokenRoleDefinition.RoleName, HttpMethod.Post, tokenRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        private async Task MakeVaultApiRequest(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false)
        {
            await MakeVaultApiRequest<dynamic>(resourcePath, httpMethod, requestData, rawResponse);
        }

        private async Task<TResponse> MakeVaultApiRequest<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, Func<int, string, TResponse> customProcessor = null, string wrapTimeToLive = null) where TResponse : class
        {
            var headers = new Dictionary<string, string>();

            if (_lazyVaultToken != null)
            {
                headers.Add(VaultTokenHeaderKey, await _lazyVaultToken.Value);
            }

            if (wrapTimeToLive != null)
            {
                headers.Add(VaultWrapTimeToLiveHeaderKey, wrapTimeToLive);
            }

            return await _dataAccessManager.MakeRequestAsync<TResponse>(resourcePath, httpMethod, requestData, headers, rawResponse, customProcessor);
        }

        private static Secret<T2> GetMappedSecret<T1, T2>(Secret<T1> sourceSecret, T2 destinationData)
        {
            return new Secret<T2>
            {
                Data = destinationData,
                LeaseDurationSeconds = sourceSecret.LeaseDurationSeconds,
                RequestId = sourceSecret.RequestId,
                Warnings = sourceSecret.Warnings,
                LeaseId = sourceSecret.LeaseId,
                Renewable = sourceSecret.Renewable,
                AuthorizationInfo = sourceSecret.AuthorizationInfo,
                WrappedInformation = sourceSecret.WrappedInformation
            };
        }
    }
}