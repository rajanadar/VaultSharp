using System;
using System.Linq;
using Newtonsoft.Json;
using VaultSharp.Backends.Auth.Token;
using VaultSharp.Backends.System;
using Xunit;

namespace VaultSharp.Samples
{
    class Program
    {
        private static IVaultClient UnauthenticatedVaultClient;
        private static IVaultClient AuthenticatedVaultClient;

        private static string ResponseContent;

        public static void Main(string[] args)
        {
            var settings = GetVaultClientSettings();
            UnauthenticatedVaultClient = new VaultClient(settings);

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
            var exception = Assert.ThrowsAsync<Exception>(() => UnauthenticatedVaultClient.V1.System.GetSealStatusAsync()).Result;
            Assert.Contains("not yet initialized", exception.Message);

            // init
            var initStatus = UnauthenticatedVaultClient.V1.System.GetInitStatusAsync().Result;
            Assert.False(initStatus);

            var initOptions = new InitOptions
            {
                SecretShares = 10,
                SecretThreshold = 5
            };

            var masterCredentials = UnauthenticatedVaultClient.V1.System.InitAsync(initOptions).Result;
            DisplayJson(masterCredentials);

            Assert.Equal(initOptions.SecretShares, masterCredentials.MasterKeys.Length);
            Assert.Equal(initOptions.SecretShares, masterCredentials.Base64MasterKeys.Length);

            initStatus = UnauthenticatedVaultClient.V1.System.GetInitStatusAsync().Result;
            DisplayJson(initStatus);

            Assert.True(initStatus);

            // unseal

            var sealStatus = UnauthenticatedVaultClient.V1.System.GetSealStatusAsync().Result;
            DisplayJson(sealStatus);
            Assert.True(sealStatus.Sealed);

            var threshold = 0;
            var reset = false;

            foreach (var masterKey in masterCredentials.MasterKeys)
            {
                ++threshold;
                var unsealStatus = UnauthenticatedVaultClient.V1.System.UnsealAsync(masterKey).Result;

                DisplayJson(unsealStatus);

                if (threshold < initOptions.SecretThreshold)
                {
                    Assert.Equal(threshold, unsealStatus.Progress);
                    Assert.True(unsealStatus.Sealed);

                    // unseal with reset now.

                    if (!reset && (threshold == initOptions.SecretThreshold - 2))
                    {
                        unsealStatus = UnauthenticatedVaultClient.V1.System.UnsealAsync(masterKey, true).Result;

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

            // seal it

            var authSettings = GetVaultClientSettings();
            authSettings.AuthInfo = new TokenAuthInfo(masterCredentials.RootToken);
            AuthenticatedVaultClient = new VaultClient(authSettings);

            AuthenticatedVaultClient.V1.System.SealAsync().Wait();
            sealStatus = UnauthenticatedVaultClient.V1.System.GetSealStatusAsync().Result;
            DisplayJson(sealStatus);
            Assert.True(sealStatus.Sealed);

            // quick unseal
            sealStatus = UnauthenticatedVaultClient.V1.System.QuickUnsealAsync(masterCredentials.MasterKeys).Result;
            DisplayJson(sealStatus);
            Assert.False(sealStatus.Sealed);

            // audit backends
            var audits = AuthenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
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

            AuthenticatedVaultClient.V1.System.MountAuditBackendAsync(newFileAudit).Wait();

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

            AuthenticatedVaultClient.V1.System.MountAuditBackendAsync(newFileAudit2).Wait();

            // get audits
            var newAudits = AuthenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
            DisplayJson(newAudits);
            Assert.Equal(audits.Data.Count() + 2, newAudits.Data.Count());

            // hash with audit
            // var hash = await _authenticatedVaultClient.HashWithAuditBackendAsync(newFileAudit.MountPoint, "testinput");
            // Assert.NotNull(hash);

            // disabled audit
            AuthenticatedVaultClient.V1.System.UnmountAuditBackendAsync(newFileAudit.MountPoint).Wait();
            AuthenticatedVaultClient.V1.System.UnmountAuditBackendAsync(newFileAudit2.MountPoint).Wait();

            // get audits
            var oldAudits = AuthenticatedVaultClient.V1.System.GetAuditBackendsAsync().Result;
            Assert.Equal(audits.Data.Count(), oldAudits.Data.Count());

            // syslog is not supported on windows. so no acceptance tests possible.
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

                    ResponseContent = value + content;

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine(ResponseContent);
                    }
                }
            };

            return settings;
        }

        private static void DisplayJson<T>(T value)
        {
            string line = "============";
            Console.WriteLine(typeof(T).Name);

            Console.WriteLine(line + line);
            Console.WriteLine(ResponseContent);
            Console.WriteLine(JsonConvert.SerializeObject(value));
            Console.WriteLine(line + line);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
