using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the AWS EC2 role.
    /// </summary>
    public class AwsEc2AuthenticationRole
    {
        /// <summary>
        /// <para>[required]</para> 
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [JsonIgnore]
        public string RoleName { get; set; }

        /// <summary>
        /// <para>[optional]</para> 
        /// Gets or sets the bound ami identifier.
        /// If set, defines a constraint on the EC2 instances that they should be using the AMI ID 
        /// specified by this parameter.
        /// </summary>
        /// <value>
        /// The bound ami identifier.
        /// </value>
        [JsonProperty("bound_ami_id")]
        public string BoundAmiId { get; set; }

        /// <summary>
        /// <para>[optional]</para> 
        /// Gets or sets the bound account identifier.
        /// If set, defines a constraint on the EC2 instances that the account ID in its 
        /// identity document to match the one specified by this parameter.
        /// </summary>
        /// <value>
        /// The bound account identifier.
        /// </value>
        [JsonProperty("bound_account_id")]
        public string BoundAccountId { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the bound iam role arn.
        /// If set, defines a constraint on the authenticating EC2 instance that it 
        /// must match the IAM role ARN specified by this parameter. 
        /// The value is prefix-matched (as though it were a glob ending in *). 
        /// The configured IAM user or EC2 instance role must be allowed to execute 
        /// the iam:GetInstanceProfile action if this is specified.
        /// </summary>
        /// <value>
        /// The bound iam role arn.
        /// </value>
        [JsonProperty("bound_iam_role_arn")]
        public string BoundIamRoleArn { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the bound iam instance profile arn.
        /// If set, defines a constraint on the EC2 instances to be associated with an IAM instance 
        /// profile ARN which has a prefix that matches the value specified by this parameter. 
        /// The value is prefix-matched (as though it were a glob ending in *).
        /// </summary>
        /// <value>
        /// The bound iam instance profile arn.
        /// </value>
        [JsonProperty("bound_iam_instance_profile_arn")]
        public string BoundIamInstanceProfileArn { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the role tag.
        /// If set, enables the role tags for this role. 
        /// The value set for this field should be the 'key' of the tag on the EC2 instance. 
        /// The 'value' of the tag should be generated using 'role//tag' endpoint. 
        /// Defaults to an empty string, meaning that role tags are disabled.
        /// </summary>
        /// <value>
        /// The role tag.
        /// </value>
        [JsonProperty("role_tag")]
        public string RoleTag { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the time to live.
        /// The TTL period of tokens issued using this role, provided as "1h", where hour is the largest suffix.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the maximum allowed time to live.
        /// The maximum allowed lifetime of tokens issued using this role.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the Policies to be set on tokens issued using this role.
        /// </summary>
        /// <value>
        /// The policies.
        /// </value>
        [JsonProperty("policies")]
        public object Policies { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow instance migration].
        /// If set, allows migration of the underlying instance where the client resides. 
        /// This keys off of pendingTime in the metadata document, so essentially, 
        /// this disables the client nonce check whenever the instance is migrated to a 
        /// new host and pendingTime is newer than the previously-remembered time. 
        /// Use with caution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [allow instance migration]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_instance_migration")]
        public bool AllowInstanceMigration { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [disallow reauthentication].
        /// If set, only allows a single token to be granted per instance ID. 
        /// In order to perform a fresh login, the entry in whitelist for the instance ID 
        /// needs to be cleared using 'auth/aws-ec2/identity-whitelist/' endpoint. 
        /// Defaults to 'false'.
        /// </summary>
        /// <value>
        /// <c>true</c> if [disallow reauthentication]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("disallow_reauthentication")]
        public bool DisallowReauthentication { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsEc2AuthenticationRole"/> class.
        /// </summary>
        public AwsEc2AuthenticationRole()
        {
            RoleTag = string.Empty;
            DisallowReauthentication = false;
        }
    }
}