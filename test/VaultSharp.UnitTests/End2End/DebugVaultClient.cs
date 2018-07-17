using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests.End2End
{
    /// <summary>
    /// This class expects the currently supported version of vault.exe file 
    /// in the bin/Debug folder of the Testing solution.
    /// The config is Configs\end2end.hcl
    /// </summary>
    public class DebugVaultClient : IDisposable
    {
        public MasterCredentials MasterCredentials { get; private set; }
        public IVaultClient AuthenticatedVaultClient { get; private set; }
        public IVaultClient UnauthenticatedVaultClient { get; private set; }
        private Process _vaultServerProcess;
        private string _workingDirectory;

        public DebugVaultClient(Uri vaultUri = null, bool unseal = true)
        {
            if (vaultUri == null)
            {
                vaultUri = new Uri("http://127.0.0.1:8200");
            }

            CheckForVaultInstallation(Directory.GetCurrentDirectory());

            _workingDirectory = CreateLocalFileBackend();

            _vaultServerProcess = StartVaultServerProcess();

            UnauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(vaultUri, null);
            MasterCredentials = InitializeVault();

            if (unseal) Unseal(vaultUri);
        }

        // Create fresh file_directory in the testing folder
        private string CreateLocalFileBackend()
        {
            var workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "file_backend");
            if (Directory.Exists(workingDirectory))
                Directory.Delete(workingDirectory, true);

            Directory.CreateDirectory(workingDirectory);

            return workingDirectory;
        }

        private void CheckForVaultInstallation(string folderPathToVaultExecutable)
        {
            var path = Path.Combine(folderPathToVaultExecutable, "vault.exe");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find the Vault executable. Excepted path: {path}");
            }
        }

        /// <summary>
        /// Run the vault.exe that is expected to be in the bin/Debug folder of this project
        /// </summary>
        /// <returns>
        /// <see cref="System.Diagnostics.Process"/> running the vault.exe
        /// </returns>
        private Process StartVaultServerProcess()
        {
            // Todo: Create Load Config helper methods
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "end2end.hcl");
            var startupArguements = $"server -log-level debug -config={configPath}";

            var processStartInfo = new ProcessStartInfo("vault.exe");
            processStartInfo.WorkingDirectory = _workingDirectory;
            processStartInfo.Arguments = startupArguements;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            Thread.Sleep(100);
            if (process.HasExited)
            {
                var err = process.StandardError.ReadToEnd();
                var output = process.StandardOutput.ReadToEnd();
                throw new Exception("The vault server executable used in these tests had trouble starting. \n" +
                    $"STDOUT: \"{output}\"\n" +
                    $"STDERR: \"{err}\"\n" +
                    "The End2End tests expect a vault executable in the bin/Debug folder."
                    );
            }

            return process;
        }

        /// <summary>
        /// Initialize the vault server. Basic health checks
        /// </summary>
        /// <returns><see cref="Backends.System.Models.MasterCredentials"/> of the Vault instance sealed and initialized</returns>
        private MasterCredentials InitializeVault()
        {
            var initialized = UnauthenticatedVaultClient.GetInitializationStatusAsync().Result;
            Assert.False(initialized);

            var healthStatus = UnauthenticatedVaultClient.GetHealthStatusAsync().Result;
            Assert.False(healthStatus.Initialized);

            // Initialize Vault
            var masterCredentials = UnauthenticatedVaultClient.InitializeAsync(new InitializeOptions
            {
                SecretShares = 5,
                SecretThreshold = 3
            }).Result;

            healthStatus = UnauthenticatedVaultClient.GetHealthStatusAsync().Result;
            Assert.True(healthStatus.Initialized);
            Assert.True(healthStatus.Sealed);

            return masterCredentials;
        }

        /// <summary>
        /// Unseal the vault instance
        /// </summary>
        private void Unseal(Uri vaultUri)
        {
            foreach (var masterKey in MasterCredentials.MasterKeys)
            {
                var sealStatus = UnauthenticatedVaultClient.UnsealAsync(masterKey).Result;

                if (!sealStatus.Sealed)
                {
                    var healthStatus = UnauthenticatedVaultClient.GetHealthStatusAsync().Result;
                    Assert.True(healthStatus.Initialized);
                    Assert.False(healthStatus.Sealed);

                    // we are acting as the root user here.

                    IAuthenticationInfo tokenAuthenticationInfo =
                        new TokenAuthenticationInfo(MasterCredentials.RootToken);
                    AuthenticatedVaultClient = VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);

                    break;
                }
            }
        }

        public void Dispose()
        {
            _vaultServerProcess.CloseMainWindow();
            // Todo: Figure out why WaitToExit() here doesn't work.
            // If WaitToExit() was used, the process was not shut down correctly
            // It had a lock on the config directory and the next Test could not run
            //_vaultServerProcess.WaitForExit(100);
            _vaultServerProcess.Kill();
        }
    }
}