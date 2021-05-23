using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// The AWS Secrets Engine.
    /// </summary>
    public interface IAWSSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic IAM AWS credential based on the named role.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AWSCredentials" /> as the data.
        /// </returns>
        Task<Secret<AWSCredentials>> GetCredentialsAsync(string awsRoleName, string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a dynamic IAM AWS credential  with an STS token based on the named role.
        /// The TTL will be 3600 seconds (one hour).
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="timeToLive"><para>[optional]</para>
        /// Time to live. Defaults to 1 hour</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AWSCredentials" /> as the data.
        /// </returns>
        Task<Secret<AWSCredentials>> GenerateSTSCredentialsAsync(string awsRoleName, string timeToLive = "1h", string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint lists all existing roles in the secrets engine.
        /// </summary>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of role names.</returns>
        Task<Secret<ListInfo>> ReadAllRolesAsync(string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint queries an existing role by the given name.
        /// If invalid role data was supplied to the role from an earlier version of Vault, 
        /// then it will show up in the response as invalid_data.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of role names.</returns>
        Task<Secret<AWSRoleModel>> ReadRoleAsync(string awsRoleName, string awsMountPoint = null, string wrapTimeToLive = null);
    }
}