
using System;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunSecretsEngineSamples()
        {
            Console.WriteLine("\n RunActiveDirectorySecretsBackendSamples \n");
            RunActiveDirectorySecretsBackendSamples();

            Console.WriteLine("\n RunAliCloudSecretsBackendSamples \n");
            RunAliCloudSecretsBackendSamples();

            Console.WriteLine("\n RunCubbyHoleSecretsBackendSamples \n");
            RunCubbyHoleSecretsBackendSamples();

            Console.WriteLine("\n RunKeyValueSecretsBackendSamples \n");
            RunKeyValueSecretsBackendSamples();

            Console.WriteLine("\n RunTOTPSecretsBackendSamples \n");
            RunTOTPSecretsBackendSamples();

            Console.WriteLine("\n RunTransitSecretsBackendSamples \n");
            RunTransitSecretsBackendSamples();
        }
    }
}
