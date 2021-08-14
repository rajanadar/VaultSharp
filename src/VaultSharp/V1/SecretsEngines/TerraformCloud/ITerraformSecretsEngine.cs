using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Terraform
{
    /// <summary>
    /// The Terraform Secrets Engine.
    /// </summary>
    public interface ITerraformSecretsEngine
    {
        /// <summary>
        /// Returns a Terraform Cloud token based on the given role definition. 
        /// For Organization and Team roles, the same API token is returned until 
        /// the token is rotated with rotate-role. 
        /// For User roles, a new token is generated with each request.
        /// </summary>
        /// <param name="roleName">[required]
        ///  Specifies the name of an existing role against which to create this Terraform Cloud token.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Terraform" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Terraform Cloud token</returns>
        Task<Secret<TerraformCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);
    }
}