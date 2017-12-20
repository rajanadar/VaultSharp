using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using VaultSharp.Backends;
using VaultSharp.Backends.Auth;
using VaultSharp.Backends.Auth.Token;
using VaultSharp.Backends.System;
using VaultSharp.Core;
using Xunit;

namespace VaultSharp.Samples
{
    class Program
    {
        private static IVaultClient _unauthenticatedVaultClient;
        private static IVaultClient _authenticatedVaultClient;

        private static string _responseContent;

        public static void Main(string[] args)
        {
            var settings = GetVaultClientSettings();
            _unauthenticatedVaultClient = new VaultClient(settings);

            RunAllSamples();

            Console.WriteLine();
            Console.Write("I think we are done here...");
            Console.ReadLine();
        }

        private static void RunAllSamples()
        {
            // before runnig these tests, just start your local vault server with a file backend.
            // don't init or unseal it.

            RunSystemBackendSamples();
            RunAuthBackendSamples();
            RunSecretBackendSamples();
        }

        private static void RunAuthBackendSamples()
        {
        }

        private static void RunSecretBackendSamples()
        {
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
            Assert.False(health.Initialized);
            Assert.True(health.Sealed);
            Assert.Equal((int)HttpStatusCode.NotImplemented, health.HttpStatusCode);

            // do just one head check.
            health = _unauthenticatedVaultClient.V1.System.GetHealthStatusAsync(queryHttpMethod: HttpMethod.Head).Result;
            DisplayJson(health);
            Assert.Equal((int)HttpStatusCode.NotImplemented, health.HttpStatusCode);

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

            var authSettings = GetVaultClientSettings();
            authSettings.AuthInfo = new TokenAuthInfo(masterCredentials.RootToken);
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
            var newAuth = new AuthBackend
            {
                Path = "github1",
                Type = AuthBackendType.GitHub,
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

            // control group config
            // blocked due to https://github.com/hashicorp/vault/issues/3702
            /*
            var cgconfig = _authenticatedVaultClient.V1.System.GetControlGroupConfigAsync().Result;
            DisplayJson(cgconfig);

            _authenticatedVaultClient.V1.System.ConfigureControlGroupAsync("4h").Wait();

            cgconfig = _authenticatedVaultClient.V1.System.GetControlGroupConfigAsync().Result;
            DisplayJson(cgconfig);
            Assert.Equal("4h", cgconfig.Data.MaxTimeToLive);

            _authenticatedVaultClient.V1.System.DeleteControlGroupConfigAsync().Wait();
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

            var cgStatus = _authenticatedVaultClient.V1.System.CheckControlGroupStatusAsync(cgTokenAccessor).Result;
            DisplayJson(cgStatus);
            Assert.False(cgStatus.Data.Approved);

            cgStatus = _authenticatedVaultClient.V1.System.AuthorizeControlGroupAsync(cgTokenAccessor).Result;
            DisplayJson(cgStatus);
            Assert.False(cgStatus.Data.Approved);

            cgStatus = _authenticatedVaultClient.V1.System.CheckControlGroupStatusAsync(cgTokenAccessor).Result;
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
        }

        private static VaultClientSettings GetVaultClientSettings()
        {
            var settings = new VaultClientSettings
            {
                VaultServerUriWithPort = "http://localhost:8200",
                AfterApiResponseAction = r =>
                {
                    var value = ((int)r.StatusCode + "-" + r.StatusCode) + "\n";
                    var content = r.Content != null ? r.Content.ReadAsStringAsync().Result : string.Empty;

                    _responseContent = value + content;

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
            string line = "============";

            var type = typeof(T);
            var genTypes = type.GenericTypeArguments;

            if (genTypes != null && genTypes.Length == 1)
            {
                var genType = genTypes[0];
                var subGenTypes = genType.GenericTypeArguments;

                // single generic. e.g. Secret<AuthBackend>
                if (subGenTypes == null || subGenTypes.Length == 0)
                {
                    Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + genType.Name + ">");
                }
                else
                {
                    // single sub-generic e.g. Secret<IEnumerable<AuthBackend>>
                    if (subGenTypes.Length == 1)
                    {
                        var subGenType = subGenTypes[0];

                        Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" +
                                          genType.Name.Substring(0, genType.Name.IndexOf('`')) +
                                          "<" + subGenType.Name + ">>");
                    }
                    else
                    {
                        // double generic. e.g. Secret<Dictionary<string, AuthBackend>>
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
