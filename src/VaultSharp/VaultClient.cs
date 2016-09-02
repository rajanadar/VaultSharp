using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Backends.Audit.Models;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Authentication.Providers;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.Secret.Models.AWS;
using VaultSharp.Backends.Secret.Models.Cassandra;
using VaultSharp.Backends.Secret.Models.Consul;
using VaultSharp.Backends.Secret.Models.MicrosoftSql;
using VaultSharp.Backends.Secret.Models.MySql;
using VaultSharp.Backends.Secret.Models.PKI;
using VaultSharp.Backends.Secret.Models.PostgreSql;
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

        private readonly IDataAccessManager _dataAccessManager;

        // leave it at instance level to avoid any garbage collection scenarios.
        private readonly IAuthenticationProvider _authenticationProvider;

        private readonly Lazy<Task<string>> _lazyVaultToken;

        private readonly bool _continueAsyncTasksOnCapturedContext;

        public VaultClient(Uri vaultServerUriWithPort, IAuthenticationInfo authenticationInfo, bool continueAsyncTasksOnCapturedContext = false, TimeSpan? serviceTimeout = null, IDataAccessManager dataAccessManager = null)
        {
            Checker.NotNull(vaultServerUriWithPort, "vaultServerUriWithPort");

            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;

            var vaultBaseAddress = new Uri(vaultServerUriWithPort, "v1/");

            // some operations can happen without the need to pass any authentication info. (unauthenticated endpoints)
            if (authenticationInfo != null)
            {
                _authenticationProvider = AuthenticationProviderFactory.CreateAuthenticationProvider(
                    authenticationInfo, vaultBaseAddress, serviceTimeout, continueAsyncTasksOnCapturedContext);

                _lazyVaultToken = new Lazy<Task<string>>(_authenticationProvider.GetTokenAsync);
            }

            _dataAccessManager = dataAccessManager ??
                                 new HttpDataAccessManager(vaultBaseAddress, serviceTimeout: serviceTimeout);
        }

        public async Task<bool> GetInitializationStatusAsync()
        {
            var response = await MakeVaultApiRequest<dynamic>("sys/init", HttpMethod.Get, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response.initialized;
        }

        public async Task<MasterCredentials> InitializeAsync(int secretShares, int secretThreshold, string[] pgpKeys = null)
        {
            var requestData = new { secret_shares = secretShares, secret_threshold = secretThreshold, pgp_keys = pgpKeys };

            var response = await MakeVaultApiRequest<MasterCredentials>("sys/init", HttpMethod.Put, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync()
        {
            var response = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/attempt", HttpMethod.Get, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(
            string base64EncodedOneTimePassword = null, string pgpKey = null)
        {
            var requestData = new { otp = base64EncodedOneTimePassword, pgpKey = pgpKey };

            var response = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/attempt", HttpMethod.Put, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task CancelRootTokenGenerationAsync()
        {
            await MakeVaultApiRequest("sys/generate-root/attempt", HttpMethod.Delete, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey,
            string nonce)
        {
            Checker.NotNull(masterShareKey, "masterShareKey");
            Checker.NotNull(nonce, "nonce");

            var requestData = new
            {
                key = masterShareKey,
                nonce = nonce
            };

            var progress = await MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/update", HttpMethod.Put, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return progress;
        }

        public async Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] allMasterShareKeys, string nonce)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");
            Checker.NotNull(nonce, "nonce");

            RootTokenGenerationStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                var requestData = new
                {
                    key = masterShareKey,
                    nonce = nonce
                };

                finalStatus =
                    await
                        MakeVaultApiRequest<RootTokenGenerationStatus>("sys/generate-root/update", HttpMethod.Put,
                            requestData, sendClientToken: false)
                            .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            }

            return finalStatus;
        }

        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await MakeVaultApiRequest<SealStatus>("sys/seal-status", HttpMethod.Get, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

            var response = await MakeVaultApiRequest<SealStatus>("sys/unseal", HttpMethod.Put, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys)
        {
            Checker.NotNull(allMasterShareKeys, "allMasterShareKeys");

            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                var requestData = new
                {
                    key = masterShareKey,
                    reset = false
                };

                finalStatus = await MakeVaultApiRequest<SealStatus>("sys/unseal", HttpMethod.Put, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

        public async Task UnmountSecretBackendAsync(string mountPoint)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}", mountPoint.Trim('/'));
            await MakeVaultApiRequest(resourcePath, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<MountConfiguration> GetMountedSecretBackendConfigurationAsync(string mountPoint)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var resourcePath = string.Format(CultureInfo.InvariantCulture, "sys/mounts/{0}/tune", mountPoint.Trim('/'));

            var response = await MakeVaultApiRequest<MountConfiguration>(resourcePath, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task TuneSecretBackendConfigurationAsync(string mountPoint, MountConfiguration mountConfiguration)
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
            Checker.NotNull(previousMountPoint, "previousMountPoint");
            Checker.NotNull(newMountPoint, "newMountPoint");

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

        public async Task TuneAuthenticationBackendConfigurationAsync(string authenticationPath, MountConfiguration mountConfiguration)
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


        public async Task<IEnumerable<string>> GetTokenCapabilitiesAsync(string token, string path)
        {
            Checker.NotNull(token, "token");
            Checker.NotNull(path, "path");

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
            Checker.NotNull(path, "path");

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
            Checker.NotNull(tokenAccessor, "tokenAccessor");
            Checker.NotNull(path, "path");

            var requestData = new { accessor = tokenAccessor, path = path };
            var response = await MakeVaultApiRequest<dynamic>("sys/capabilities-accessor", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null && response.capabilities != null)
            {
                return response.capabilities.ToObject<List<string>>();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<AuditBackend>> GetAllEnabledAuditBackendsAsync()
        {
            var response = await MakeVaultApiRequest<Dictionary<string, AuditBackend>>("sys/audit", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            if (response != null)
            {
                foreach (var kv in response)
                {
                    kv.Value.MountPoint = kv.Key;
                }

                return response.Values.AsEnumerable();
            }

            return Enumerable.Empty<AuditBackend>();
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
            Checker.NotNull(leaseId, "leaseId");

            var requestData = incrementSeconds.HasValue ? new { increment = incrementSeconds.Value } : null;

            var response = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>("sys/renew/" + leaseId, HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task RevokeSecretAsync(string leaseId)
        {
            Checker.NotNull(leaseId, "leaseId");

            await MakeVaultApiRequest("sys/revoke/" + leaseId, HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeAllSecretsUnderPrefixAsync(string pathPrefix)
        {
            Checker.NotNull(pathPrefix, "pathPrefix");

            await MakeVaultApiRequest("sys/revoke-prefix/" + pathPrefix.Trim('/'), HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task ForceRevokeAllSecretsUnderPrefixAsync(string pathPrefix)
        {
            Checker.NotNull(pathPrefix, "pathPrefix");

            await MakeVaultApiRequest("sys/revoke-force/" + pathPrefix.Trim('/'), HttpMethod.Put).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Leader> GetLeaderAsync()
        {
            var leader = await MakeVaultApiRequest<Leader>("sys/leader", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return leader;
        }

        public async Task StepDownActiveNodeAsync()
        {
            await
                MakeVaultApiRequest("sys/step-down", HttpMethod.Put)
                    .ConfigureAwait(_continueAsyncTasksOnCapturedContext);
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

        public async Task InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false)
        {
            var requestData = new { secret_shares = secretShares, secret_threshold = secretThreshold, pgp_keys = pgpKeys, backup = backup };
            await MakeVaultApiRequest("sys/rekey/init", HttpMethod.Put, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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
            Checker.NotNull(masterShareKey, "masterShareKey");
            Checker.NotNull(rekeyNonce, "rekeyNonce");

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
            Checker.NotNull(rekeyNonce, "rekeyNonce");

            RekeyProgress finalRekeyProgress = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                var requestData = new
                {
                    key = masterShareKey,
                    nonce = rekeyNonce
                };

                finalRekeyProgress =
                    await
                        MakeVaultApiRequest<RekeyProgress>("sys/rekey/update", HttpMethod.Put, requestData)
                            .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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
            Checker.NotNull(values, "values");

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
        // eventually, i need to solve this in a cleaner manner.
        public async Task<HealthStatus> GetHealthStatusAsync(bool? standbyOk = null, int? activeStatusCode = null, int? standbyStatusCode = null, int? sealedStatusCode = null, int? uninitializedStatusCode = null)
        {
            var failureHealthStatus = new HealthStatus();
            var expectedFailure = false;

            try
            {
                // raja todo.. add other querystrings.
                var queryString = standbyOk == true ? "?standbyok" : string.Empty;
                var resourcePath = "sys/health" + queryString;

                var healthStatus =
                    await MakeVaultApiRequest<HealthStatus>(resourcePath, HttpMethod.Get, sendClientToken: false,
                        failureDelegate: (statusCode, responseText) =>
                        {
                            // raja todo.. do the user defined error code equality as well.
                            if (statusCode == HttpStatusCode.InternalServerError
                                || statusCode == HttpStatusCode.NotImplemented
                                || statusCode == HttpStatusCode.ServiceUnavailable
                                || (int)statusCode == 429)
                            {
                                expectedFailure = true;
                                failureHealthStatus = JsonConvert.DeserializeObject<HealthStatus>(responseText);
                            }
                        }).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

                healthStatus.HealthCheckSucceeded = true;
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

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path)
        {
            Checker.NotNull(path, "path");

            var value = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(path.Trim('/'), HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return value;
        }

        public async Task WriteSecretAsync(string path, IDictionary<string, object> values)
        {
            Checker.NotNull(path, "path");

            await MakeVaultApiRequest(path.Trim('/'), HttpMethod.Post, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path)
        {
            Checker.NotNull(path, "path");

            await MakeVaultApiRequest(path.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<IEnumerable<string>>> GetTokenAccessorListAsync()
        {
            var response = await MakeVaultApiRequest<Secret<dynamic>>("auth/token/accessors?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            var result = new Secret<IEnumerable<string>>
            {
                AuthorizationInfo = response.AuthorizationInfo,
                Data = response.Data.keys.ToObject<List<string>>(),
                LeaseDurationSeconds = response.LeaseDurationSeconds,
                LeaseId = response.LeaseId,
                Renewable = response.Renewable,
                RequestId = response.RequestId,
                Warnings = response.Warnings
            };

            return result;
        }

        public async Task<Secret<Dictionary<string, object>>> CreateTokenAsync(TokenCreationOptions tokenCreationOptions = null)
        {
            var action = (tokenCreationOptions != null && tokenCreationOptions.CreateAsOrphan) ? "create-orphan" : "create";
            return await MakeVaultApiRequest<Secret<Dictionary<string, object>>>("auth/token/" + action, HttpMethod.Post, tokenCreationOptions).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenInfo>> GetCallingTokenInfoAsync()
        {
            return await MakeVaultApiRequest<Secret<TokenInfo>>("auth/token/lookup-self", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenInfo>> GetTokenInfoAsync(string token)
        {
            Checker.NotNull(token, "token");

            var requestData = new { token = token };
            return await MakeVaultApiRequest<Secret<TokenInfo>>("auth/token/lookup", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenInfo>> GetTokenInfoByAccessorAsync(string tokenAccessor)
        {
            Checker.NotNull(tokenAccessor, "tokenAccessor");

            var requestData = new { accessor = tokenAccessor };
            return await MakeVaultApiRequest<Secret<TokenInfo>>("auth/token/lookup-accessor", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RevokeTokenAsync(string token, bool revokeAllChildTokens)
        {
            Checker.NotNull(token, "token");

            var action = revokeAllChildTokens ? "revoke" : "revoke-orphan";
            await MakeVaultApiRequest("auth/token/" + action + "/" + token, HttpMethod.Post).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

        public async Task RenewCallingTokenAsync(int? incrementSeconds = null)
        {
            var requestData = incrementSeconds.HasValue ? new { increment = incrementSeconds.Value } : null;
            await MakeVaultApiRequest("auth/token/renew-self", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task RenewTokenAsync(string token, int? incrementSeconds = null)
        {
            Checker.NotNull(token, "token");

            // If the token given for renewal is the same as the client token, 
            // the renew-self endpoint will be used in the API. Given that the default policy (by default) 
            // allows all clients access to the renew-self endpoint, this makes it much more likely that the 
            // intended operation will be successful. [GH-894]
            // https://github.com/hashicorp/vault/pull/894

            var clientToken = await _lazyVaultToken.Value;

            if (string.Equals(token, clientToken, StringComparison.Ordinal))
            {
                await RenewCallingTokenAsync(incrementSeconds);
            }
            else
            {
                var requestData = incrementSeconds.HasValue ? new { increment = incrementSeconds.Value } : null;
                await
                    MakeVaultApiRequest("auth/token/renew/" + token, HttpMethod.Post, requestData)
                        .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            }
        }

        public async Task AWSConfigureRootCredentialsAsync(AWSRootCredentials awsRootCredentials, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRootCredentials, "awsRootCredentials");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/config/root", HttpMethod.Post, awsRootCredentials).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AWSConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(credentialLeaseSettings, "credentialLeaseSettings");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task AWSWriteNamedRoleAsync(string awsRoleName, AWSRoleDefinition awsRoleDefinition, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Post, awsRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSRoleDefinition>> AWSReadNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            var result =
                await
                    MakeVaultApiRequest<Secret<AWSRoleDefinition>>(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Get)
                        .ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

            return result;
        }

        public async Task AWSDeleteNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            await MakeVaultApiRequest(awsBackendMountPoint.Trim('/') + "/roles/" + awsRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AWSCredentials>> AWSGenerateDynamicCredentialsAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS)
        {
            Checker.NotNull(awsBackendMountPoint, "awsBackendMountPoint");
            Checker.NotNull(awsRoleName, "awsRoleName");

            var result = await MakeVaultApiRequest<Secret<AWSCredentials>>(awsBackendMountPoint.Trim('/') + "/creds/" + awsRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CassandraConfigureConnectionAsync(CassandraConnectionInfo cassandraConnectionInfo, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraConnectionInfo, "cassandraConnectionInfo");

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

        public async Task<Secret<CassandraRoleDefinition>> CassandraReadNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            var result = await MakeVaultApiRequest<Secret<CassandraRoleDefinition>>(cassandraBackendMountPoint.Trim('/') + "/roles/" + cassandraRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CassandraDeleteNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            await MakeVaultApiRequest(cassandraBackendMountPoint.Trim('/') + "/roles/" + cassandraRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> CassandraGenerateDynamicCredentialsAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra)
        {
            Checker.NotNull(cassandraBackendMountPoint, "cassandraBackendMountPoint");
            Checker.NotNull(cassandraRoleName, "cassandraRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(cassandraBackendMountPoint.Trim('/') + "/creds/" + cassandraRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task ConsulConfigureAccessAsync(ConsulAccessInfo consulAccessInfo, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulAccessInfo, "consulAccessInfo");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/config/access", HttpMethod.Post, consulAccessInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task ConsulWriteNamedRoleAsync(string consulRoleName, ConsulRoleDefinition consulRoleDefinition, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Post, consulRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulRoleDefinition>> ConsulReadNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            var result = await MakeVaultApiRequest<Secret<ConsulRoleDefinition>>(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task ConsulDeleteNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            await MakeVaultApiRequest(consulBackendMountPoint.Trim('/') + "/roles/" + consulRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulCredentials>> ConsulGenerateDynamicCredentialsAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul)
        {
            Checker.NotNull(consulBackendMountPoint, "consulBackendMountPoint");
            Checker.NotNull(consulRoleName, "consulRoleName");

            var result = await MakeVaultApiRequest<Secret<ConsulCredentials>>(consulBackendMountPoint.Trim('/') + "/creds/" + consulRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<Dictionary<string, object>>> CubbyholeReadSecretAsync(string locationPath)
        {
            Checker.NotNull(locationPath, "locationPath");

            var result = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(SecretBackendType.CubbyHole + "/" + locationPath.Trim('/'), HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task CubbyholeWriteSecretAsync(string locationPath, IDictionary<string, object> values)
        {
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(SecretBackendType.CubbyHole + "/" + locationPath.Trim('/'), HttpMethod.Post, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task CubbyholeDeleteSecretAsync(string locationPath)
        {
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(SecretBackendType.CubbyHole + "/" + locationPath.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> GenericReadSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            var result = await MakeVaultApiRequest<Secret<Dictionary<string, object>>>(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task GenericWriteSecretAsync(string locationPath, IDictionary<string, object> values, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Post, values).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task GenericDeleteSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic)
        {
            Checker.NotNull(genericBackendMountPoint, "genericBackendMountPoint");
            Checker.NotNull(locationPath, "locationPath");

            await MakeVaultApiRequest(genericBackendMountPoint.Trim('/') + "/" + locationPath.Trim('/'), HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlConfigureConnectionAsync(MicrosoftSqlConnectionInfo microsoftSqlConnectionInfo, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlConnectionInfo, "microsoftSqlConnectionInfo");
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, microsoftSqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(credentialLeaseSettings, "credentialLeaseSettings");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MicrosoftSqlWriteNamedRoleAsync(string microsoftSqlRoleName, MicrosoftSqlRoleDefinition microsoftSqlRoleDefinition, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Post, microsoftSqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MicrosoftSqlRoleDefinition>> MicrosoftSqlReadNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<MicrosoftSqlRoleDefinition>>(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ListInfo>> MicrosoftSqlReadAllRolesAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<ListInfo>>(microsoftSqlBackendMountPoint.Trim('/') + "/roles/?list=true", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MicrosoftSqlDeleteNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            await MakeVaultApiRequest(microsoftSqlBackendMountPoint.Trim('/') + "/roles/" + microsoftSqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> MicrosoftSqlGenerateDynamicCredentialsAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql)
        {
            Checker.NotNull(microsoftSqlBackendMountPoint, "microsoftSqlBackendMountPoint");
            Checker.NotNull(microsoftSqlRoleName, "microsoftSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(microsoftSqlBackendMountPoint.Trim('/') + "/creds/" + microsoftSqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MySqlConfigureConnectionAsync(MySqlConnectionInfo mySqlConnectionInfo, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlConnectionInfo, "mySqlConnectionInfo");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, mySqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MySqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(credentialLeaseSettings, "credentialLeaseSettings");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task MySqlWriteNamedRoleAsync(string mySqlRoleName, MySqlRoleDefinition mySqlRoleDefinition, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Post, mySqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<MySqlRoleDefinition>> MySqlReadNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            var result = await MakeVaultApiRequest<Secret<MySqlRoleDefinition>>(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task MySqlDeleteNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            await MakeVaultApiRequest(mySqlBackendMountPoint.Trim('/') + "/roles/" + mySqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> MySqlGenerateDynamicCredentialsAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql)
        {
            Checker.NotNull(mySqlBackendMountPoint, "mySqlBackendMountPoint");
            Checker.NotNull(mySqlRoleName, "mySqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(mySqlBackendMountPoint.Trim('/') + "/creds/" + mySqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<RawCertificateData> PKIReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var format = certificateFormat == CertificateFormat.pem ? "/" + CertificateFormat.pem : string.Empty;
            var outputFormat = certificateFormat == CertificateFormat.pem
                ? CertificateFormat.pem
                : CertificateFormat.der;

            var result = await MakeVaultApiRequest<string>(pkiBackendMountPoint.Trim('/') + "/ca" + format, HttpMethod.Get, sendClientToken: false, rawResponse: true).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

            var result = await MakeVaultApiRequest<Secret<RawCertificateData>>(pkiBackendMountPoint.Trim('/') + "/cert/" + predicate, HttpMethod.Get, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            result.Data.EncodedCertificateFormat = CertificateFormat.pem;

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

            var result = await MakeVaultApiRequest<string>(pkiBackendMountPoint.Trim('/') + "/crl" + format, HttpMethod.Get, sendClientToken: false, rawResponse: true).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return new RawCertificateData
            {
                CertificateContent = result,
                EncodedCertificateFormat = outputFormat
            };
        }

        public async Task<bool> PKIRotateCRLAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI)
        {
            Checker.NotNull(pkiBackendMountPoint, "pkiBackendMountPoint");

            var result = await MakeVaultApiRequest<Secret<dynamic>>(pkiBackendMountPoint.Trim('/') + "/crl/rotate", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result.Data.success;
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

        public async Task PostgreSqlConfigureConnectionAsync(PostgreSqlConnectionInfo postgreSqlConnectionInfo, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlConnectionInfo, "postgreSqlConnectionInfo");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/config/connection", HttpMethod.Post, postgreSqlConnectionInfo).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task PostgreSqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(credentialLeaseSettings, "credentialLeaseSettings");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/config/lease", HttpMethod.Post, credentialLeaseSettings).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task PostgreSqlWriteNamedRoleAsync(string postgreSqlRoleName, PostgreSqlRoleDefinition postgreSqlRoleDefinition, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Post, postgreSqlRoleDefinition).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<PostgreSqlRoleDefinition>> PostgreSqlReadNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<PostgreSqlRoleDefinition>>(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task PostgreSqlDeleteNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            await MakeVaultApiRequest(postgreSqlBackendMountPoint.Trim('/') + "/roles/" + postgreSqlRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> PostgreSqlGenerateDynamicCredentialsAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql)
        {
            Checker.NotNull(postgreSqlBackendMountPoint, "postgreSqlBackendMountPoint");
            Checker.NotNull(postgreSqlRoleName, "postgreSqlRoleName");

            var result = await MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(postgreSqlBackendMountPoint.Trim('/') + "/creds/" + postgreSqlRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

        public async Task<Secret<SSHRoleDefinition>> SSHReadNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");

            var result = await MakeVaultApiRequest<Secret<SSHRoleDefinition>>(sshBackendMountPoint.Trim('/') + "/roles/" + sshRoleName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task SSHDeleteNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");

            await MakeVaultApiRequest(sshBackendMountPoint.Trim('/') + "/roles/" + sshRoleName, HttpMethod.Delete).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHCredentials>> SSHGenerateDynamicCredentialsAsync(string sshRoleName, string ipAddress, string username = null, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(sshRoleName, "sshRoleName");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { username = username, ip = ipAddress };
            return await MakeVaultApiRequest<Secret<SSHCredentials>>(sshBackendMountPoint.Trim('/') + "/creds/" + sshRoleName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHRoleData>> SSHLookupRolesAsync(string ipAddress, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(ipAddress, "ipAddress");

            var requestData = new { ip = ipAddress };
            return await MakeVaultApiRequest<Secret<SSHRoleData>>(sshBackendMountPoint.Trim('/') + "/lookup", HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SSHOTPVerificationData>> SSHVerifyOTPAsync(string otp, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH)
        {
            Checker.NotNull(sshBackendMountPoint, "sshBackendMountPoint");
            Checker.NotNull(otp, "otp");

            var requestData = new { otp = otp };

            var response = await MakeVaultApiRequest<Secret<SSHOTPVerificationData>>(sshBackendMountPoint.Trim('/') + "/verify", HttpMethod.Post, requestData, sendClientToken: false).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task TransitCreateEncryptionKeyAsync(string encryptionKeyName, bool mustUseKeyDerivation = false, bool doConvergentEncryption = false, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { derived = mustUseKeyDerivation, convergent_encryption = doConvergentEncryption };
            await MakeVaultApiRequest(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TransitEncryptionKeyInfo>> TransitGetEncryptionKeyInfoAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var result = await MakeVaultApiRequest<Secret<TransitEncryptionKeyInfo>>(transitBackendMountPoint.Trim('/') + "/keys/" + encryptionKeyName, HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

        public async Task<Secret<CipherTextData>> TransitEncryptAsync(string encryptionKeyName, string base64EncodedPlainText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { plaintext = base64EncodedPlainText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<CipherTextData>>(transitBackendMountPoint.Trim('/') + "/encrypt/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<PlainTextData>> TransitDecryptAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { ciphertext = cipherText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<PlainTextData>>(transitBackendMountPoint.Trim('/') + "/decrypt/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<CipherTextData>> TransitRewrapWithLatestEncryptionKeyAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var requestData = new { ciphertext = cipherText, context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce };

            var result = await MakeVaultApiRequest<Secret<CipherTextData>>(transitBackendMountPoint.Trim('/') + "/rewrap/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<TransitKeyData>> TransitCreateDataKeyAsync(string encryptionKeyName, bool returnKeyAsPlainText = false, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, int keyBits = 256, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit)
        {
            Checker.NotNull(transitBackendMountPoint, "transitBackendMountPoint");
            Checker.NotNull(encryptionKeyName, "encryptionKeyName");

            var plainorWrapped = returnKeyAsPlainText ? "plaintext" : "wrapped";
            var requestData = new { context = base64EncodedKeyDerivationContext, nonce = convergentEncryptionBase64EncodedNonce, bits = keyBits };

            var result = await MakeVaultApiRequest<Secret<TransitKeyData>>(transitBackendMountPoint.Trim('/') + "/datakey/" + plainorWrapped + "/" + encryptionKeyName, HttpMethod.Post, requestData).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return result;
        }

        private async Task MakeVaultApiRequest(string resourcePath, HttpMethod httpMethod, object requestData = null, bool sendClientToken = true, bool rawResponse = false, Action<HttpStatusCode, string> failureDelegate = null)
        {
            await MakeVaultApiRequest<dynamic>(resourcePath, httpMethod, requestData, sendClientToken, rawResponse, failureDelegate);
        }

        private async Task<TResponse> MakeVaultApiRequest<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, bool sendClientToken = true, bool rawResponse = false, Action<HttpStatusCode, string> failureDelegate = null) where TResponse : class
        {
            if (sendClientToken && _lazyVaultToken == null)
            {
                // a secure API was invoked, but no auth info was provided.
                throw new InvalidOperationException("This API is a secure API and needs a client token. So please initialize Vault Client with AuthenticationInfo.");
            }

            var headers = sendClientToken ? new Dictionary<string, string> { { VaultTokenHeaderKey, await _lazyVaultToken.Value } } : null;
            return await _dataAccessManager.MakeRequestAsync<TResponse>(resourcePath, httpMethod, requestData, headers, rawResponse, failureDelegate);
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