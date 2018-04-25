using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.SystemBackend.Plugin
{
    /// <summary>
    /// Plugin interface.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Reloads mounted plugin backends.
        /// </summary>
        /// <param name="plugin">The name of the plugin to reload, as registered in the plugin catalog.</param>
        /// <param name="backendMountPaths">Array of mount paths of the plugin backends to reload.</param>
        /// <returns>Task</returns>
        Task ReloadBackendsAsync(string plugin, IEnumerable<string> backendMountPaths);

        /// <summary>
        /// Gets the list of plugins from the catalog.
        /// </summary>
        /// <returns>The plugin names.</returns>
        Task<Secret<ListInfo>> GetCatalogAsync();

        /// <summary>
        /// Registers a new plugin, or updates an existing one with the supplied name.
        /// </summary>
        /// <param name="pluginConfig">
        /// The plugin configuration.
        /// </param>
        /// <returns>Task.</returns>
        Task RegisterAsync(PluginConfig pluginConfig);

        /// <summary>
        /// Gets the configuration data for the plugin with the given name.
        /// </summary>
        /// <param name="pluginName"><para>[required]</para>
        /// Specifies the name of the plugin to retrieve. 
        /// </param>
        /// <returns></returns>
        Task<Secret<PluginConfig>> GetConfigAsync(string pluginName);

        /// <summary>
        /// Removes the plugin from the catalog.
        /// </summary>
        /// <param name="pluginName">The plugin to be removed.</param>
        /// <returns>Task.</returns>
        Task UnregisterAsync(string pluginName);
    }
}