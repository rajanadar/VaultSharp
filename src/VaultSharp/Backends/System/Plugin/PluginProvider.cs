using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System.Plugin
{
    /// <summary>
    /// Plugin provider.
    /// </summary>
    internal class PluginProvider : IPlugin
    {
        private readonly Polymath _polymath;

        public PluginProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task ReloadBackendsAsync(string plugin, IEnumerable<string> backendMountPaths)
        {
            var requestData = new
            {
                plugin = plugin,
                mounts = backendMountPaths
            };

            await _polymath.MakeVaultApiRequest("v1/sys/plugin/reload/backend", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}