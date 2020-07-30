# VaultSharp

The most comprehensive cross-platform .NET Library for HashiCorp's Vault - A Secret Management System.

**VaultSharp NuGet:** [![NuGet](https://img.shields.io/nuget/dt/VaultSharp.svg?style=flat)](https://www.nuget.org/packages/VaultSharp)

**VaultSharp Latest Documentation:** Inline Below and also at: http://rajanadar.github.io/VaultSharp/

**VaultSharp Questions/Clarifications:** [Ask on Stack Overflow with the tag vaultsharp](https://stackoverflow.com/questions/tagged/vaultsharp)

**VaultSharp Gitter Lobby:** [Gitter Lobby](https://gitter.im/rajanadar-VaultSharp/Lobby)

**Older VaultSharp 0.6.x Documentation:** [0.6.x Docs](https://github.com/rajanadar/VaultSharp/blob/master/README-0.6.x.md)

**Report Issues/Feedback:** [Create a VaultSharp GitHub issue](https://github.com/rajanadar/VaultSharp/issues/new)

**Contributing Guidlines:** [VaultSharp Contribution Guidelines](https://github.com/rajanadar/VaultSharp/blob/master/CONTRIBUTING.MD)

[![NuGet](https://img.shields.io/nuget/dt/VaultSharp.svg?style=flat)](https://www.nuget.org/packages/VaultSharp)	
[![Join the chat at https://gitter.im/rajanadar-VaultSharp/Lobby](https://badges.gitter.im/rajanadar-VaultSharp/Lobby.svg)](https://gitter.im/rajanadar-VaultSharp/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)	
[![License](https://img.shields.io/:license-apache%202.0-brightgreen.svg)](http://www.apache.org/licenses/LICENSE-2.0.html)	
[![Build status](https://ci.appveyor.com/api/projects/status/aldh4a6n2t7hthdv?svg=true)](https://ci.appveyor.com/project/rajanadar/vaultsharp)

### What is VaultSharp?

- VaultSharp is a .NET Standard 2.0, 1.3 and .NET 4.5 based cross-platform C# Library that can be used in any .NET application to interact with Hashicorp's Vault.
- The Vault system is a secret management system built as an Http Service by Hashicorp.

VaultSharp has been re-designed ground up, to give a structured user experience across the various auth methods, secrets engines & system apis.
Also, the Intellisense on IVaultClient class should help. I have tried to add a lot of documentation.

### Give me a quick snippet for use!

- Add a Nuget reference to VaultSharp as follows `Install-Package VaultSharp -Version <latest_version>`
- Instantiate a IVaultClient as follows:

```cs
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod = new TokenAuthMethodInfo("MY_VAULT_TOKEN");

// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// Use client to read a key-value secret.
Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("secret-name");

// Generate a dynamic Consul credential
Secret<ConsulCredentials> consulCreds = await vaultClient.V1.Secrets.Consul.GetCredentialsAsync(consulRole, consulMount);
string consulToken = consulCredentials.Data.Token;
```

### Gist of the features

 * VaultSharp supports 
   - All the Auth Methods for Logging  into Vault. (AppRole, AWS, Azure, GitHub, Google Cloud, JWT/OIDC, Kubernetes, LDAP, Okta, RADIUS, TLS, Tokens & UserPass)
   - All the secret engines to get dynamic credentials. (AD, AWS EC2 and IAM, Consul, Cubbyhole, Databases, Google Cloud, Key-Value, Nomad, PKI, RabbitMQ, SSH and TOTP)
   - Several system APIs including enterprise vault apis
 * You can also bring your own "Auth Method" by providing a custom delegate to fetch a token from anywhere.
 * VaultSharp has first class support for Consul engine.
 * KeyValue engine supports both v1 and v2 apis.
 * Abundant intellisense.
 * Provides hooks into http-clients to set custom proxy settings etc.

### VaultSharp - Supported .NET Platforms

VaultSharp is built on **.NET Standard 1.3** & **.NET Standard 2.0** & **.NET Framework 4.5**. This makes it highly compatible and cross-platform.

The following platforms are supported due to that.

- .NET Core 1.0 and above including .NET Core 2.0 and .NET Core 3.0
- .NET Framework 4.5 and above
- Mono 4.6 and above
- Xamarin.iOS 10.0 and above
- Xamarin Mac 3.0 and above
- Xamarin.Android 7.0 and above
- UWP 10.0 and above

Source: https://github.com/dotnet/standard/blob/master/docs/versions.md

### VaultSharp and Consul Support

- VaultSharp supports dynamic Consul credential generation.
- Please look at the API usage in the 'Consul' section of 'Secrets Engines' below, to see all the Consul related methods in action.

### VaultSharp and Automatic Built-in Client Side failover

* VaultSharp DOES NOT support built-in client-side failover either by supporting multiple endpoint URI's or by supporting roundrobin DNS.
* I repeat, it DOES NOT.
* It works off a single URL that you provide. Any sort of fail-over etc. needs to be done by you.
* You are free to instantiate a new instance of VaultClient with a different URI.

### Auth Methods

- VaultSharp supports all authentication methods supported by the Vault Service
- Here is a sample to instantiate the vault client with each of the authentication backends.

#### AliCloud Auth Method

```cs
// setup the AliCloud based auth to get the right token.

IAuthMethodInfo authMethod = new AliCloudAuthMethodInfo(roleName, base64EncodedIdentityRequestUrl, base64EncodedIdentityRequestHeaders);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the AliCloud jwt
```

#### App Role Auth Method

```cs
// setup the AppRole based auth to get the right token.

IAuthMethodInfo authMethod = new AppRoleAuthMethodInfo(roleId, secretId);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the app role and secret id.
```

#### AWS Auth Method

AWS Auth method has 2 flavors. An EC2 way and an IAM way. Here are examples for both.

##### AWS Auth Method - EC2

```cs
// setup the AWS-EC2 based auth to get the right token.

IAuthMethodInfo authMethod = new EC2AWSAuthMethodInfo(pkcs7, null, null, nonce, roleName);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the aws-ec2 role
```

```cs
// setup the AWS-EC2 based auth to get the right token.

IAuthMethodInfo authMethod = new EC2AWSAuthMethodInfo(null, identity, signature, nonce, roleName);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the aws-ec2 role
```

##### AWS Auth Method - IAM

```cs
// setup the AWS-IAM based auth to get the right token.

// Step 1: Pull the following NuGet Packages

// 1. AWSSDK.Core
// 2. AWSSDK.SecurityToken

// Step 2: Boiler-plate code to generate the Signed AWS STS Headers.

var amazonSecurityTokenServiceConfig = new AmazonSecurityTokenServiceConfig();

// If you are running VaultSharp on a real EC2 instance, use the following line of code.
// var awsCredentials = new InstanceProfileAWSCredentials();

// If you are running VaultSharp on a non-EC2 instance like local dev boxes or non-AWS environment, use the following line of code.

AWSCredentials awsCredentials = new StoredProfileAWSCredentials(); // picks up the credentials from your profile.
// AWSCredentials awsCredentials = new BasicAWSCredentials(accessKey: "YOUR_ACCESS_KEY", secretKey: "YOUR_SECRET_KEY"); // explicit credentials

var iamRequest = GetCallerIdentityRequestMarshaller.Instance.Marshall(new GetCallerIdentityRequest());

iamRequest.Endpoint = new Uri(amazonSecurityTokenServiceConfig.DetermineServiceURL());
iamRequest.ResourcePath = "/";

iamRequest.Headers.Add("User-Agent", "https://github.com/rajanadar/vaultsharp/0.11.1000");
iamRequest.Headers.Add("X-Amz-Security-Token", awsCredentials.GetCredentials().Token);
iamRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

new AWS4Signer().Sign(iamRequest, amazonSecurityTokenServiceConfig, new RequestMetrics(), awsCredentials.GetCredentials().AccessKey, awsCredentials.GetCredentials().SecretKey);

// This is the point, when you have the final set of required Headers.
var iamSTSRequestHeaders = iamRequest.Headers;

// Step 3: Convert the headers into a base64 value needed by Vault.

var base64EncodedIamRequestHeaders = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(iamSTSRequestHeaders)));

// Step 4: Setup the IAM AWS Auth Info.

IAuthMethodInfo authMethod = new IAMAWSAuthMethodInfo(nonce, roleName, base64EncodedIamRequestHeaders);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the aws-iam role
```

#### Azure Auth Method

```cs
// setup the Azure based auth to get the right token.

IAuthMethodInfo authMethod = new AzureAuthMethodInfo(roleName, jwt);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the azure jwt
```

#### CloudFoundry Auth Method

```cs
// setup the CloudFoundry based auth to get the right token.

IAuthMethodInfo authMethod = new CloudFoundryAuthMethodInfo(roleName, instanceCertContent, instanceKeyContent);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the CloudFoundry jwt
```

##### CloudFoundry Signature Creation

 - VaultSharp also provides a helper class to generate on-demand CloudFoundry signature.
 - Use the ```CloudFoundrySignatureProvider``` class as follows

```cs
var signing_time = CloudFoundrySignatureProvider.GetFormattedSigningTime(DateTime.UtcNow);
var signature = CloudFoundrySignatureProvider.GetSignature(signingTime, cfInstanceCertContent, roleName, cfInstanceKeyContent);
```

#### GitHub Auth Method

```cs
IAuthMethodInfo authMethod = new GitHubAuthMethodInfo(personalAccessToken);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the github token.
```

#### Google Cloud Auth Method

```cs
// setup the Google Cloud based auth to get the right token.

IAuthMethodInfo authMethod = new GoogleCloudAuthMethodInfo(roleName, jwt);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the Google Cloud jwt
```

#### JWT/OIDC Auth Method

```cs
// setup the JWT/OIDC based auth to get the right token.

IAuthMethodInfo authMethod = new JWTAuthMethodInfo(roleName, jwt);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the jwt
```

#### Kubernetes Auth Method

```cs
// setup the Kubernetes based auth to get the right token.

IAuthMethodInfo authMethod = new KubernetesAuthMethodInfo(roleName, jwt);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the Kubernetes jwt
```

#### LDAP Authentication Backend

##### LDAP Authentication Login Method

```cs
IAuthMethodInfo authMethod = new LDAPAuthMethodInfo(userName, password);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the LDAP username and password.
```

##### LDAP Auth Backend - Groups and User management

 - You can use the methods to CRUD LDAP groups and users now.

```cs
await _authenticatedVaultClient.V1.Auth.LDAP.WriteGroupAsync(groupName, policies);
await _authenticatedVaultClient.V1.Auth.LDAP.ReadGroupAsync(groupName);
await _authenticatedVaultClient.V1.Auth.LDAP.ReadAllGroupsAsync();
await _authenticatedVaultClient.V1.Auth.LDAP.DeleteGroupAsync(groupName);

await _authenticatedVaultClient.V1.Auth.LDAP.WriteUserAsync(username, policies, groups);
await _authenticatedVaultClient.V1.Auth.LDAP.ReadUserAsync(username);
await _authenticatedVaultClient.V1.Auth.LDAP.ReadAllUsersAsync();
await _authenticatedVaultClient.V1.Auth.LDAP.DeleteUserAsync(username);

```

#### Kerberos Authentication Backend

Requires https://github.com/wintoncode/vault-plugin-auth-kerberos .

```cs
IAuthMethodInfo authMethod = new KerberosAuthMethodInfo(); // uses network credential by default.
// IAuthMethodInfo authMethod = new KerberosAuthMethodInfo(credentials); // use your own ICredentials
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the 
// vault token/policies mapped to the current ActiveDirectory/Kerberos identity.
```

#### OCI Auth Method

```cs

var requestHeaders = new Dictionary<string, object>
{
     {"date", new List<string> { "Fri, 22 Aug 2019 21:02:19 GMT" } },
     {"(request-target)", new List<string> { "get /v1/auth/oci/login/devrole" } },
     {"host", new List<string> { "127.0.0.1" } },
     {"content-type", new List<string> { "application/json" } },
     {"authorization", new List<string> { 
          "Signature algorithm=\"rsa-sha256\",headers=\"date (request-target) host\",keyId=\"ocid1.tenancy.oc1..aaaaaaaaba3pv6wkcr4jqae5f15p2b2m2yt2j6rx32uzr4h25vqstifsfdsq/ocid1.user.oc1..aaaaaaaat5nvwcna5j6aqzjcaty5eqbb6qt2jvpkanghtgdaqedqw3rynjq/73:61:a2:21:67:e0:df:be:7e:4b:93:1e:15:98:a5:b7\",signature=\"GBas7grhyrhSKHP6AVIj/h5/Vp8bd/peM79H9Wv8kjoaCivujVXlpbKLjMPeDUhxkFIWtTtLBj3sUzaFj34XE6YZAHc9r2DmE4pMwOAy/kiITcZxa1oHPOeRheC0jP2dqbTll8fmTZVwKZOKHYPtrLJIJQHJjNvxFWeHQjMaR7M=\",version=\"1\""
        } }
};

IAuthMethodInfo authMethod = new OCIAuthMethodInfo(roleName, requestHeaders);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the OCI entity.
```

#### Okta Auth Method

```cs
IAuthMethodInfo authMethod = new OktaAuthMethodInfo(userName, password);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the Okta username and password.
```

#### RADIUS Auth Method

```cs
IAuthMethodInfo authMethod = new RADIUSAuthMethodInfo(userName, password);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
// vault token/policies mapped to the RADIUS username and password.
```

#### Certificate (TLS) Auth Method

```cs
// Please note that the certificate needs to be in pkcs12 format with a private key.
// Turn your cert + key into pkcs12 format with the following command:

// openssl pkcs12 -export -out Cert.p12 -in your-cert.pem -inkey your-key.pem

var certificate = new X509Certificate2(your-p12-bytes, your-pass);

IAuthMethodInfo authMethod = new CertAuthMethodInfo(certificate);

// Optionally, you can also provide a Certificate Role Name during Auth.
// IAuthMethodInfo authMethod = new CertAuthMethodInfo(certificate, certificateRoleName);

var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
vault token/policies mapped to the client certificate.
```

#### Token Auth Method

##### Token Auth Login Method

```cs
IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
vault token/policies mapped to the vault token.
```

##### Token Creation

 - You can use the ```CreateTokenAsync``` method to create various types of tokens.

```cs
CreateTokenRequest request = new CreateTokenRequest();

// CreateTokenRequest has options to create orphaned tokens, role based tokens etc. with attached policies.
Secret<object> tokenData = await _authenticatedVaultClient.V1.Auth.Token.CreateTokenAsync(request);
```

#### Username and Password Auth Method

```cs
IAuthMethodInfo authMethod = new UserPassAuthMethodInfo(username, password);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the
vault token/policies mapped to the username/password.
```

#### Custom Auth Method - Bring your own Vault Token

- In cases where the Vault Server has a supported Auth backend, not YET supported by VaultSharp, you can use the CustomAuthMethodInfo
- In this approach, you write the delegate logic that gets the token from Vault along with lease renewal info etc.

```cs
// Func<Task<CustomAuthMethodInfo>> getCustomAuthMethodInfoAsync = a custom async method to return the vault token.
IAuthMethodInfo authMethod = new CustomAuthMethodInfo("vault-server-auth-method", getCustomAuthMethodInfoAsync);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// Once VaultSharp evaluates the delegate, VaultSharp can now provide you with the associated lease info for the Token as well.
// authMethod.ReturnedLoginAuthInfo has all the info including the token and renewal info.

``` 

#### App Id Auth Method (DEPRECATED)

- Please note that the app-id auth backend has been deprecated by Vault. They recommend us to use the AppRole backend.
- So VaultSharp doesn't support App Id natively.
- If you are in dire need of the App Id support, please raise an issue.

#### MFA (LEGACY/UNSUPPORTED)

- Please note that this legacy Auth Method is not supported by Vault anymore.
- Instead Vault Enterprise contains a fully-supported MFA system.
- It is significantly more complete and flexible and which can be used throughout Vault's API.
- Please see the _System Backend_ section of the docs for the Enterprise MFA apis.

### Secrets Engines

- VaultSharp supports all secrets engines supported by the Vault Service
- Here is a sample to instantiate the vault client with each of the secrets engine

All of the below examples assume that you have a vault client instance ready. e.g.

```cs
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod = new TokenAuthMethodInfo("MY_VAULT_TOKEN");

// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);
```

#### Active Directory Secrets Engine

##### Retrieving Passwords (offering credentials)

- This method offers the credential information for a given role.

```cs
Secret<ActiveDirectoryCredentials> adCreds = await vaultClient.V1.Secrets.ActiveDirectory.GetCredentialsAsync(role);
string currentPassword = adCreds.Data.CurrentPassword;
```

#### AliCloud Secrets Engine

##### Generate RAM Credentials

 * This endpoint generates dynamic RAM credentials based on the named role.

 ```cs	
Secret<AliCloudCredentials> aliCloudCreds = await vaultClient.V1.Secrets.AliCloud.GetCredentialsAsync(role);

string accessKey = aliCloudCreds.Data.AccessKey;
string secretKey = aliCloudCreds.Data.SecretKey;
string securityToken = aliCloudCreds.Data.SecurityToken;
string expiration = aliCloudCreds.Data.Expiration;
```

#### AWS Secrets Engine

##### Generate IAM Credentials

- This endpoint generates dynamic IAM credentials based on the named role.

```cs
Secret<AWSCredentials> awsCreds = await vaultClient.V1.Secrets.AWS.GetCredentialsAsync(role);

string accessKey = awsCreds.Data.AccessKey;
string secretKey = awsCreds.Data.SecretKey;
string securityToken = awsCreds.Data.SecurityToken;
```

##### Read All Roles

- This endpoint reads all the AWS Roles

```cs
Secret<ListInfo> roles = await vaultClient.V1.Secrets.AWS.ReadAllRolesAsync();
List<string> names = roles.Data;
```

##### Generate IAM Credentials

- This endpoint reads details of one AWS Role.

```cs
Secret<AWSRoleModel> role = await vaultClient.V1.Secrets.AWS.ReadRoleAsync(roleName);
List<string> credTypes = role.Data.CredentialTypes;
```

##### Generate IAM Credentials with STS

- This generates a dynamic IAM credential with an STS token based on the named role.

```cs
Secret<AWSCredentials> awsCreds = await vaultClient.V1.Secrets.AWS.GenerateSTSCredentialsAsync(role, ttl);

string accessKey = awsCreds.Data.AccessKey;
string secretKey = awsCreds.Data.SecretKey;
string securityToken = awsCreds.Data.SecurityToken;
```

#### Azure Secrets Engine

##### Generate dynamic Azure credentials

- This endpoint generates a new service principal based on the named role.

```cs
Secret<AzureCredentials> azureCredentials = await vaultClient.V1.Secrets.Azure.GetCredentialsAsync(roleName);
string clientId = azureCredentials.Data.ClientId;
string clientSecret = azureCredentials.Data.ClientSecret;
```

#### Consul Secrets Engine

- This endpoint generates a dynamic Consul token based on the given role definition.

```cs
// Generate a dynamic Consul credential
Secret<ConsulCredentials> consulCreds = await vaultClient.V1.Secrets.Consul.GetCredentialsAsync(consulRole);
string consulToken = consulCredentials.Data.Token;
```

#### Cubbyhole Secrets Engine

##### Read Secret

- This endpoint retrieves the secret at the specified location.

```cs
Secret<Dictionary<string, object>> secret = await vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath);
Dictionary<string, object> secretValues = secret.Data;
```

##### List Secrets

- This endpoint returns a list of secret entries at the specified location.
- Folders are suffixed with /. The input must be a folder; list on a file will not return a value.
- The values themselves are not accessible via this command.

```cs
Secret<ListInfo> secret = await vaultClient.V1.Secrets.Cubbyhole.ReadSecretPathsAsync(folderPath);
ListInfo paths = secret.Data;
```

##### Create/Update Secret

- This endpoint stores a secret at the specified location.

```cs
var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
await vaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath, value);
```

##### Delete Secret

- This endpoint deletes the secret at the specified location.

```cs
await vaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretPath);
```

#### Databases Secrets Engine

##### Generate dynamic DB credentials

- This endpoint generates a new set of dynamic credentials based on the named role.

```cs
Secret<UsernamePasswordCredentials> dbCreds = await vaultClient.V1.Secrets.Database.GetCredentialsAsync(role);
string username = dbCreds.Data.Username;
string password = dbCreds.Data.Password;
```

##### Create, Read and Delete Database Roles (please see next section for static db roles)

- These endpoints manage the creation, reading and deletion of DB roles.

```cs
await vaultClient.V1.Secrets.Database.CreateRoleAsync(roleName, roleRequest);

await vaultClient.V1.Secrets.Database.ReadRoleAsync(roleName);

await vaultClient.V1.Secrets.Database.ReadAllRolesAsync();

await vaultClient.V1.Secrets.Database.DeleteRoleAsync(roleName);
```

##### Create, Read and Delete Static Database Roles

- These endpoints manage the creation, reading and deletion of static DB roles.

```cs
await vaultClient.V1.Secrets.Database.CreateStaticRoleAsync(roleName, roleRequest);

await vaultClient.V1.Secrets.Database.ReadStaticRoleAsync(roleName);

await vaultClient.V1.Secrets.Database.ReadAllStaticRolesAsync();

await vaultClient.V1.Secrets.Database.DeleteStaticRoleAsync(roleName);
```

##### Generate Static DB credentials

- This endpoint generates a new set of static credentials based on the named role.

```cs
Secret<StaticCredentials> dbCreds = await vaultClient.V1.Secrets.Database.GetStaticCredentialsAsync(role);
```

##### Rotate static DB credentials

- This endpoint rotates the static credentials on demand.

```cs
await vaultClient.V1.Secrets.Database.RotateStaticCredentialsAsync(role);
```

#### Google Cloud Secrets Engine

##### Generate Secret (IAM Service Account Creds): OAuth2 Access Token

- Generates an OAuth2 token with the scopes defined on the roleset. This OAuth access token can be used in GCP API calls

```cs
Secret<GoogleCloudOAuth2Token> oauthSecret = await vaultClient.V1.Secrets.GoogleCloud.GetOAuth2TokenAsync(roleset);
string token = oauthSecret.Data.Token;
```

##### Generate Secret (IAM Service Account Creds): Service Account Key

- Generates a service account key.

```cs
Secret<GoogleCloudServiceAccountKey> privateKeySecret = await vaultClient.V1.Secrets.GoogleCloud.GenerateServiceAccountKeyAsync(roleset, keyAlgorithm, privateKeyType);
string privateKeyData = privateKeySecret.Data.Base64EncodedPrivateKeyData;
```

#### Google Cloud KMS Secrets Engine

##### Encrypt, Decrypt, ReEncrypt, Sign & Verify

```cs
await vaultClient.V1.Secrets.GoogleCloudKMS.EncryptAsync(keyName, requestOptions);
await vaultClient.V1.Secrets.GoogleCloudKMS.DecryptAsync(keyName, requestOptions);
await vaultClient.V1.Secrets.GoogleCloudKMS.ReEncryptAsync(keyName, requestOptions);
await vaultClient.V1.Secrets.GoogleCloudKMS.SignAsync(keyName, requestOptions);
await vaultClient.V1.Secrets.GoogleCloudKMS.VerifyAsync(keyName, requestOptions);
```

#### Key Value Secrets Engine

- VaultSharp supports both v1 and v2 of the Key Value Secrets Engine.
- Here are examples for both.

##### Key Value Secrets Engine - V1

###### Create/Update Secret

- This endpoint stores a secret at the specified location.
- If the value does not yet exist, the calling token must have an ACL policy granting the create capability.
- If the value already exists, the calling token must have an ACL policy granting the update capability.

```cs
var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
var writtenValue = await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(secretPath, value);
```

###### Read Secret

- Reads the secret at the specified location returning data.

```cs
// Use client to read a v1 key-value secret.
Secret<Dictionary<string, object>> kv1Secret = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("v1-secret-name");
Dictionary<string, object> dataDictionary = kv1Secret.Data;
```

###### List Secrets

- This endpoint returns a list of key names at the specified location.
- Folders are suffixed with /. The input must be a folder; list on a file will not return a value.
- Note that no policy-based filtering is performed on keys; do not encode sensitive information in key names.
- The values themselves are not accessible via this command.

```cs
Secret<ListInfo> secret = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync(path);
ListInfo paths = secret.Data;
```

###### Delete Secret

- This endpoint deletes the secret at the specified location.

```cs
await vaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(secretPath);
```

##### Key Value Secrets Engine - V2

###### Create/Update Secret

- This endpoint stores a secret at the specified location.
- If the value does not yet exist, the calling token must have an ACL policy granting the create capability.
- If the value already exists, the calling token must have an ACL policy granting the update capability.

```cs
var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
var writtenValue = await vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(secretPath, value, checkAndSet);
```

###### Read Secret

- Reads the secret at the specified location returning data and metadata.

```cs
// Use client to read a v2 key-value secret.
Secret<Dictionary<string, object>> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("v2-secret-name");
Dictionary<string, object> dataDictionary = kv2Secret.Data;
```

###### Read Metadata

- Reads the secret metadata at the specified location returning.

```cs
Secret<FullSecretMetadata> kv2SecretMetadata = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync("v1-secret-name");
```

###### List Secrets

- This endpoint returns a list of key names at the specified location.
- Folders are suffixed with /. The input must be a folder; list on a file will not return a value.
- Note that no policy-based filtering is performed on keys; do not encode sensitive information in key names.
- The values themselves are not accessible via this command.

```cs
Secret<ListInfo> secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync(path);
ListInfo paths = secret.Data;
```

###### Delete Secret

 - This endpoint issues a soft delete of the secret's latest version at the specified location. 
 - This marks the version as deleted and will stop it from being returned from reads, but the underlying data will not be removed.
 - A delete can be undone using the undelete method.

```cs
await vaultClient.V1.Secrets.KeyValue.V2.DeleteSecretAsync(secretPath);
```

###### Delete Secret Versions

 - This endpoint issues a soft delete of the specified versions of the secret.
 - This marks the versions as deleted and will stop them from being returned from reads, but the underlying data will not be removed.
 - A delete can be undone using the undelete method.

```cs
await vaultClient.V1.Secrets.KeyValue.V2.DeleteSecretVersionsAsync(secretPath, versions);
```

###### Undelete Secret Versions

 - Undeletes the data for the provided version and path in the key-value store.
 - This restores the data, allowing it to be returned on get requests.

```cs
await vaultClient.V1.Secrets.KeyValue.V2.UndeleteSecretVersionsAsync(secretPath, versions);
```

###### Destroy Secret

- This endpoint destroys the secret at the specified location for the given versions.

```cs
await vaultClient.V1.Secrets.KeyValue.V2.DestroySecretAsync(secretPath, new List<int> { 1, 2 });
```

###### Delete Secret Metadata and all versions

 * This endpoint permanently deletes the key metadata and all version data for the specified key. 
 * All version history will be removed.

 ```cs	
await vaultClient.V1.Secrets.KeyValue.V2.DeleteMetadataAsync(secretPath);
```

#### Identity Secrets Engine

##### Generate a Signed ID Token

- Use this endpoint to generate a signed ID (OIDC) token.

```cs
Secret<IdentityToken> token = await vaultClient.V1.Secrets.Identity.GetTokenAsync(roleName);
string clientId = token.Data.ClientId;
string token = token.Data.Token;
```

##### Introspect a signed ID Token

- This endpoint can verify the authenticity and active state of a signed ID token.

```cs
Secret<bool> activeResponse = await vaultClient.V1.Secrets.Identity.IntrospectTokenAsync(token, clientId);
bool active = activeResponse.Data;
```

#### KMIP Secrets Engine

##### Generate dynamic credentials

- Create a new client certificate tied to the given role and scope.

```cs
Secret<KMIPCredentials> kmipCredentials = await vaultClient.V1.Secrets.KMIP.GetCredentialsAsync(scopeName, roleName);
string certificateContent = kmipCredentials.Data.CertificateContent;
string privateKeyContent = kmipCredentials.Data.PrivateKeyContent;
```

#### MongoDBAtlas Secrets Engine

##### Generate dynamic credentials

- Generates a dynamic MongoDBAtlas creds based on the given role definition.

```cs
Secret<MongoDBAtlasCredentials> creds = await vaultClient.V1.Secrets.MongoDBAtlas.GetCredentialsAsync(name);
string privateKey = creds.Data.PrivateKey;
string publicKey = nomadCredentials.Data.PublicKey;
```

#### Nomad Secrets Engine

##### Generate dynamic credentials

- Generates a dynamic Nomad token based on the given role definition.

```cs
Secret<NomadCredentials> nomadCredentials = await vaultClient.V1.Secrets.Nomad.GetCredentialsAsync(roleName);
string accessorId = nomadCredentials.Data.AccessorId;
string secretId = nomadCredentials.Data.SecretId;
```

#### OpenLDAP Secrets Engine

##### Generate static credentials

 - This endpoint offers the credential information for a given static-role.

```cs
Secret<StaticCredentials> credentials = await vaultClient.V1.Secrets.OpenLDAP.GetStaticCredentialsAsync(roleName);
string username = credentials.Data.Username;
string password = credentials.Data.Password;
```

#### PKI (Cerificates) Secrets Engine

##### Generate credentials

```cs
var certificateCredentialsRequestOptions = new CertificateCredentialsRequestOptions { // initialize };
Secret<CertificateCredentials> certSecret = await vaultClient.V1.Secrets.PKI.GetCredentialsAsync(pkiRoleName, certificateCredentialsRequestOptions);

string privateKeyContent = certSecret.Data.PrivateKeyContent;
```

##### Revoke Certificate

```cs
Secret<RevokeCertificateResponse> revoke = await vaultClient.V1.Secrets.PKI.RevokeCertificateAsync(serialNumber);
long revocationTime = revoke.Data.RevocationTime;
```

##### Tidy up Certificate Storage

```cs
var request = new CertificateTidyRequest { TidyCertStore = false, TidyRevokedCerts = true };
await vaultClient.V1.Secrets.PKI.TidyAsync(request);
```

##### Read CA Certificate

 - This endpoint retrieves the CA certificate in raw DER-encoded form. 
 - The CA certificate can be returned in PEM or DER format.
 - This is an unauthenticated endpoint.

```cs
var caCert = await vaultClient.V1.Secrets.PKI.ReadCACertificateAsync(CertificateFormat.pem, mountpoint);
Assert.NotNull(caCert.CertificateContent);
```

#### RabbitMQ Secrets Engine

##### Generate dynamic DB credentials

- This endpoint generates a new set of dynamic credentials based on the named role.

```cs
Secret<UsernamePasswordCredentials> secret = await vaultClient.V1.Secrets.RabbitMQ.GetCredentialsAsync(role);
string username = secret.Data.Username;
string password = secret.Data.Password;
```

#### SSH Secrets Engine

##### Generate SSH credentials

- This endpoint creates credentials for a specific username and IP with the parameters defined in the given role.

```cs
Secret<SSHCredentials> sshCreds = await vaultClient.V1.Secrets.SSH.GetCredentialsAsync(role, ipAddress, username);
string sshKey = sshCreds.Data.Key;
```

##### SSH key signing

 * This endpoint signs an SSH public key based on the supplied parameters, subject to the restrictions contained in the role named in the endpoint.

 ```cs	
SignKeyRequest request = new SignKeyRequest { PublicKey = "ipsem" };
Secret<SignedKeyResponse> signedKey = await vaultClient.V1.Secrets.SSH.SignKeyAsync(roleName, request);
string signedKey = signedKey.Data.SignedKey;
```

#### TOTP Secrets Engine

##### Generate Code

This endpoint generates a new time-based one-time use password based on the named key.

```cs
Secret<TOTPCode> totpSecret = await vaultClient.V1.Secrets.TOTP.GetCodeAsync(keyName);
string code = totpSecret.Data.Code;
```

##### Validate Code

This endpoint validates a time-based one-time use password generated from the named key.

```cs
Secret<TOTPCodeValidity> totpValidity = await vaultClient.V1.Secrets.TOTP.ValidateCodeAsync(keyName, code);
bool valid = totpValidity.Data.Valid;
```

##### Create TOTP Key

This endpoint creates or updates a key definition.
You can create both Vault based or non-vault based keys.

```cs

TOTPCreateKeyRequest request = new TOTPCreateKeyRequest
{
 Issuer = "Google",
 AccountName = "scooby@gmail.com",
 KeyGenerationOption = new TOTPVaultBasedKeyGeneration { // specific stuff }
 // for non-vault based, use new TOTPNonVaultBasedKeyGeneration { // specific stuff }
};

Secret<TOTPCreateKeyResponse> response = await vaultClient.V1.Secrets.TOTP.CreateKeyAsync(keyName, request);
var barcode = response.Data.Barcode;
```

##### Read Key

This endpoint queries the key definition.

```cs
Secret<TOTPKey> key = await vaultClient.V1.Secrets.TOTP.ReadKeyAsync(keyName);
```

##### Read all Keys

This endpoint returns a list of available keys. Only the key names are returned, not any values.

```cs
Secret<ListInfo> keys = await vaultClient.V1.Secrets.TOTP.ReadAllKeysAsync();
```

##### Delete Key

This endpoint deletes the key definition.

```cs
await vaultClient.V1.Secrets.TOTP.DeleteKeyAsync(keyName);
```

#### Transform Secrets Engine

##### Encode Method

###### Encode Single Item

```cs

var encodeOptions = new EncodeRequestOptions { Value = "ipsem" };
Secret<EncodedResponse> response = await _authenticatedVaultClient.V1.Secrets.Transit.EncodeAsync(roleName, encodeOptions);
response.Data.EncodedText;

```

###### Encode Batched Items

```cs
var encodeOptions = new EncodeRequestOptions 
{ 
  BatchItems = new List<EncodingItem> { new EncodingItem { Value = "ipsem1" }, new EncodingItem { Value = "ipsem2" } }
};

Secret<EncodedResponse> response = await _authenticatedVaultClient.V1.Secrets.Transit.EncodeAsync(roleName, encodeOptions);
response.Data.BatchResults;
```

##### Decode Method

###### Decode Single Item

```cs
var decodeOptions = new DecodeRequestOptions { Value = "ipsem" };
Secret<DecodedResponse> response = await _authenticatedVaultClient.V1.Secrets.Transit.DecodeAsync(roleName, decodeOptions);
response.Data.DecodedText;
```

###### Decode Batched Item

```cs
var decodeOptions = new DecodeRequestOptions 
{ 
  BatchItems = new List<DecodingItem> { new DecodingItem { Value = "ipsem1" }, new DecodingItem { Value = "ipsem2" } }
};

Secret<DecodedResponse> response = await _authenticatedVaultClient.V1.Secrets.Transit.DecodeAsync(roleName, decodeOptions);
response.Data.BatchResults;
```

#### Transit Secrets Engine

##### Encrypt Method

###### Encrypt Single Item

```cs
var keyName = "test_key";

var context = "context1";
var plainText = "raja";
var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
var encodedContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(context));

var encryptOptions = new EncryptRequestOptions
{
    Base64EncodedPlainText = encodedPlainText,
    Base64EncodedContext = encodedContext,
};

Secret<EncryptionResponse> encryptionResponse = await _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions);
string cipherText = encryptionResponse.Data.CipherText;
```

###### Encrypt Batched Items

```cs
var encryptOptions = new EncryptRequestOptions
{
    BatchedEncryptionItems = new List<EncryptionItem>
    {
        new EncryptionItem { Base64EncodedContext = encodedContext1, Base64EncodedPlainText = encodedPlainText1 },
        new EncryptionItem { Base64EncodedContext = encodedContext2, Base64EncodedPlainText = encodedPlainText2 },
        new EncryptionItem { Base64EncodedContext = encodedContext3, Base64EncodedPlainText = encodedPlainText3 },
    }
};

Secret<EncryptionResponse> encryptionResponse = await _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions);
string firstCipherText = encryptionResponse.Data.BatchedResults.First().CipherText;
```

##### Decrypt Method

###### Decrypt Single Item

```cs
var decryptOptions = new DecryptRequestOptions
{
    CipherText = cipherText,
    Base64EncodedContext = encodedContext,
};

Secret<DecryptionResponse> decryptionResponse = await _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions);
string encodedPlainText = decryptionResponse.Data.Base64EncodedPlainText;
```

###### Decrypt Batched Item

```cs
var decryptOptions = new DecryptRequestOptions
{
    BatchedDecryptionItems = new List<DecryptionItem>
    {
        new DecryptionItem { Base64EncodedContext = encodedContext1, CipherText = cipherText1 },
        new DecryptionItem { Base64EncodedContext = encodedContext2, CipherText = cipherText2 },
        new DecryptionItem { Base64EncodedContext = encodedContext3, CipherText = cipherText3 },
    }
};

Secret<DecryptionResponse> decryptionResponse = await _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions);
string firstEncodedPlainText = decryptionResponse.Data.BatchedResults.First().Base64EncodedPlainText;
```
###### Generate Data Key

```cs
// Generate Data Key
var dataKeyOptions = new DataKeyRequestOptions
{
    Base64EncodedContext = encodedContext,
    Bits = 256, // default
    Nonce = nonce
};

Secret<DataKeyResponse> dataKeyResponse = await _authenticatedVaultClient.V1.Secrets.Transit.GenerateDataKeyAsync(keyName, "plaintext", dataKeyOptions);

var encodedDataKeyPlainText = dataKeyResponse.Data.Base64EncodedPlainText;
var dataKeyCipherText = dataKeyResponse.Data.Base64EncodedPlainText;

```

##### Read all Encryption Keys

```cs
var allKeys = await _authenticatedVaultClient.V1.Secrets.Transit.ReadAllEncryptionKeysAsync();

### System Backend

- The system backend is a default backend in Vault that is mounted at the /sys endpoint.
- This endpoint cannot be disabled or moved, and is used to configure Vault and interact with many of Vault's internal features.

VaultSharp already supports several of the System backend features.

```cs
// vaultClient.V1.System.<method> The method you are looking for.
```

### Can I inject my own HttpClient into VaultSharp?

 - Yes you can.
 - The ```VaultClientSettings``` object takes a ```MyHttpClientProviderFunc``` delegate that can be as follows.
 - Don't worry about setting any vault specific URL, timeout etc. on this http client. VaultSharp will do that.

 ```cs
var settings = new VaultClientSettings("http://localhost:8200", authMethodInfo)
            {
                Namespace = "mynamespace",
                MyHttpClientProviderFunc = handler => new HttpClient(handler)
            };
```

### What is the deal with the Versioning of VaultSharp?

- This library is written for Hashicorp's Vault Service
- The Vault service is evolving constantly and the Hashicorp team is rapidly working on it.
- Because this client library is intended to facilititate the Vault Service operations, this library makes it easier for its consumers to relate to the Vault service it supports.
- Hence a version of 0.11.x denotes that this library will support the Vault 0.11.x Service Apis.
- Tomorrow when Vault Service gets upgraded to x.0.0, this library will be modified accordingly and versioned as x.0.0
- VaultSharp starts at e.g. 2.3.0 matching the Vault Server exactly, and then can go 2.3.0001, 2.3.0002 etc. for bug fixes etc. within 2.3.0 of Vault.
- Another thing to note is that, empirically, VaultSharp and Vault have been amazingly compatible even with great versioning differences. Kudos to the Vault team.

### Can I use it in my PowerShell Automation?

- Absolutely. VaultSharp is a .NET Library.
- This means, apart from using it in your C#, VB.NET, J#.NET and any .NET application, you can use it in PowerShell automation as well.
- Load up the DLL in your PowerShell code and execute the methods. PowerShell can totally work with .NET Dlls.

### All the methods are async. How do I use them synchronously?

- The methods are async as the defacto implementation. The recommended usage.
- However, there are innumerable scenarios where you would continue to want to use it synchronously.
- For all those cases, there are various options available to you.
- There is a lot of discussion around the right usage, avoiding deadlocks etc.
- This library allows you to set the 'continueAsyncTasksOnCapturedContext' option when you initialize the client.
- It is an optional parameter and defaults to 'false'
- Setting it to false, allows you to access the .Result property of the task with reduced/zero deadlock issues.
- There are other ways as well to invoke it synchronously, and I leave it to the users of the library. (Task.Run etc.)
- But please note that as much as possible, use it in an async manner.

### In Conclusion

- If the above documentation doesn't help you, feel free to create an issue or email me. https://github.com/rajanadar/VaultSharp/issues/new

### Happy Coding folks!
