using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaultSharp.Core;

using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;
using VaultSharp.V1.SecretsEngines.Transit;
using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunSecretsEngineSamples()
        {
            RunActiveDirectorySecretsBackendSamples();
            RunAliCloudSecretsBackendSamples();
            RunCubbyHoleSecretsBackendSamples();
            RunKeyValueSecretsBackendSamples();
            RunTransitSecretsBackendSamples();
        }
    }
}
