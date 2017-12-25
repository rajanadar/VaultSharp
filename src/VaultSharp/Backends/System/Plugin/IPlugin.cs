using System.Collections.Generic;
using System.Threading.Tasks;

namespace VaultSharp.Backends.System.Plugin
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
    }
}