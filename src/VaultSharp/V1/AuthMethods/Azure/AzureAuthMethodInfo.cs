
using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Azure
{
    /// <summary>
    /// Represents the login information for the Azure Authentication backend.
    /// </summary>
    public class AzureAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.Azure;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// [required]
        /// Gets the name of the role against which the login is being attempted.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        [JsonProperty("role")]
        public string RoleName { get; }

        /// <summary>
        /// [required]
        /// Gets the signed JSON Web Token (JWT) from Azure MSI.
        /// </summary>
        /// <value>
        /// The jwt.
        /// </value>
        [JsonProperty("jwt")]
        public string JWT { get; }

        /// <summary>
        /// [optional]
        /// Gets the subscription ID for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("subscription_id")]
        public string SubscriptionId { get; }

        /// <summary>
        /// [optional]
        /// Gets the resource group for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("resource_group_name")]
        public string ResourceGroupName { get; }

        /// <summary>
        /// [optional]
        /// Gets the virtual machine name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata. 
        /// If <see cref="VirtualMachineScaleSetName"/> is provided, this value is ignored.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("vm_name")]
        public string VirtualMachineName { get; }

        /// <summary>
        /// [optional]
        /// Gets the virtual machine scale set name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("vmss_name")]
        public string VirtualMachineScaleSetName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT) from Azure MSI.
        /// </param>
        /// <param name="subscriptionId">
        /// [optional]
        /// The subscription ID for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        /// <param name="resourceGroupName">
        /// [optional]
        /// The resource group for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        /// <param name="virtualMachineName">
        /// [optional]
        /// The virtual machine name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata. 
        /// If <see cref="VirtualMachineScaleSetName"/> is provided, this value is ignored.
        /// </param>
        /// <param name="virtualMachineScaleSetName">
        /// [optional]
        /// The virtual machine scale set name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        public AzureAuthMethodInfo(string roleName, string jwt, string subscriptionId = null, string resourceGroupName = null, string virtualMachineName = null, string virtualMachineScaleSetName = null)
            : this (AuthMethodType.Azure.Type, roleName, jwt, subscriptionId, resourceGroupName, virtualMachineName, virtualMachineScaleSetName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="jwt">
        /// [required]
        /// The signed JSON Web Token (JWT) from Azure MSI.
        /// </param>
        /// <param name="subscriptionId">
        /// [optional]
        /// The subscription ID for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        /// <param name="resourceGroupName">
        /// [optional]
        /// The resource group for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        /// <param name="virtualMachineName">
        /// [optional]
        /// The virtual machine name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata. 
        /// If <see cref="VirtualMachineScaleSetName"/> is provided, this value is ignored.
        /// </param>
        /// <param name="virtualMachineScaleSetName">
        /// [optional]
        /// The virtual machine scale set name for the machine that generated the MSI token. 
        /// This information can be obtained through instance metadata.
        /// </param>
        public AzureAuthMethodInfo(string mountPoint, string roleName, string jwt, string subscriptionId = null, string resourceGroupName = null, string virtualMachineName = null, string virtualMachineScaleSetName = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(jwt, "jwt");

            MountPoint = mountPoint;
            RoleName = roleName;
            JWT = jwt;
            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
            VirtualMachineName = virtualMachineName;
            VirtualMachineScaleSetName = virtualMachineScaleSetName;
        }
    }
}