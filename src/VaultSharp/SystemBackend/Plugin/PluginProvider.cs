using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.SystemBackend.Plugin
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

        public async Task<Secret<ListInfo>> GetCatalogAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/plugins/catalog?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RegisterAsync(PluginConfig pluginConfig)
        {
            var requestData = new
            {
                sha_256 = pluginConfig.Sha256, // raja todo: check this https://www.vaultproject.io/api/system/plugins-catalog.html
                command = pluginConfig.Command
            };

            await _polymath.MakeVaultApiRequest("v1/sys/plugins/catalog/" + pluginConfig.Name, HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<PluginConfig>> GetConfigAsync(string pluginName)
        {
            return await _polymath.MakeVaultApiRequest<Secret<PluginConfig>>("v1/sys/plugins/catalog/" + pluginName, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnregisterAsync(string pluginName)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/plugins/catalog/" + pluginName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}