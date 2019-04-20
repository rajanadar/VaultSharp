using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.AWS
{
    /// <summary>
    /// Represents the login information for the IAM AWS Authentication backend.
    /// </summary>
    public class IAMAWSAuthMethodInfo : AbstractAWSAuthMethodInfo
    {
        /// <summary>
        /// Gets the AWS Auth method type.
        /// </summary>
        /// <value>
        /// The AWS auth type.
        /// </value>
        [JsonIgnore]
        public override AWSAuthMethodType AWSAuthMethodType { get { return AWSAuthMethodType.IAM; } }

        /// <summary>
        /// Gets the HTTP method used in the signed request. 
        /// Currently only POST is supported, but other methods may be supported in the future. 
        /// This is required when using the iam auth method.
        /// </summary>
        /// <value>
        /// The HTTP method used in the signed request.
        /// </value>
        [JsonProperty("iam_http_request_method")]
        public string HttpRequestMethod { get; }

        /// <summary>
        /// Gets the Base64-encoded HTTP URL used in the signed request. 
        /// Most likely just aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8= (base64-encoding of https://sts.amazonaws.com/) as most 
        /// requests will probably use POST with an empty URI.
        /// This is required when using the iam auth method.
        /// </summary>
        /// <value>
        /// The Request Url used in the signed request.
        /// </value>
        [JsonProperty("iam_request_url")]
        public string RequestUrl { get; }

        /// <summary>
        /// Gets the Base64-encoded HTTP body used in the signed request. 
        /// Most likely QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ== which is the base64 encoding of 
        /// Action=GetCallerIdentity&Version=2011-06-15
        /// This is required when using the iam auth method.
        /// </summary>
        /// <value>
        /// The Request Url used in the signed request.
        /// </value>
        [JsonProperty("iam_request_body")]
        public string RequestBody { get; }

        /// <summary>
        /// Gets the Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of string 
        /// values (though the length of that array will probably only be one). 
        /// If the iam_server_id_header_value is configured in Vault for the aws auth mount, 
        /// then the headers must include the X-Vault-AWS-IAM-Server-ID header, 
        /// its value must match the value configured, and the header must be included in the signed headers. 
        /// This is required when using the iam auth method.
        /// </summary>
        /// <value>
        /// The Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// </value>
        [JsonProperty("iam_request_headers")]
        public string RequestHeaders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAMAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="requestHeaders">
        /// <para>[required]</para>
        /// The Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of string 
        /// values (though the length of that array will probably only be one). 
        /// If the iam_server_id_header_value is configured in Vault for the aws auth mount, 
        /// then the headers must include the X-Vault-AWS-IAM-Server-ID header, 
        /// its value must match the value configured, and the header must be included in the signed headers. 
        /// This is required when using the iam auth method.
        /// Please see <see cref="https://github.com/rajanadar/VaultSharp/blob/master/README.md#aws-auth-method---iam"/> on how to construct this.
        /// </param>
        /// <param name="httpRequestMethod">
        /// <para>[required]</para>
        /// The HTTP method used in the signed request. 
        /// Currently only POST is supported, but other methods may be supported in the future. 
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="requestUrl">
        /// <para>[required]</para>
        /// Base64-encoded HTTP URL used in the signed request. 
        /// Most likely just aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8= (base64-encoding of https://sts.amazonaws.com/) as 
        /// most requests will probably use POST with an empty URI. 
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="requestBody">
        /// <para>[required]</para>
        /// Base64-encoded body of the signed request. Most likely QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ== 
        /// which is the base64 encoding of Action=GetCallerIdentity&Version=2011-06-15.
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-whitelist 
        /// entry and returns the nonce back as part of auth metadata. 
        /// This value should be used with further login requests, to establish client authenticity. 
        /// Clients can choose to set a custom nonce if preferred, in which case, it is recommended 
        /// that clients provide a strong nonce. If a nonce is provided but with an empty value, 
        /// it indicates intent to disable reauthentication. Note that, when disallow_reauthentication option 
        /// is enabled on either the role or the role tag, the nonce holds no significance. 
        /// This is ignored unless using the ec2 auth method.
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.
        /// </param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login
        /// if using the ec2 auth method, or the "friendly name" (i.e., role name or username) 
        /// of the IAM principal authenticated.. 
        /// If a matching role is not found, login fails.
        /// </param>
        public IAMAWSAuthMethodInfo(string requestHeaders, string httpRequestMethod = "POST", string requestUrl = "aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8=", string requestBody = "QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ==", string nonce = null, string roleName = null)
            : this(AuthMethodType.AWS.Type, requestHeaders, httpRequestMethod, requestUrl, requestBody, nonce, roleName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAMAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="requestHeaders">
        /// <para>[required]</para>
        /// The Base64-encoded, JSON-serialized representation of the sts:GetCallerIdentity HTTP request headers. 
        /// The JSON serialization assumes that each header key maps to either a string value or an array of string 
        /// values (though the length of that array will probably only be one). 
        /// If the iam_server_id_header_value is configured in Vault for the aws auth mount, 
        /// then the headers must include the X-Vault-AWS-IAM-Server-ID header, 
        /// its value must match the value configured, and the header must be included in the signed headers. 
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="httpRequestMethod">
        /// <para>[required]</para>
        /// The HTTP method used in the signed request. 
        /// Currently only POST is supported, but other methods may be supported in the future. 
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="requestUrl">
        /// <para>[required]</para>
        /// Base64-encoded HTTP URL used in the signed request. 
        /// Most likely just aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8= (base64-encoding of https://sts.amazonaws.com/) as 
        /// most requests will probably use POST with an empty URI. 
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="requestBody">
        /// <para>[required]</para>
        /// Base64-encoded body of the signed request. Most likely QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ== 
        /// which is the base64 encoding of Action=GetCallerIdentity&Version=2011-06-15.
        /// This is required when using the iam auth method.
        /// </param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-whitelist 
        /// entry and returns the nonce back as part of auth metadata. 
        /// This value should be used with further login requests, to establish client authenticity. 
        /// Clients can choose to set a custom nonce if preferred, in which case, it is recommended 
        /// that clients provide a strong nonce. If a nonce is provided but with an empty value, 
        /// it indicates intent to disable reauthentication. Note that, when disallow_reauthentication option 
        /// is enabled on either the role or the role tag, the nonce holds no significance. 
        /// This is ignored unless using the ec2 auth method.
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.
        /// </param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login
        /// if using the ec2 auth method, or the "friendly name" (i.e., role name or username) 
        /// of the IAM principal authenticated.. 
        /// If a matching role is not found, login fails.
        /// </param>
        public IAMAWSAuthMethodInfo(string mountPoint, string requestHeaders, string httpRequestMethod = "POST", string requestUrl = "aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8=", string requestBody = "QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ==", string nonce = null, string roleName = null)
            : base(mountPoint, nonce, roleName)
        {
            Checker.NotNull(httpRequestMethod, "httpRequestMethod");
            Checker.NotNull(requestUrl, "requestUrl");
            Checker.NotNull(requestBody, "requestBody");
            Checker.NotNull(requestHeaders, "requestHeaders");

            HttpRequestMethod = httpRequestMethod;
            RequestUrl = requestUrl;
            RequestBody = requestBody;
            RequestHeaders = requestHeaders;
        }
    }
}