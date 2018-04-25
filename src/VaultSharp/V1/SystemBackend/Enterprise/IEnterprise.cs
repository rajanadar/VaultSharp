using System.Threading.Tasks;
using VaultSharp.V1.Core;

namespace VaultSharp.V1.SystemBackend.Enterprise
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

        /// <summary>
        /// Returns information about the currently installed license.
        /// </summary>
        /// <returns>License.</returns>
        Task<Secret<License>> GetLicenseAsync();

        /// <summary>
        /// Used to install a license into Vault.
        /// </summary>
        /// <param name="licenceText"><para>[required]</para>
        /// The license text.
        /// </param>
        /// <returns>Task.</returns>
        Task InstallLicenseAsync(string licenceText);

        /// <summary>
        /// Gets all the RGP policy names in the system.
        /// </summary>
        /// <returns>
        /// The policy names.
        /// </returns>
        Task<Secret<ListInfo>> GetRGPPoliciesAsync();

        /// <summary>
        /// Gets the rules for the named RGP policy.
        /// </summary>
        /// <param name="policyName">
        /// <para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The rules for the policy.
        /// </returns>
        Task<Secret<RGPPolicy>> GetRGPPolicyAsync(string policyName);

        /// <summary>
        /// Adds or updates the RGP policy.
        /// Once a policy is updated, it takes effect immediately to all associated users.
        /// </summary>
        /// <param name="policy"><para>[required]</para>
        /// The policy to be added or updated.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteRGPPolicyAsync(RGPPolicy policy);

        /// <summary>
        /// Deletes the named RGP policy. This will immediately affect all associated users.
        /// </summary>
        /// <param name="policyName"><para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteRGPPolicyAsync(string policyName);

        /// <summary>
        /// Gets all the EGP policy names in the system.
        /// </summary>
        /// <returns>
        /// The policy names.
        /// </returns>
        Task<Secret<ListInfo>> GetEGPPoliciesAsync();

        /// <summary>
        /// Gets the rules for the named EGP policy.
        /// </summary>
        /// <param name="policyName">
        /// <para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The rules for the policy.
        /// </returns>
        Task<Secret<EGPPolicy>> GetEGPPolicyAsync(string policyName);

        /// <summary>
        /// Adds or updates the EGP policy.
        /// Once a policy is updated, it takes effect immediately to all associated users.
        /// </summary>
        /// <param name="policy"><para>[required]</para>
        /// The policy to be added or updated.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteEGPPolicyAsync(EGPPolicy policy);

        /// <summary>
        /// Deletes the named EGP policy. This will immediately affect all associated users.
        /// </summary>
        /// <param name="policyName"><para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteEGPPolicyAsync(string policyName);
    }
}