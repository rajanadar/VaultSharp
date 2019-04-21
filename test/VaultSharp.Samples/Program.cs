using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.Transit;
using VaultSharp.V1.SystemBackend;
using VaultSharp.V1.SystemBackend.Plugin;
using Xunit;

namespace VaultSharp.Samples
{
    class Program
    {
        private const string ExpectedVaultVersion = "0.11.0";

        private static IVaultClient _unauthenticatedVaultClient;
        private static IVaultClient _authenticatedVaultClient;

        private static string _responseContent;

        public static void Main(string[] args)
        {
            const string path = "ProgramOutput.txt";

            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    Console.WriteLine();
                    Console.Write("Writing results to file. Hang tight...");

                    var existingOut = Console.Out;
                    Console.SetOut(sw);

                    var settings = GetVaultClientSettings();
                    _unauthenticatedVaultClient = new VaultClient(settings);

                    RunAllSamples();

                    Console.SetOut(existingOut);

                    Console.WriteLine();
                    Console.Write("I think we are done here. Press any key to exit...");
                }
            }

            Console.ReadLine();
        }

        private static void RunAllSamples()
        {
            // before runnig these tests, just start your local vault server with a file backend.
            
            // startvault.cmd OR these 2 lines.
            // rd E:\raja\work\vault\file_backend /S /Q
            // vault server -config E:\raja\work\vault\f.hcl
            
            // f.hcl looks like
            /*
                backend "file" {
                  path = "e:\\raja\\work\\vault\\file_backend"
                  }
 
                listener "tcp" {
                  address = "127.0.0.1:8200"
                  tls_disable = 1
                }

                raw_storage_endpoint = true
            */

            // don't init or unseal it. these tests will do all of that.
            // i dev on a Windows 10 x64 bit OS.

            RunSystemBackendSamples();
            RunAuthMethodSamples();
            RunSecretsEngineSamples();
        }

        private static void RunAuthMethodSamples()
        {
            // Token Apis.
            var callingTokenInfo = _authenticatedVaultClient.V1.Auth.Token.LookupSelfAsync().Result;
            DisplayJson(callingTokenInfo);

            // use a renewable token here.
            // var renewInfo = _authenticatedVaultClient.V1.Auth.Token.RenewSelfAsync().Result;
            // DisplayJson(renewInfo);

            // _authenticatedVaultClient.V1.Auth.Token.RevokeSelfAsync().Wait();

            // Needs Manual pre-steps.
            // Startup vault with normal dev mode. not real.
            /*
                .\vault.exe auth enable approle
                .\vault.exe write auth/approle/role/my-role secret_id_ttl=10m  token_num_uses=10  token_ttl=20m   token_max_ttl=30m  secret_id_num_uses=40
                .\vault.exe read auth/approle/role/my-role/role-id
                << note roleid >>
                .\vault.exe write -f auth/approle/role/my-role/secret-id
                << .\vault.exe secret id >>

                .]vault.exe write auth/approle/login role_id=335277eb-932a-71ef-825d-d403a1663f0d  secret_id=5ed1d413-4d06-1604-8db3-9000b4bd9204
             */

            string appRoleId = "71fce15a-3edb-4263-9227-9d623f1f9a22";
            string secretId = "df1326a5-07f2-814c-4f83-9a00c99694d6";

            IAuthMethodInfo appRoleAuthMethodInfo = new AppRoleAuthMethodInfo(appRoleId, secretId);

            VaultClientSettings authVaultClientSettings = GetVaultClientSettings(appRoleAuthMethodInfo);

            var token = "";
            authVaultClientSettings.BeforeApiRequestAction = (c, r) =>
            {
                if (r.Headers.Contains("X-Vault-Token"))
                {
                    token = r.Headers.Single(h => h.Key == "X-Vault-Token").Value.Single();
                }
            };

            // IVaultClient vaultClient = new VaultClient(authVaultClientSettings);

            // var result = vaultClient.V1.System.GetCallingTokenCapabilitiesAsync("v1/sys").Result;
            // Assert.True(result.Data.Capabilities.Any());

            // token
            // IAuthMethodInfo tokenAuthMethodInfo = new TokenAuthMethodInfo(token); // throws null

            // authVaultClientSettings = GetVaultClientSettings(tokenAuthMethodInfo);
            // vaultClient = new VaultClient(authVaultClientSettings);

            // result = vaultClient.V1.System.GetCallingTokenCapabilitiesAsync("v1/sys").Result;
            // Assert.True(result.Data.Capabilities.Any());
        }

        private static void RunSecretsEngineSamples()
        {
            RunCubbyholeSamples();
            RunKeyValueSamples();

            // RunTransitSamples();
        }

        private static void RunCubbyholeSamples()
        {
            var secretPath = "a/test-path";

            var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath, value).Wait();

            var read1 = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath).Result;
            Assert.True(read1.Data.Count == 2);

            value.Add("key3", true);
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath, value).Wait();

            var read2 = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath).Result;
            Assert.True(read2.Data.Count == 3);

            var value2 = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath + "2", value2).Wait();

            var paths = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretPathsAsync("a/").Result;
            Assert.True(paths.Data.Keys.Count() == 2);

            _authenticatedVaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretPath).Wait();
            Assert.ThrowsAsync<VaultApiException>(() => _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath)).Wait();

            _authenticatedVaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretPath + "2").Wait();
        }

        private static void RunTransitSamples()
        {
            // Transit

            // manually setup the following.

            var keyName = "test_key";

            // .\vault.exe secrets enable transit
            // .\vault.exe write -f transit/keys/test_key

            var context = "context1";
            var plainText = "raja";
            var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            var encodedContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(context));

            var nonce = Convert.ToBase64String(Enumerable.Range(0, 12).Select(i => (byte)i).ToArray());
            var nonce2 = Convert.ToBase64String(Enumerable.Range(0, 12).Select(i => (byte)(i + 1)).ToArray());

            var encryptOptions = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedPlainText,
                Base64EncodedContext = encodedContext,
                ConvergentEncryption = true,
                Nonce = nonce
            };

            var encryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions).Result;
            var cipherText = encryptionResponse.Data.CipherText;

            encryptOptions = new EncryptRequestOptions
            {
                BatchedEncryptionItems = new List<EncryptionItem>
                {
                    new EncryptionItem { Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                }
            };

            var batchedEncryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions).Result;
            var firstCipherText = batchedEncryptionResponse.Data.BatchedResults.First().CipherText;

            var decryptOptions = new DecryptRequestOptions
            {
                CipherText = cipherText,
                Base64EncodedContext = encodedContext,
                Nonce = nonce
            };

            var decryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions).Result;
            var gotPlainText = decryptionResponse.Data.Base64EncodedPlainText;

            decryptOptions = new DecryptRequestOptions
            {
                BatchedDecryptionItems = new List<DecryptionItem>
                {
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(0).CipherText },
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(1).CipherText },
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(2).CipherText },
                }
            };

            var batchedDecryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions).Result;
            var firstPlainText = batchedDecryptionResponse.Data.BatchedResults.First().Base64EncodedPlainText;
        }

        private static void RunKeyValueSamples()
        {
            var path = "blah";

            var values = new Dictionary<string, object>
            {
                {"foo", "bar"},
                {"foo2", 345 }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(path, values).Wait();

            var paths1 = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("").Result;
            Assert.True(paths1.Data.Keys.Count() == 1);

            var kv1Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(path).Result;
            Assert.True(kv1Secret.Data.Count == 2);

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(path).Wait();

            // mount a new v2 kv
            var kv2SecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.KeyValue,
                Config = new Dictionary<string, string>
                {
                    {  "version", "2" }
                },
                Path = "kv",
            };

            values["foo"] = "kv2";

            // Manually set a kv mount with version 2. The programmatic mount doesn't seem to setting up v2.
            return;

            // .\vault.exe secrets enable -version=2 kv            

            // _authenticatedVaultClient.V1.System.MountSecretBackendAsync(kv2SecretsEngine).Wait();

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, values, mountPoint: kv2SecretsEngine.Path).Wait();

            var kv2Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(kv2Secret.Data.Data.Count == 2);

            var paths2 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("", mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(paths2.Data.Keys.Count() == 1);

            var kv2metadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(kv2metadata.Data.CurrentVersion > 0); 

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.DestroySecretAsync(path, new List<int> { kv2metadata.Data.CurrentVersion }, mountPoint: kv2SecretsEngine.Path).Wait();

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(kv2SecretsEngine.Path).Wait();         
        }

        private static void RunSystemBackendSamples()
        {
            var exception = Assert.ThrowsAsync<VaultApiException>(() => _unauthenticatedVaultClient.V1.System.GetSealStatusAsync()).Result;
            Assert.Contains("not yet initialized", exception.Message);
            Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Contains("not yet initialized", exception.ApiErrors.First());

            // init
            var initStatus = _unauthenticatedVaultClient.V1.System.GetInitStatusAsync().Result;
            Assert.False(initStatus);

            // pre-init health checks.

            var health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync().Result;
            DisplayJson(health);

            Assert.Equal(ExpectedVaultVersion, health.Version);

            Assert.False(health.Initialized);
            Assert.True(health.Sealed);
            Assert.Equal((int)HttpStatusCode.NotImplemented, health.HttpStatusCode);

            // do just one head check.
            var headException = Assert.ThrowsAsync<VaultApiException>(() => _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(queryHttpMethod: HttpMethod.Head)).Result;
            Assert.Equal((int)HttpStatusCode.NotImplemented, headException.StatusCode);

            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(uninitializedStatusCode: 300).Result;
            DisplayJson(health);
            Assert.False(health.Initialized);
            Assert.Equal(300, health.HttpStatusCode);

            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(uninitializedStatusCode: 200).Result;
            DisplayJson(health);
            Assert.False(health.Initialized);
            Assert.Equal((int)HttpStatusCode.OK, health.HttpStatusCode);

            // do the init

            var initOptions = new InitOptions
            {
                SecretShares = 10,
                SecretThreshold = 5
            };

            var masterCredentials = _unauthenticatedVaultClient.V1.System.InitAsync(initOptions).Result;
            DisplayJson(masterCredentials);

            Assert.Equal(initOptions.SecretShares, masterCredentials.MasterKeys.Length);
            Assert.Equal(initOptions.SecretShares, masterCredentials.Base64MasterKeys.Length);

            initStatus = _unauthenticatedVaultClient.V1.System.GetInitStatusAsync().Result;
            DisplayJson(initStatus);

            Assert.True(initStatus);

            // health check for initialized but sealed vault.
            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync().Result;
            DisplayJson(health);
            Assert.True(health.Initialized);
            Assert.True(health.Sealed);
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, health.HttpStatusCode);

            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(sealedStatusCode: 404).Result;
            DisplayJson(health);
            Assert.True(health.Initialized);
            Assert.True(health.Sealed);
            Assert.Equal((int)HttpStatusCode.NotFound, health.HttpStatusCode);

            // unseal

            var sealStatus = _unauthenticatedVaultClient.V1.System.GetSealStatusAsync().Result;
            DisplayJson(sealStatus);
            Assert.True(sealStatus.Sealed);

            var threshold = 0;
            var reset = false;

            foreach (var masterKey in masterCredentials.MasterKeys)
            {
                ++threshold;
                var unsealStatus = _unauthenticatedVaultClient.V1.System.UnsealAsync(masterKey).Result;

                DisplayJson(unsealStatus);

                if (threshold < initOptions.SecretThreshold)
                {
                    Assert.Equal(threshold, unsealStatus.Progress);
                    Assert.True(unsealStatus.Sealed);

                    // unseal with reset now.

                    if (!reset && (threshold == initOptions.SecretThreshold - 2))
                    {
                        unsealStatus = _unauthenticatedVaultClient.V1.System.UnsealAsync(masterKey, true).Result;

                        Assert.Equal(0, unsealStatus.Progress);
                        Assert.True(unsealStatus.Sealed);

                        threshold = 0;
                        reset = true;
                    }
                }
                else
                {
                    Assert.Equal(0, unsealStatus.Progress);
                    Assert.False(unsealStatus.Sealed);
                }
            }

            // health check for unsealed and active.
            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync().Result;
            DisplayJson(health);
            Assert.True(health.Initialized);
            Assert.False(health.Sealed);
            Assert.Equal((int)HttpStatusCode.OK, health.HttpStatusCode);

            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(activeStatusCode: 405).Result;
            DisplayJson(health);
            Assert.True(health.Initialized);
            Assert.False(health.Sealed);
            Assert.Equal((int)HttpStatusCode.MethodNotAllowed, health.HttpStatusCode);

            // seal it

            var authSettings = GetVaultClientSettings(new TokenAuthMethodInfo(masterCredentials.RootToken));
            _authenticatedVaultClient = new VaultClient(authSettings);

            _authenticatedVaultClient.V1.System.SealAsync().Wait();
            sealStatus = _unauthenticatedVaultClient.V1.System.GetSealStatusAsync().Result;
            DisplayJson(sealStatus);
            Assert.True(sealStatus.Sealed);

            // quick unseal
            sealStatus = _unauthenticatedVaultClient.V1.System.QuickUnsealAsync(masterCredentials.MasterKeys).Result;
            DisplayJson(sealStatus);
            Assert.False(sealStatus.Sealed);

            // audit backends
            var audits = _authenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
            DisplayJson(audits);
            Assert.False(audits.Data.Any());

            // enable new file audit
            var newFileAudit = new FileAuditBackend
            {
                Description = "store logs in a file - test cases",
                Options = new FileAuditBackendOptions
                {
                    FilePath = "/var/log/file",
                    LogSensitiveDataInRawFormat = true.ToString().ToLowerInvariant(),
                    HmacAccessor = false.ToString().ToLowerInvariant(),
                    Format = "jsonx"
                }
            };

            _authenticatedVaultClient.V1.System.MountAuditBackendAsync(newFileAudit).Wait();

            var newFileAudit2 = new FileAuditBackend
            {
                MountPoint = "file2/test",
                Description = "2 store logs in a file - test cases",
                Options = new FileAuditBackendOptions
                {
                    FilePath = "/var/log/file2",
                    LogSensitiveDataInRawFormat = true.ToString().ToLowerInvariant(),
                    HmacAccessor = false.ToString().ToLowerInvariant(),
                    Format = "jsonx"
                }
            };

            _authenticatedVaultClient.V1.System.MountAuditBackendAsync(newFileAudit2).Wait();

            // get audits
            var newAudits = _authenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
            DisplayJson(newAudits);
            Assert.Equal(audits.Data.Count() + 2, newAudits.Data.Count());

            // hash with audit
            var hash = _authenticatedVaultClient.V1.System.AuditHashAsync(newFileAudit.MountPoint, "testinput").Result;
            DisplayJson(hash);
            Assert.NotNull(hash.Data.Hash);

            // disabled audit
            _authenticatedVaultClient.V1.System.UnmountAuditBackendAsync(newFileAudit.MountPoint).Wait();
            _authenticatedVaultClient.V1.System.UnmountAuditBackendAsync(newFileAudit2.MountPoint).Wait();

            // get audits
            var oldAudits = _authenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
            Assert.Equal(audits.Data.Count(), oldAudits.Data.Count());

            // syslog is not supported on windows. so no acceptance tests possible on my machine.
            // this being a netstandard compliant library, some non-ironic folks can write tests for non-windows boxes and comment it out.

            // auth backend mounting and tuning.

            // get Authentication backends
            var authBackends = _authenticatedVaultClient.V1.System.GetAuthBackendsAsync().Result;
            DisplayJson(authBackends);
            Assert.True(authBackends.Data.Any());  // default mounted

            var backendConfig = _authenticatedVaultClient.V1.System.GetAuthBackendConfigAsync(authBackends.Data.First().Value.Path).Result;
            DisplayJson(backendConfig);
            Assert.NotNull(backendConfig);

            // enable new auth
            var newAuth = new AuthMethod
            {
                Path = "github1",
                Type = AuthMethodType.GitHub,
                Description = "Github auth - test cases"
            };

            _authenticatedVaultClient.V1.System.MountAuthBackendAsync(newAuth).Wait();

            backendConfig = _authenticatedVaultClient.V1.System.GetAuthBackendConfigAsync(newAuth.Path).Result;
            DisplayJson(backendConfig);
            Assert.Equal(2764800, backendConfig.Data.DefaultLeaseTtl);
            Assert.Equal(2764800, backendConfig.Data.MaximumLeaseTtl);

            var newBackendConfig = new BackendConfig
            {
                DefaultLeaseTtl = 3600,
                MaximumLeaseTtl = 4200,
                ForceNoCache = true
            };

            _authenticatedVaultClient.V1.System.ConfigureAuthBackendAsync(newAuth.Path, newBackendConfig).Wait();

            backendConfig = _authenticatedVaultClient.V1.System.GetAuthBackendConfigAsync(newAuth.Path).Result;
            DisplayJson(backendConfig);
            Assert.Equal(newBackendConfig.DefaultLeaseTtl, backendConfig.Data.DefaultLeaseTtl);
            Assert.Equal(newBackendConfig.MaximumLeaseTtl, backendConfig.Data.MaximumLeaseTtl);

            // raja todo: this is not heeded by vault. look into it.
            // Assert.Equal(newBackendConfig.ForceNoCache, backendConfig.Data.ForceNoCache);

            // get all auths
            var newAuthBackends = _authenticatedVaultClient.V1.System.GetAuthBackendsAsync().Result;
            DisplayJson(newAuthBackends);
            Assert.Equal(authBackends.Data.Count() + 1, newAuthBackends.Data.Count());

            // disable auth
            _authenticatedVaultClient.V1.System.UnmountAuthBackendAsync(newAuth.Path).Wait();

            // get all auths
            var oldAuthBackends = _authenticatedVaultClient.V1.System.GetAuthBackendsAsync().Result;
            DisplayJson(oldAuthBackends);
            Assert.Equal(authBackends.Data.Count(), oldAuthBackends.Data.Count());

            // capabilities
            var caps = _authenticatedVaultClient.V1.System.GetTokenCapabilitiesAsync("v1/sys", masterCredentials.RootToken).Result;
            DisplayJson(caps);
            Assert.True(caps.Data.Capabilities.Any());

            // var accessCaps = _authenticatedVaultClient.V1.System.GetTokenCapabilitiesByAcessorAsync("v1/sys", "raja todo").Result;
            // DisplayJson(accessCaps);
            // Assert.True(accessCaps.Data.Capabilities.Any());

            var callingCaps = _authenticatedVaultClient.V1.System.GetCallingTokenCapabilitiesAsync("v1/sys").Result;
            DisplayJson(callingCaps);
            Assert.True(callingCaps.Data.Capabilities.Any());

            // audit headers
            var reqHeaders = _authenticatedVaultClient.V1.System.GetAuditRequestHeadersAsync().Result;
            DisplayJson(reqHeaders);

            string headerValue = "X-Forwarded-For";
            string headerValue2 = "X-RequestId";

            _authenticatedVaultClient.V1.System.PutAuditRequestHeaderAsync(headerValue, true).Wait();
            _authenticatedVaultClient.V1.System.PutAuditRequestHeaderAsync(headerValue2).Wait();

            var newReqHeaders = _authenticatedVaultClient.V1.System.GetAuditRequestHeadersAsync().Result;
            DisplayJson(newReqHeaders);
            Assert.Equal(reqHeaders.Data.Headers.Count + 2, newReqHeaders.Data.Headers.Count);

            // needs to be lowercase for now. there is a bug in Vault.
            // https://github.com/hashicorp/vault/issues/3701
            var header = _authenticatedVaultClient.V1.System.GetAuditRequestHeaderAsync(headerValue.ToLowerInvariant()).Result;
            DisplayJson(header);
            Assert.True(header.Data.HMAC);

            _authenticatedVaultClient.V1.System.DeleteAuditRequestHeaderAsync(headerValue).Wait();
            _authenticatedVaultClient.V1.System.DeleteAuditRequestHeaderAsync(headerValue2).Wait();

            reqHeaders = _authenticatedVaultClient.V1.System.GetAuditRequestHeadersAsync().Result;
            Assert.False(reqHeaders.Data.Headers.Any());

            // control group config. Enterprise only.
            // https://github.com/hashicorp/vault/issues/3702
            /*
            var cgconfig = _authenticatedVaultClient.V1.System.Enterprise.GetControlGroupConfigAsync().Result;
            DisplayJson(cgconfig);

            _authenticatedVaultClient.V1.System.Enterprise.ConfigureControlGroupAsync("4h").Wait();

            cgconfig = _authenticatedVaultClient.V1.System.Enterprise.GetControlGroupConfigAsync().Result;
            DisplayJson(cgconfig);
            Assert.Equal("4h", cgconfig.Data.MaxTimeToLive);

            _authenticatedVaultClient.V1.System.Enterprise.DeleteControlGroupConfigAsync().Wait();
            */

            // cors config

            var corsConfig = _authenticatedVaultClient.V1.System.GetCORSConfigAsync().Result;
            DisplayJson(corsConfig);
            Assert.False(corsConfig.Data.Enabled);

            var newCorsConfig = new CORSConfig
            {
                Enabled = true,
                AllowedHeaders = new List<string>
                {
                    "header1",
                    "header2"
                },
                AllowedOrigins = new List<string>
                {
                    "https://origin1",
                    "https://origin2"
                }
            };

            _authenticatedVaultClient.V1.System.ConfigureCORSAsync(newCorsConfig).Wait();

            corsConfig = _authenticatedVaultClient.V1.System.GetCORSConfigAsync().Result;
            DisplayJson(corsConfig);
            Assert.True(corsConfig.Data.Enabled);
            Assert.Contains("header1", corsConfig.Data.AllowedHeaders);

            _authenticatedVaultClient.V1.System.DeleteCORSConfigAsync().Wait();
            corsConfig = _authenticatedVaultClient.V1.System.GetCORSConfigAsync().Result;
            DisplayJson(corsConfig);
            Assert.False(corsConfig.Data.Enabled);

            // control group config.
            // only enterpise vault.

            /*
            var cgTokenAccessor = "0ad21b78-e9bb-64fa-88b8-1e38db217bde";

            var cgStatus = _authenticatedVaultClient.V1.System.Enterprise.CheckControlGroupStatusAsync(cgTokenAccessor).Result;
            DisplayJson(cgStatus);
            Assert.False(cgStatus.Data.Approved);

            cgStatus = _authenticatedVaultClient.V1.System.Enterprise.AuthorizeControlGroupAsync(cgTokenAccessor).Result;
            DisplayJson(cgStatus);
            Assert.False(cgStatus.Data.Approved);

            cgStatus = _authenticatedVaultClient.V1.System.Enterprise.CheckControlGroupStatusAsync(cgTokenAccessor).Result;
            DisplayJson(cgStatus);
            Assert.True(cgStatus.Data.Approved);
            */

            // root token generation
            var rootStatus = _unauthenticatedVaultClient.V1.System.GetRootTokenGenerationStatusAsync().Result;
            DisplayJson(rootStatus);
            Assert.False(rootStatus.Started);

            var otp = Convert.ToBase64String(Enumerable.Range(0, 16).Select(i => (byte)i).ToArray());
            rootStatus = _unauthenticatedVaultClient.V1.System.InitiateRootTokenGenerationAsync(otp, null).Result;
            DisplayJson(rootStatus);
            Assert.True(rootStatus.Started);
            Assert.NotNull(rootStatus.Nonce);

            foreach (var masterKey in masterCredentials.MasterKeys)
            {
                rootStatus = _unauthenticatedVaultClient.V1.System.ContinueRootTokenGenerationAsync(masterKey, rootStatus.Nonce).Result;
                DisplayJson(rootStatus);

                if (rootStatus.Complete)
                {
                    break;
                }
            }

            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);

            rootStatus = _unauthenticatedVaultClient.V1.System.InitiateRootTokenGenerationAsync(otp, null).Result;
            DisplayJson(rootStatus);

            rootStatus = _unauthenticatedVaultClient.V1.System.ContinueRootTokenGenerationAsync(masterCredentials.MasterKeys[0], rootStatus.Nonce).Result;
            DisplayJson(rootStatus);
            Assert.True(rootStatus.Started);

            _unauthenticatedVaultClient.V1.System.CancelRootTokenGenerationAsync().Wait();

            rootStatus = _unauthenticatedVaultClient.V1.System.GetRootTokenGenerationStatusAsync().Result;
            DisplayJson(rootStatus);
            Assert.False(rootStatus.Started);

            rootStatus = _unauthenticatedVaultClient.V1.System.InitiateRootTokenGenerationAsync(otp, null).Result;
            DisplayJson(rootStatus);

            rootStatus = _unauthenticatedVaultClient.V1.System.QuickRootTokenGenerationAsync(masterCredentials.MasterKeys, rootStatus.Nonce).Result;
            DisplayJson(rootStatus);
            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);

            // get encryption key status

            var keyStatus = _authenticatedVaultClient.V1.System.GetKeyStatusAsync().Result;
            DisplayJson(keyStatus);
            Assert.True(keyStatus.Data.SequentialKeyNumber == 1);

            // get leader
            var leader = _unauthenticatedVaultClient.V1.System.GetLeaderAsync().Result;
            DisplayJson(leader);
            Assert.NotNull(leader.Address);

            // lease tests. raja todo: do it when we have a call with a lease_id.

            // raja todo: license apis. enterprise vault only.

            // mfa test cases.

            // duo

            // gives path not supported errors?. raja todo
            /*
            var duoAuthBackend = authBackends.Data.Values.First(); 

            var duoConfig = new DuoConfig
            {
                Name = "duo-name",
                MountAccessor = duoAuthBackend.Accessor,
                Type = "duo" // raja todo. change into getter only types.
            };

            _authenticatedVaultClient.V1.System.MFA.Duo.ConfigureAsync(duoConfig).Wait();

            var readDuo = _authenticatedVaultClient.V1.System.MFA.Duo.GetConfigAsync(duoConfig.Name).Result;
            DisplayJson(readDuo);
            Assert.Equal(duoConfig.MountAccessor, readDuo.Data.MountAccessor);

            _authenticatedVaultClient.V1.System.MFA.Duo.DeleteConfigAsync(duoConfig.Name).Wait();
            */

            // mounted secret backends.

            var secretBackends = _authenticatedVaultClient.V1.System.GetSecretBackendsAsync().Result;
            DisplayJson(secretBackends);
            Assert.True(secretBackends.Data.Any());

            var mountConfig = _authenticatedVaultClient.V1.System.GetSecretBackendConfigAsync(secretBackends.Data.First().Key).Result;
            DisplayJson(mountConfig);
            Assert.True(mountConfig.Data.MaximumLeaseTtl > 0);

            // mount a new secret backend
            var newSecretBackend = new SecretsEngine
            {
                Type = SecretsEngineType.AWS,
                Path = "aws1",
                Description = "e2e tests"
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(newSecretBackend).Wait();

            var ttl = 36000;

            _authenticatedVaultClient.V1.System.ConfigureSecretBackendAsync(newSecretBackend.Path, new BackendConfig { DefaultLeaseTtl = ttl, MaximumLeaseTtl = ttl, ForceNoCache = true }).Wait();

            var newMountConfig = _authenticatedVaultClient.V1.System.GetSecretBackendConfigAsync(newSecretBackend.Path).Result;
            DisplayJson(newMountConfig);
            Assert.Equal(ttl, newMountConfig.Data.DefaultLeaseTtl);

            // get secret backends
            var newSecretBackends = _authenticatedVaultClient.V1.System.GetSecretBackendsAsync().Result;
            DisplayJson(newSecretBackends);
            Assert.Equal(secretBackends.Data.Count() + 1, newSecretBackends.Data.Count());

            // unmount
            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(newSecretBackend.Path).Wait();

            // get secret backends
            var oldSecretBackends = _authenticatedVaultClient.V1.System.GetSecretBackendsAsync().Result;
            DisplayJson(oldSecretBackends);
            Assert.Equal(secretBackends.Data.Count(), oldSecretBackends.Data.Count());

            // remount - raja todo

            /*
             
            // mount a new secret backend
            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(newSecretBackend).Wait();
             
            // var newPath = "aws2";
            // _authenticatedVaultClient.V1.System.RemountSecretBackendAsync(newSecretBackend.Path, new Path);

            // get new secret backend config
            var config = _authenticatedVaultClient.GetMountedSecretBackendConfigurationAsync(newMountPoint);
            Assert.NotNull(config);

            // unmount
            _authenticatedVaultClient.UnmountSecretBackendAsync(newMountPoint);

            // quick
            secretBackends = _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            _authenticatedVaultClient.QuickMountSecretBackendAsync(SecretBackendType.AWS);
            newSecretBackends = _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count() + 1, newSecretBackends.Data.Count());

            _authenticatedVaultClient.QuickUnmountSecretBackendAsync(SecretBackendType.AWS);
            newSecretBackends = _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count(), newSecretBackends.Data.Count());
            */

            // catalog and plugin apis.

            var plugins = _authenticatedVaultClient.V1.System.Plugins.GetCatalogAsync().Result;
            DisplayJson(plugins);
            Assert.True(plugins.Data.Keys.Any());

            var pluginConfig = _authenticatedVaultClient.V1.System.Plugins.GetConfigAsync(plugins.Data.Keys.First()).Result;
            DisplayJson(pluginConfig);
            Assert.Equal(plugins.Data.Keys.First(), pluginConfig.Data.Name);

            // cannot unregister inbuilt plugins. try our own. raja todo.
            // _authenticatedVaultClient.V1.System.Plugins.UnregisterAsync(pluginConfig.Data.Name).Wait();

            // var lessPlugins = _authenticatedVaultClient.V1.System.Plugins.GetCatalogAsync().Result;
            // DisplayJson(lessPlugins);

            // Assert.Equal(plugins.Data.Keys.Count - 1, lessPlugins.Data.Keys.Count);

            // // cannot reload inbuilt plugins. try our own. raja todo

            var newPluginConfig = new PluginConfig
            {
                Name = pluginConfig.Data.Name,
                Command = pluginConfig.Data.Command + " ",
                Sha256 = pluginConfig.Data.Sha256
            };

            // _authenticatedVaultClient.V1.System.Plugins.RegisterAsync(newPluginConfig).Wait();

            pluginConfig = _authenticatedVaultClient.V1.System.Plugins.GetConfigAsync(newPluginConfig.Name).Result;
            DisplayJson(pluginConfig);
            // Assert.Equal(newPluginConfig.Command, pluginConfig.Data.Command);

            // raja todo ReloadBackendsAsync. do it after we do mounting tests for particular plugin backends.
            // in-built
            // ["cassandra-database-plugin","hana-database-plugin","mongodb-database-plugin","mssql-database-plugin","mysql-aurora-database-plugin",
            //  "mysql-database-plugin","mysql-legacy-database-plugin","mysql-rds-database-plugin","postgresql-database-plugin"]

            // policy apis.

            var policies = _authenticatedVaultClient.V1.System.GetPoliciesAsync().Result;
            DisplayJson(policies);
            Assert.True(policies.Data.Keys.Any());

            var policy = _authenticatedVaultClient.V1.System.GetPolicyAsync(policies.Data.Keys.ElementAt(0)).Result;
            DisplayJson(policy);
            Assert.NotNull(policy);

            // write a new policy
            var newPolicy = new Policy
            {
                Name = "gubdu",
                Rules = "path \"sys/*\" {  policy = \"deny\" }"
            };

            _authenticatedVaultClient.V1.System.WritePolicyAsync(newPolicy).Wait();

            // get new policy
            var newPolicyGet = _authenticatedVaultClient.V1.System.GetPolicyAsync(newPolicy.Name).Result;
            DisplayJson(newPolicyGet);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Data.Rules);

            // write updates to a new policy
            newPolicy.Rules = "path \"sys/*\" {  policy = \"read\" }";

            _authenticatedVaultClient.V1.System.WritePolicyAsync(newPolicy).Wait();

            // get new policy
            newPolicyGet = _authenticatedVaultClient.V1.System.GetPolicyAsync(newPolicy.Name).Result;
            DisplayJson(newPolicyGet);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Data.Rules);

            var newPolicies = _authenticatedVaultClient.V1.System.GetPoliciesAsync().Result;
            DisplayJson(newPolicies);
            Assert.True(newPolicies.Data.Keys.Any());

            // delete policy
            _authenticatedVaultClient.V1.System.DeletePolicyAsync(newPolicy.Name).Wait();

            // get all policies
            var oldPolicies = _authenticatedVaultClient.V1.System.GetPoliciesAsync().Result;
            DisplayJson(oldPolicies);
            Assert.Equal(newPolicies.Data.Keys.Count(), oldPolicies.Data.Keys.Count() + 1);

            // get ACL policy apis.

            var aclPolicies = _authenticatedVaultClient.V1.System.GetACLPoliciesAsync().Result;
            DisplayJson(aclPolicies);
            Assert.True(aclPolicies.Data.Keys.Any());

            var aclPolicy = _authenticatedVaultClient.V1.System.GetACLPolicyAsync(aclPolicies.Data.Keys.ElementAt(0)).Result;
            DisplayJson(aclPolicy);
            Assert.NotNull(aclPolicy);

            // write a new policy
            var newAclPolicy = new ACLPolicy
            {
                Name = "gubdu",
                Policy = "path \"sys/*\" {  policy = \"deny\" }"
            };

            _authenticatedVaultClient.V1.System.WriteACLPolicyAsync(newAclPolicy).Wait();

            // get new policy
            var newAclPolicyGet = _authenticatedVaultClient.V1.System.GetACLPolicyAsync(newPolicy.Name).Result;
            DisplayJson(newAclPolicyGet);
            Assert.Equal(newAclPolicy.Policy, newAclPolicyGet.Data.Policy);

            // write updates to a new policy
            newAclPolicy.Policy = "path \"sys/*\" {  policy = \"read\" }";

            _authenticatedVaultClient.V1.System.WriteACLPolicyAsync(newAclPolicy).Wait();

            // get new policy
            newAclPolicyGet = _authenticatedVaultClient.V1.System.GetACLPolicyAsync(newAclPolicy.Name).Result;
            DisplayJson(newAclPolicyGet);
            Assert.Equal(newAclPolicy.Policy, newAclPolicyGet.Data.Policy);

            var newAclPolicies = _authenticatedVaultClient.V1.System.GetACLPoliciesAsync().Result;
            DisplayJson(newAclPolicies);
            Assert.True(newAclPolicies.Data.Keys.Any());

            // delete policy
            _authenticatedVaultClient.V1.System.DeleteACLPolicyAsync(newAclPolicy.Name).Wait();

            // get all policies
            var oldAclPolicies = _authenticatedVaultClient.V1.System.GetACLPoliciesAsync().Result;
            DisplayJson(oldAclPolicies);
            Assert.Equal(newAclPolicies.Data.Keys.Count(), oldAclPolicies.Data.Keys.Count() + 1);

            // raja todo: enterprise RGP and EGP policy APIs.
            // raja todo: vault-override-policy header.

            // raw secret apis.
            // ** THis RAW path is OFF by default. Turn it on from the vault startup config file.
            // https://www.vaultproject.io/docs/configuration/index.html#raw_storage_endpoint

            var rawPath1 = "secret/raw/path1";
            var rawValues1 = new Dictionary<string, object>
            {
                {"foo", "bar"},
                {"foo2", 345 }
            };

            _authenticatedVaultClient.V1.System.WriteRawSecretAsync(rawPath1, rawValues1).Wait();

            var readRawValues1 = _authenticatedVaultClient.V1.System.ReadRawSecretAsync(rawPath1).Result;
            DisplayJson(readRawValues1);
            Assert.True(readRawValues1.Data.Count == 2);

            rawValues1["foo"] = "bar42";
            _authenticatedVaultClient.V1.System.WriteRawSecretAsync(rawPath1, rawValues1).Wait();

            readRawValues1 = _authenticatedVaultClient.V1.System.ReadRawSecretAsync(rawPath1).Result;
            DisplayJson(readRawValues1);
            Assert.Equal("bar42", readRawValues1.Data["foo"]);

            var rawPath2 = "secret/raw/path2";
            var rawValues2 = new Dictionary<string, object>
            {
                {"foo", "bar2"},
                {"foo2", 346 }
            };

            _authenticatedVaultClient.V1.System.WriteRawSecretAsync(rawPath2, rawValues2).Wait();

            var rawKeys = _authenticatedVaultClient.V1.System.GetRawSecretKeysAsync("secret/raw/").Result;
            DisplayJson(rawKeys);
            Assert.True(rawKeys.Data.Keys.Count() == 2);

            _authenticatedVaultClient.V1.System.DeleteRawSecretAsync(rawPath1).Wait();
            _authenticatedVaultClient.V1.System.DeleteRawSecretAsync(rawPath2).Wait();

            // rekey apis.

            var rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
            DisplayJson(rekeyStatus);
            Assert.False(rekeyStatus.Started);

            var newInitOptions = new InitOptions
            {
                SecretThreshold = 4,
                SecretShares = 8
            };

            rekeyStatus = _unauthenticatedVaultClient.V1.System.InitiateRekeyAsync(newInitOptions.SecretShares, newInitOptions.SecretThreshold).Result;
            DisplayJson(rekeyStatus);
            Assert.True(rekeyStatus.Started);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.NotNull(rekeyStatus.Nonce);

            // raja todo: test the rekey backup API, after giving good pgp encrypted keys.

            // var backups = _authenticatedVaultClient.V1.System.GetRekeyBackupKeysAsync().Result;
            // DisplayJson(backups);
            // Assert.NotNull(backups);

            _authenticatedVaultClient.V1.System.DeleteRekeyBackupKeysAsync().Wait();

            var rekeyNonce = rekeyStatus.Nonce;
            RekeyProgress rekeyProgress = null;
            int j = 0;

            for (j = 0; j < initOptions.SecretThreshold - 1; ++j)
            {
                rekeyProgress = _unauthenticatedVaultClient.V1.System.ContinueRekeyAsync(masterCredentials.MasterKeys[j], rekeyNonce).Result;
                DisplayJson(rekeyProgress);
                Assert.False(rekeyProgress.Complete);
                Assert.Null(rekeyProgress.MasterKeys);

                rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
                DisplayJson(rekeyStatus);
                Assert.True(rekeyStatus.Started);
                Assert.True(rekeyStatus.UnsealKeysProvided == (j+1));
            }

            rekeyProgress = _unauthenticatedVaultClient.V1.System.ContinueRekeyAsync(masterCredentials.MasterKeys[j], rekeyNonce).Result;
            DisplayJson(rekeyProgress);
            Assert.True(rekeyProgress.Complete);
            Assert.NotNull(rekeyProgress.MasterKeys);
            Assert.Equal(newInitOptions.SecretShares, rekeyProgress.MasterKeys.Length);

            // new keys.
            masterCredentials.MasterKeys = rekeyProgress.MasterKeys;
            masterCredentials.Base64MasterKeys = rekeyProgress.Base64MasterKeys;

            rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
            DisplayJson(rekeyStatus);

            Assert.False(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 0);
            Assert.True(rekeyStatus.RequiredUnsealKeys == newInitOptions.SecretThreshold);
            Assert.True(rekeyStatus.SecretShares == 0);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.Equal(string.Empty, rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            _unauthenticatedVaultClient.V1.System.InitiateRekeyAsync(5, 5).Wait();

            rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
            DisplayJson(rekeyStatus);
            Assert.True(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 5);
            Assert.True(rekeyStatus.RequiredUnsealKeys == newInitOptions.SecretThreshold);
            Assert.True(rekeyStatus.SecretShares == 5);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.NotNull(rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            _unauthenticatedVaultClient.V1.System.CancelRekeyAsync().Wait();
            rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
            DisplayJson(rekeyStatus);
            Assert.False(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 0);
            Assert.True(rekeyStatus.RequiredUnsealKeys == newInitOptions.SecretThreshold);
            Assert.True(rekeyStatus.SecretShares == 0);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.Equal(string.Empty, rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            _unauthenticatedVaultClient.V1.System.InitiateRekeyAsync(2, 2).Wait();

            rekeyStatus = _unauthenticatedVaultClient.V1.System.GetRekeyStatusAsync().Result;
            DisplayJson(rekeyStatus);

            var quick = _unauthenticatedVaultClient.V1.System.QuickRekeyAsync(masterCredentials.MasterKeys, rekeyStatus.Nonce).Result;
            DisplayJson(quick);
            Assert.True(quick.Complete);

            masterCredentials.MasterKeys = quick.MasterKeys;
            masterCredentials.Base64MasterKeys = quick.Base64MasterKeys;

            authSettings = GetVaultClientSettings(new TokenAuthMethodInfo(masterCredentials.RootToken));
            _authenticatedVaultClient = new VaultClient(authSettings);

            var data = new Dictionary<string, object>
            {
                {"key1", "wrappy"},
                {"key2", 23},
                {"key3", true}
            };

            var wrapTimeToLive = "1h";

            var wrappedResponse = _authenticatedVaultClient.V1.System.WrapResponseDataAsync(data, wrapTimeToLive).Result;
            DisplayJson(wrappedResponse);
            Assert.NotNull(wrappedResponse.WrapInfo.Token);

            var tokenWrapInfo = _authenticatedVaultClient.V1.System.LookupTokenWrapInfoAsync(wrappedResponse.WrapInfo.Token).Result;
            DisplayJson(tokenWrapInfo);
            Assert.NotEqual(DateTimeOffset.MinValue, tokenWrapInfo.Data.CreationTime);

            var rewrappedResponse = _authenticatedVaultClient.V1.System.RewrapWrappedResponseDataAsync(wrappedResponse.WrapInfo.Token).Result;
            DisplayJson(rewrappedResponse);
            Assert.NotNull(rewrappedResponse.WrapInfo.Token);
            Assert.NotEqual(wrappedResponse.WrapInfo.Token, rewrappedResponse.WrapInfo.Token);

            var unwrappedResponse = _authenticatedVaultClient.V1.System.UnwrapWrappedResponseDataAsync<Dictionary<string, object>>(rewrappedResponse.WrapInfo.Token).Result;
            DisplayJson(unwrappedResponse);
            Assert.Equal(data.Count, unwrappedResponse.Data.Count);

            wrappedResponse = _authenticatedVaultClient.V1.System.WrapResponseDataAsync(null, wrapTimeToLive).Result;
            DisplayJson(wrappedResponse);
            Assert.NotNull(wrappedResponse.WrapInfo.Token);

            unwrappedResponse = _authenticatedVaultClient.V1.System.UnwrapWrappedResponseDataAsync<Dictionary<string, object>>(wrappedResponse.WrapInfo.Token).Result;
            DisplayJson(unwrappedResponse);
            Assert.True(unwrappedResponse.Data.Count == 0);
        }

        private static VaultClientSettings GetVaultClientSettings(IAuthMethodInfo authMethodInfo = null)
        {
            var settings = new VaultClientSettings("http://localhost:8200", authMethodInfo)
            {
                AfterApiResponseAction = r =>
                {
                    var value = ((int)r.StatusCode + "-" + r.StatusCode) + "\n";
                    var content = r.Content != null ? r.Content.ReadAsStringAsync().Result : string.Empty;

                    _responseContent = "From Vault Server: " + value + content;

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine(_responseContent);
                    }
                }
            };

            return settings;
        }

        private static void DisplayJson<T>(T value)
        {
            string line = "===========";

            var type = typeof(T);
            var genTypes = type.GenericTypeArguments;

            if (genTypes != null && genTypes.Length == 1)
            {
                var genType = genTypes[0];
                var subGenTypes = genType.GenericTypeArguments;

                // single generic. e.g. SecretsEngine<AuthBackend>
                if (subGenTypes == null || subGenTypes.Length == 0)
                {
                    Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + genType.Name + ">");
                }
                else
                {
                    // single sub-generic e.g. SecretsEngine<IEnumerable<AuthBackend>>
                    if (subGenTypes.Length == 1)
                    {
                        var subGenType = subGenTypes[0];

                        Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" +
                                          genType.Name.Substring(0, genType.Name.IndexOf('`')) +
                                          "<" + subGenType.Name + ">>");
                    }
                    else
                    {
                        // double generic. e.g. SecretsEngine<Dictionary<string, AuthBackend>>
                        if (subGenTypes.Length == 2)
                        {
                            var subGenType1 = subGenTypes[0];
                            var subGenType2 = subGenTypes[1];

                            Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" +
                                              genType.Name.Substring(0, genType.Name.IndexOf('`')) +
                                              "<" + subGenType1.Name + ", " + subGenType2.Name + ">>");
                        }
                    }
                }
            }
            else
            {
                // non-generic.
                Console.WriteLine(type.Name);
            }

            Console.WriteLine(line + line);
            Console.WriteLine(_responseContent);
            Console.WriteLine(JsonConvert.SerializeObject(value));
            Console.WriteLine(line + line);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
