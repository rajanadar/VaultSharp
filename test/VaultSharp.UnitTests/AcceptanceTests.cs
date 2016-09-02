using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.Token;
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
        public async Task TheTest()
        {
            try
            {
                StartupVaultServer();
                await RunAllAcceptanceTestsAsync();
            }
            finally
            {
                ShutdownVaultServer();
            }
        }

        private async Task RunAllAcceptanceTestsAsync()
        {
            await RunInitApiTests();
            await RunSealApiTests();
            await RunGenerateRootApiTests();
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