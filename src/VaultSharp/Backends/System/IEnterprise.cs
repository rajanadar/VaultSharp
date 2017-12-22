using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// Enterprise System backend APIs
    /// </summary>
    public interface IEnterprise
    {
        /// <summary>
        /// Gets the current Control Group configuration.
        /// </summary>
        /// <remarks>
        /// Requires Enterprise Vault.
        /// </remarks>
        /// <returns>Config</returns>
        Task<Secret<ControlGroup>> GetControlGroupConfigAsync();

        /// <summary>
        /// Configures control groups.
        /// </summary>
        /// <remarks>
        /// Requires Enterprise Vault.
        /// </remarks>
        /// <param name="maxTimeToLive">The maximum ttl for a control group wrapping token. This can be provided in seconds or duration (2h).</param>
        /// <returns>Task</returns>
        Task ConfigureControlGroupAsync(string maxTimeToLive);

        /// <summary>
        /// Removes any control group configuration.
        /// </summary>
        /// <remarks>
        /// Requires Enterprise Vault.
        /// </remarks>
        /// <returns>Task</returns>
        Task DeleteControlGroupConfigAsync();

        /// <summary>
        /// Authorizes a control group request.
        /// </summary>
        /// <param name="accessor"><para>[required]</para>
        /// The accessor for the control group wrapping token.</param>
        /// <remarks>
        /// Requires Enterprise Vault.
        /// </remarks>
        /// <returns>Authorization.</returns>
        Task<Secret<ControlGroupRequestStatus>> AuthorizeControlGroupAsync(string accessor);

        /// <summary>
        /// Checks the status of a control group request.
        /// </summary>
        /// <param name="accessor"><para>[required]</para>
        /// The accessor for the control group wrapping token.</param>
        /// <remarks>
        /// Requires Enterprise Vault.
        /// </remarks>
        /// <returns>The status.</returns>
        Task<Secret<ControlGroupRequestStatus>> CheckControlGroupStatusAsync(string accessor);
    }
}