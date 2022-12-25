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
        /// This endpoint configures the root IAM credentials to communicate with AWS. 
        /// There are multiple ways to pass root IAM credentials to the Vault server, 
        /// specified below with the highest precedence first. 
        /// If credentials already exist, this will overwrite them.
        /// The official AWS SDK is used for sourcing credentials from env vars, 
        /// shared files, or IAM/ECS instances.
        /// 
        /// Static credentials provided to the API as a payload
        /// Credentials in the AWS_ACCESS_KEY, AWS_SECRET_KEY, and AWS_REGION environment 
        /// variables on the server
        /// Shared credentials files
        /// Assigned IAM role or ECS task role credentials
        /// 
        /// At present, this endpoint does not confirm that the provided AWS credentials are 
        /// valid AWS credentials with proper permissions.
        /// </summary>
        /// <param name="configureRootIAMCredentialsModel"><para>[required]</para>
        /// The config object.</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task
        /// </returns>
        Task ConfigureRootIAMCredentialsAsync(ConfigureRootIAMCredentialsModel configureRootIAMCredentialsModel, string awsMountPoint = null);

        /// <summary>
        /// This endpoint allows you to read non-secure values that have been configured in the config/root endpoint.
        /// In particular, the secret_key parameter is never returned.
        /// </summary>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The root config
        /// </returns>
        Task<Secret<RootIAMCredentialsConfigModel>> GetRootIAMCredentialsConfigAsync(string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// When you have configured Vault with static credentials, 
        /// you can use this endpoint to have Vault rotate the access key it used. 
        /// Note that, due to AWS eventual consistency, after calling this endpoint, 
        /// subsequent calls from Vault to AWS may fail for a few seconds until 
        /// AWS becomes consistent again.
        /// In order to call this endpoint, Vault's AWS access key MUST be the only 
        /// access key on the IAM user; otherwise, generation of a new access key will fail. 
        /// Once this method is called, Vault will now be the only entity that 
        /// knows the AWS secret key is used to access AWS.
        /// </summary>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The new access key Vault uses is returned by this operation.
        /// </returns>
        Task<Secret<RotateRootIAMCredentialsResponseModel>> RotateRootIAMCredentialsAsync(string awsMountPoint = null);

        /// <summary>
        /// This endpoint configures lease settings for the AWS secrets engine.
        /// </summary>
        /// <param name="leaseConfigModel"><para>[required]</para>
        /// The config object.</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task
        /// </returns>
        Task ConfigureLeaseAsync(AWSLeaseConfigModel leaseConfigModel, string awsMountPoint = null);

        /// <summary>
        /// This endpoint returns the current lease settings for the AWS secrets engine.
        /// </summary>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The root config
        /// </returns>
        Task<Secret<AWSLeaseConfigModel>> GetLeaseConfigAsync(string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint creates or updates the role with the given name. 
        /// If a role with the name does not exist, it will be created. 
        /// If the role exists, it will be updated with the new attributes.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsRoleModel"><para>[required]</para>
        /// The role object.</param>
        /// <param name="awsMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task
        /// </returns>
        Task WriteRoleAsync(string awsRoleName, CreateAWSRoleModel awsRoleModel, string awsMountPoint = null);

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
        /// Deletes a role
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// Name of the role.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.AWS" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <returns>The task</returns>
        Task DeleteRoleAsync(string roleName, string mountPoint = null);

        /// <summary>
        /// Generates a dynamic IAM AWS credential based on the named role.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="roleARN">
        /// The ARN of the role to assume if credential_type on the Vault role is assumed_role. 
        /// Must match one of the allowed role ARNs in the Vault role. 
        /// Optional if the Vault role only allows a single AWS role ARN; required otherwise.
        /// </param>
        /// <param name="roleSessionName">
        /// The role session name to attach to the assumed role ARN. 
        /// Limited to 64 characters; if exceeded, the assumed role ARN will be truncated to 64 characters. 
        /// If not provided, then it will be generated dynamically by default.
        /// </param>
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
        Task<Secret<AWSCredentials>> GetCredentialsAsync(string awsRoleName, string roleARN = null, string roleSessionName = null, string awsMountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a dynamic IAM AWS credential  with an STS token based on the named role.
        /// The TTL will be 3600 seconds (one hour).
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="roleARN">
        /// The ARN of the role to assume if credential_type on the Vault role is assumed_role. 
        /// Must match one of the allowed role ARNs in the Vault role. 
        /// Optional if the Vault role only allows a single AWS role ARN; required otherwise.
        /// </param>
        /// <param name="roleSessionName">
        /// The role session name to attach to the assumed role ARN. 
        /// Limited to 64 characters; if exceeded, the assumed role ARN will be truncated to 64 characters. 
        /// If not provided, then it will be generated dynamically by default.
        /// </param>
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
        Task<Secret<AWSCredentials>> GenerateSTSCredentialsAsync(string awsRoleName, string roleARN = null, string roleSessionName = null, string timeToLive = "1h", string awsMountPoint = null, string wrapTimeToLive = null);
    }
}