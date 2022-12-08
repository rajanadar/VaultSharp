using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Amazon.SecurityToken.Model.Internal.MarshallTransformations;
using Newtonsoft.Json;
using System;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace VaultSharp.V1.AuthMethods.AWS
{
    /// <summary>
    /// Adapted version of IAMAWSAuthMethodInfo class
    /// which uses EC2 instance profile to get AWS credentials for Vault Authentication.
    /// Also it dynamically generates AWS header for sts:GetCallerIdentity HTTP request,
    /// since they have only 15 minutes TTL.
    /// </summary>
    public class InstanceProfileAWSAuthMethodInfo : AbstractAWSAuthMethodInfo
    {
        [JsonIgnore]
        private readonly string _headerValue;

        [JsonIgnore]
        private readonly MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

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
        /// <![CDATA[ Action=GetCallerIdentity&Version=2011-06-15 ]]>
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
        public string RequestHeaders => GetRequestHeaders();

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProfileAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
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
        /// which is the base64 encoding of <![CDATA[ Action=GetCallerIdentity&Version=2011-06-15 ]]>.
        /// This is required when using the iam auth method.
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
        /// <param name="headerValue">
        /// Value of X-Vault-AWS-IAM-Server-ID HTTP header which is required by IAM AWS authentication.
        /// </param>
        public InstanceProfileAWSAuthMethodInfo(string mountPoint,
                                                string httpRequestMethod = "POST",
                                                string requestUrl = "aHR0cHM6Ly9zdHMuYW1hem9uYXdzLmNvbS8=",
                                                string requestBody = "QWN0aW9uPUdldENhbGxlcklkZW50aXR5JlZlcnNpb249MjAxMS0wNi0xNQ==",
                                                string roleName = null,
                                                string headerValue = null)
            : base(mountPoint, null, roleName)
        {
            _headerValue = headerValue;
            HttpRequestMethod = httpRequestMethod;
            RequestUrl = requestUrl;
            RequestBody = requestBody;
        }

        private string GetRequestHeaders()
        {
            Console.WriteLine("Getting Headers.");
            return _memoryCache.GetOrCreate("AWSIAMHeaders", (entry) =>
            {
                // Cache headers for 15 minutes, to re-use them if needed.
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                return CreateRequestHeaders();
            });
        }

        private string CreateRequestHeaders()
        {
            Console.WriteLine("Creating Headers.");

            var amazonSecurityTokenServiceConfig = new AmazonSecurityTokenServiceConfig();
            var awsCredentials = new InstanceProfileAWSCredentials();

            IRequest iamRequest = GetCallerIdentityRequestMarshaller.Instance.Marshall(new GetCallerIdentityRequest());

            iamRequest.Endpoint = new Uri(amazonSecurityTokenServiceConfig.DetermineServiceURL());
            iamRequest.ResourcePath = "/";

            ImmutableCredentials immutableAwsCredentials = awsCredentials.GetCredentials();

            iamRequest.Headers.Add("X-Amz-Security-Token", immutableAwsCredentials.Token);
            iamRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            iamRequest.Headers.Add("X-Vault-AWS-IAM-Server-ID", _headerValue);

            AWS4Signer aws4Signer = new AWS4Signer();

            aws4Signer.Sign(iamRequest,
                            amazonSecurityTokenServiceConfig,
                            new RequestMetrics(),
                            immutableAwsCredentials.AccessKey,
                            immutableAwsCredentials.SecretKey);

            string headersJson = JsonConvert.SerializeObject(iamRequest.Headers);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(headersJson));
        }
    }
}