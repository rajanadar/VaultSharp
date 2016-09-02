using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests
{
    public class AcceptanceTests
    {
        // Acceptance tests setup tests:
        // 1. Ensure you have the right version of Vault.exe installed on the machine running this test.
        // 2. Fill in the variables below in SetupData.
        // 3. Please ensure this is not your production vault or anything. And this is a box you can play with.
        // 4. The acceptance tests will setup a temporary file backend, run the tests and tear it down finally.

        /// <summary>
        /// Change the data in this class as suitable.
        /// This is the only class you should be modifying for the acceptance tests to run successfully.
        /// </summary>
        private static class SetupData
        {
            public const string VaultExeFullPath = @"c:\temp\raja\vaultsharp-acceptance-tests\vault.exe";
        }

        // no need to modify these values.

        private const string FileBackendsFolderName = "per_run_file_backends_delete_anytime";
        private const string VaultConfigsFolderName = "per_run_vault_configs_delete_anytime";
        private const string FileBackendPlaceHolder = "##FILE_BACKEND_PATH##";
        private const string VaultConfigPath = "acceptance-tests-vault-config.hcl";
        private static readonly Uri VaultUriWithPort = new Uri("http://127.0.0.1:8200");

        private static IVaultClient _unauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(
            VaultUriWithPort, authenticationInfo: null);

        private static IVaultClient _authenticatedVaultClient;

        private static Process VaultProcess;
        private static MasterCredentials MasterCredentials;

        /// <summary>
        /// The one stop test for all the Vault APIs.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAllAcceptanceTestsAsync()
        {
            try
            {
                StartupVaultServer();

                await RunInitApiTests();
                await RunSealApiTests();
                await RunGenerateRootApiTests();
                await RunSecretBackendMountApiTests();
                await RunAuthenticationBackendMountApiTests();
                await RunPolicyApiTests();
                await RunCapabilitiesApiTests();
            }
            finally
            {
                ShutdownVaultServer();
            }
        }

        private async Task RunCapabilitiesApiTests()
        {
            var secret1 = await _authenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions {NoParent = true});

            var caps =
                await _authenticatedVaultClient.GetTokenCapabilitiesAsync(secret1.AuthorizationInfo.ClientToken, "sys/mounts");
            Assert.NotNull(caps);

            var caps2 = await _authenticatedVaultClient.GetCallingTokenCapabilitiesAsync("sys/mounts");
            Assert.NotNull(caps2);

            var cap3 =
                await
                    _authenticatedVaultClient.GetTokenAccessorCapabilitiesAsync(
                        secret1.AuthorizationInfo.ClientTokenAccessor, "sys/mounts");
            Assert.NotNull(cap3);
        }

        private async Task RunPolicyApiTests()
        {
            var policies = (await _authenticatedVaultClient.GetAllPoliciesAsync()).ToList();
            Assert.True(policies.Any());

            var policy = await _authenticatedVaultClient.GetPolicyAsync(policies[0]);
            Assert.NotNull(policy);

            // write a new policy
            var newPolicy = new Policy
            {
                Name = "gubdu",
                Rules = "path \"sys/*\" {  policy = \"deny\" }"
            };

            await _authenticatedVaultClient.WritePolicyAsync(newPolicy);

            // get new policy
            var newPolicyGet = await _authenticatedVaultClient.GetPolicyAsync(newPolicy.Name);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

            // write updates to a new policy
            newPolicy.Rules = "path \"sys/*\" {  policy = \"read\" }";

            await _authenticatedVaultClient.WritePolicyAsync(newPolicy);

            // get new policy
            newPolicyGet = await _authenticatedVaultClient.GetPolicyAsync(newPolicy.Name);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

            // delete policy
            await _authenticatedVaultClient.DeletePolicyAsync(newPolicy.Name);

            // get all policies
            var oldPolicies = (await _authenticatedVaultClient.GetAllPoliciesAsync()).ToList();
            Assert.Equal(policies.Count, oldPolicies.Count);
        }

        private async Task RunAuthenticationBackendMountApiTests()
        {
            // get Authentication backends
            var authenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authenticationBackends.Data.Any());

            var mountConfig = await _authenticatedVaultClient.GetMountedAuthenticationBackendConfigurationAsync(authenticationBackends.Data.First().AuthenticationPath);
            Assert.NotNull(mountConfig);

            // enable new auth
            var newAuth = new AuthenticationBackend
            {
                AuthenticationPath = "github1",
                BackendType = AuthenticationBackendType.GitHub,
                Description = "Github auth - test cases"
            };

            await _authenticatedVaultClient.EnableAuthenticationBackendAsync(newAuth);

            string ttl = "11h";

            await
                _authenticatedVaultClient.TuneAuthenticationBackendConfigurationAsync(newAuth.AuthenticationPath,
                    new MountConfiguration { DefaultLeaseTtl = ttl, MaximumLeaseTtl = ttl });

            mountConfig =
                await
                    _authenticatedVaultClient.GetMountedAuthenticationBackendConfigurationAsync(
                        newAuth.AuthenticationPath);

            Assert.NotNull(mountConfig);

            // get all auths
            var newAuthenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.Equal(authenticationBackends.Data.Count() + 1, newAuthenticationBackends.Data.Count());

            // disable auth
            await _authenticatedVaultClient.DisableAuthenticationBackendAsync(newAuth.AuthenticationPath);

            // get all auths
            var oldAuthenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.Equal(authenticationBackends.Data.Count(), oldAuthenticationBackends.Data.Count());
        }

        private async Task RunSecretBackendMountApiTests()
        {
            var secretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.True(secretBackends.Data.Any());

            var mountConfig = await _authenticatedVaultClient.GetMountedSecretBackendConfigurationAsync(secretBackends.Data.First().MountPoint);
            Assert.NotNull(mountConfig);

            // mount a new secret backend
            var newSecretBackend = new SecretBackend
            {
                BackendType = SecretBackendType.AWS,
                MountPoint = "aws1",
                Description = "e2e tests"
            };

            await _authenticatedVaultClient.MountSecretBackendAsync(newSecretBackend);

            string ttl = "10h";

            await
                _authenticatedVaultClient.TuneSecretBackendConfigurationAsync(newSecretBackend.MountPoint,
                    new MountConfiguration { DefaultLeaseTtl = ttl, MaximumLeaseTtl = ttl });

            // get secret backends
            var newSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count() + 1, newSecretBackends.Data.Count());

            // unmount
            await _authenticatedVaultClient.UnmountSecretBackendAsync(newSecretBackend.MountPoint);

            // get secret backends
            var oldSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count(), oldSecretBackends.Data.Count());

            // mount a new secret backend
            await _authenticatedVaultClient.MountSecretBackendAsync(newSecretBackend);

            // remount
            var newMountPoint = "aws2";
            await _authenticatedVaultClient.RemountSecretBackendAsync(newSecretBackend.MountPoint, newMountPoint);

            // get new secret backend config
            var config = await _authenticatedVaultClient.GetMountedSecretBackendConfigurationAsync(newMountPoint);
            Assert.NotNull(config);

            // unmount
            await _authenticatedVaultClient.UnmountSecretBackendAsync(newMountPoint);
        }

        private async Task RunInitApiTests()
        {
            await AssertInitializationStatusAsync(false);
            await InitializeVaultAsync();
            await AssertInitializationStatusAsync(true);
        }

        private async Task RunSealApiTests()
        {
            await AssertSealStatusAsync(true);
            await UnsealAsync();

            await AssertSealStatusAsync(false);
            await _authenticatedVaultClient.SealAsync();

            var sealStatus = await _unauthenticatedVaultClient.UnsealAsync(MasterCredentials.MasterKeys[0]);
            Assert.True(sealStatus.Sealed);
            Assert.False(sealStatus.Progress == 0);

            await _unauthenticatedVaultClient.UnsealAsync(resetCompletely: true);
            await AssertSealStatusAsync(true);

            await UnsealAsync();

            await _authenticatedVaultClient.SealAsync();
            await AssertSealStatusAsync(true);

            sealStatus = await _unauthenticatedVaultClient.QuickUnsealAsync(MasterCredentials.MasterKeys);
            Assert.False(sealStatus.Sealed);
        }

        private async Task RunGenerateRootApiTests()
        {
            var rootStatus = await _unauthenticatedVaultClient.GetRootTokenGenerationStatusAsync();
            Assert.False(rootStatus.Started);

            var otp = Convert.ToBase64String(Enumerable.Range(0, 16).Select(i => (byte) i).ToArray());
            rootStatus = await _unauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);

            Assert.True(rootStatus.Started);
            Assert.NotNull(rootStatus.Nonce);

            foreach (var masterKey in MasterCredentials.MasterKeys)
            {
                rootStatus = await _unauthenticatedVaultClient.ContinueRootTokenGenerationAsync(masterKey, rootStatus.Nonce);
            }

            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);

            rootStatus = await _unauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);

            rootStatus = await _unauthenticatedVaultClient.ContinueRootTokenGenerationAsync(MasterCredentials.MasterKeys[0], rootStatus.Nonce);
            Assert.True(rootStatus.Started);

            await _unauthenticatedVaultClient.CancelRootTokenGenerationAsync();

            rootStatus = await _unauthenticatedVaultClient.GetRootTokenGenerationStatusAsync();
            Assert.False(rootStatus.Started);

            rootStatus = await _unauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);
            rootStatus =
                await
                    _unauthenticatedVaultClient.QuickRootTokenGenerationAsync(MasterCredentials.MasterKeys,
                        rootStatus.Nonce);

            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);
        }

        private async Task UnsealAsync()
        {
            SealStatus sealStatus = null;

            foreach (var masterKey in MasterCredentials.MasterKeys)
            {
                sealStatus = await _unauthenticatedVaultClient.UnsealAsync(masterKey);
            }

            Assert.False(sealStatus.Sealed);
            Assert.Equal(0, sealStatus.Progress);
            Assert.NotNull(sealStatus.ClusterId);
            Assert.NotNull(sealStatus.ClusterName);

            _authenticatedVaultClient = VaultClientFactory.CreateVaultClient(VaultUriWithPort, new TokenAuthenticationInfo(MasterCredentials.RootToken));
        }

        private async Task AssertSealStatusAsync(bool expected)
        {
            var actual = await _unauthenticatedVaultClient.GetSealStatusAsync();
            Assert.Equal(expected, actual.Sealed);
        }

        private async Task InitializeVaultAsync()
        {
            MasterCredentials = await _unauthenticatedVaultClient.InitializeAsync(2, 2);
            Assert.NotNull(MasterCredentials);
        }

        private async Task AssertInitializationStatusAsync(bool expectedStatus)
        {
            var actual = await _unauthenticatedVaultClient.GetInitializationStatusAsync();
            Assert.Equal(expectedStatus, actual);
        }

        private void StartupVaultServer()
        {
            if (!File.Exists(SetupData.VaultExeFullPath))
            {
                throw new Exception("Vault EXE full path does not exist: " + SetupData.VaultExeFullPath);
            }

            if (!File.Exists(VaultConfigPath))
            {
                throw new Exception("Vault acceptance tests config file does not exist: " + VaultConfigPath);
            }

            var vaultFolder = Path.GetDirectoryName(SetupData.VaultExeFullPath);
            var fileBackendsFullRootFolderPath = Path.Combine(vaultFolder, FileBackendsFolderName);
            var vaultConfigsFullRootFolderPath = Path.Combine(vaultFolder, VaultConfigsFolderName);

            if (!Directory.Exists(vaultConfigsFullRootFolderPath))
            {
                Directory.CreateDirectory(vaultConfigsFullRootFolderPath);
            }

            var fileBackendFolderName = Guid.NewGuid().ToString();
            var fileBackendFullFolderPath = Path.Combine(fileBackendsFullRootFolderPath, fileBackendFolderName);

            if (Directory.Exists(fileBackendFullFolderPath))
            {
                throw new Exception("A directory of the same name already exists. Please try a new run." +
                                    fileBackendFullFolderPath);
            }

            Directory.CreateDirectory(fileBackendFullFolderPath);

            var config =
                File.ReadAllText(VaultConfigPath)
                    .Replace(FileBackendPlaceHolder, fileBackendFullFolderPath)
                    .Replace("\\", "\\\\");
            var testConfigFileName = fileBackendFolderName + ".hcl";

            var testConfigFullPath = Path.Combine(vaultConfigsFullRootFolderPath, testConfigFileName);
            File.WriteAllText(testConfigFullPath, config);

            var startupCommand = "\"" + SetupData.VaultExeFullPath + "\" server -config \"" + testConfigFullPath + "\"";

            var procStartInfo = new ProcessStartInfo();

            procStartInfo.FileName = "cmd";
            procStartInfo.Arguments = "/k \"" + startupCommand + "\"";
            procStartInfo.WorkingDirectory = Path.GetPathRoot(vaultFolder);

            VaultProcess = new Process();
            VaultProcess.StartInfo = procStartInfo;
            VaultProcess.Start();

            // sleep for a bit.
            Thread.Sleep(2000);
        }

        private void ShutdownVaultServer()
        {
            if (VaultProcess != null && !VaultProcess.HasExited)
            {
                VaultProcess.CloseMainWindow();
            }
        }
    }
}