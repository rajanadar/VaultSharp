using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.Plugin
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

        public async Task ReloadBackendsAsync(string plugin, IEnumerable<string> backendMountPaths, CancellationToken cancellationToken = default)
        {
            var requestData = new PluginReloadRequest
            {
                Plugin = plugin,
                Mounts = backendMountPaths
            };

            await _polymath.MakeVaultApiRequest("v1/sys/plugin/reload/backend", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetCatalogAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/plugins/catalog", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RegisterAsync(PluginConfig pluginConfig, CancellationToken cancellationToken = default)
        {
            var requestData = new PluginRegistrationRequest
            {
                Sha256 = pluginConfig.Sha256, // raja todo: check this https://www.vaultproject.io/api/system/plugins-catalog.html
                Command = pluginConfig.Command
            };

            await _polymath.MakeVaultApiRequest("v1/sys/plugins/catalog/" + pluginConfig.Name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<PluginConfig>> GetConfigAsync(string pluginName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<PluginConfig>>("v1/sys/plugins/catalog/" + pluginName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnregisterAsync(string pluginName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/plugins/catalog/" + pluginName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}