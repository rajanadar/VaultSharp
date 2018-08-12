VaultSharp
==========

A cross-platform .NET Library for HashiCorp's Vault - A Secret Management System.

**Latest release: [0.10.4001](https://www.nuget.org/packages/VaultSharp) [Install-Package VaultSharp -Version 0.10.4001]**

**Latest Documentation:** Inline Below and also at: http://rajanadar.github.io/VaultSharp/

**Older VaultSharp 0.6.x Documentation:** [0.6.x Docs](https://github.com/rajanadar/VaultSharp/blob/master/README-0.6.x.md)

**Report Issues/Feedback:** [Create a VaultSharp GitHub issue](https://github.com/rajanadar/VaultSharp/issues/new)

[![Join the chat at https://gitter.im/rajanadar-VaultSharp/Lobby](https://badges.gitter.im/rajanadar-VaultSharp/Lobby.svg)](https://gitter.im/rajanadar-VaultSharp/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)	
[![License](https://img.shields.io/:license-apache%202.0-brightgreen.svg)](http://www.apache.org/licenses/LICENSE-2.0.html)	
[![Build status](https://ci.appveyor.com/api/projects/status/aldh4a6n2t7hthdv?svg=true)](https://ci.appveyor.com/project/rajanadar/vaultsharp)

### What is VaultSharp?	

* VaultSharp is a .NET 2.0 Standard cross-platform C# Library that can be used in any .NET application to interact with Hashicorp's Vault.	
* The Vault system is a secret management system built as an Http Service by Hashicorp.

VaultSharp has been re-designed ground up, to give a structured user experience across the various auth methods, secrets engines & system apis.
Also, the Intellisense on IVaultClient class should help. I have tried to add a lot of documentation.

### Give me a quick snippet for use!

 * Add a Nuget reference to VaultSharp as follows ```Install-Package VaultSharp -Version <latest_version>```
 * Instantiate a IVaultClient as follows:

 ```cs	
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod = new TokenAuthMethodInfo("MY_VAULT_TOKEN");

// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// Use client to read a key-value secret.
var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("secret-name");

// Generate a dynamic Consul credential
var consulCreds = await vaultClient.V1.Secrets.Consul.GenerateCredentialsAsync(consulRole, consulMount);	
var consulToken = consulCredentials.Data.Token;
```

### VaultSharp and Consul Support

* VaultSharp supports dynamic Consul credential generation.
* Please look at the API usage in the 'Consul' section of 'Secrets Engines' below, to see all the Consul related methods in action.

### Auth Methods

* VaultSharp supports all authentication methods supported by the Vault Service
* Here is a sample to instantiate the vault client with each of the authentication backends.

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

IAuthMethodInfo authMethod = new IAMAWSAuthMethodInfo(nonce, roleName); // uses default requestHeaders
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

```cs
IAuthMethodInfo authMethod = new LDAPAuthMethodInfo(userName, password);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the 
// vault token/policies mapped to the LDAP username and password.

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
var clientCertificate = new X509Certificate2(certificatePath, certificatePassword, 
                            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

IAuthMethodInfo authMethod = new CertAuthMethodInfo(clientCertificate);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the 
vault token/policies mapped to the client certificate.

```

#### Token Auth Method

```cs
IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// any operations done using the vaultClient will use the 
vault token/policies mapped to the vault token.

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

 * VaultSharp also supports a custom way to provide the Vault auth token to VaultSharp.
 * In this approach, you are free to provide any delegate that returns the Vault token.
 * The token can be retrieved from a database, another secret engine, from a file, etc.

```cs
// Func<Task<String>> getTokenAsync = a custom async method to return the vault token.
IAuthMethodInfo authMethod = new CustomAuthMethodInfo("my-own-token-auth-method", getTokenAsync);
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

```

#### App Id Auth Method (DEPRECATED)

* Please note that the app-id auth backend has been deprecated by Vault. They recommend us to use the AppRole backend.
* So VaultSharp doesn't support App Id natively. 
* If you are in dire need of the App Id support, please raise an issue.

#### MFA (LEGACY/UNSUPPORTED)

* Please note that this legacy Auth Method is not supported by Vault anymore.
* Instead Vault Enterprise contains a fully-supported MFA system.
* It is significantly more complete and flexible and which can be used throughout Vault's API. 
* Please see the *System Backend* section of the docs for the Enterprise MFA apis.

### Secrets Engines

* VaultSharp supports all secrets engines supported by the Vault Service
* Here is a sample to instantiate the vault client with each of the secrets engine

All of the below examples assume that you have a vault client instance ready. e.g.

 ```cs	
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod = new TokenAuthMethodInfo("MY_VAULT_TOKEN");

// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

```

#### Active Directory

Coming soon...

#### AWS

Coming soon...

#### Consul

```cs

// Generate a dynamic Consul credential
var consulCreds = await vaultClient.V1.Secrets.Consul.GenerateCredentialsAsync(consulRole);	
var consulToken = consulCredentials.Data.Token;

```

#### Cubbyhole

Coming soon...

#### Databases

Coming soon...

#### Google Cloud

Coming soon...

#### Key Value

 * VaultSharp supports both v1 and v2 of the Key Value Secrets Engine.
 * Here are examples for both.

##### Key Value - V1

```cs

// Use client to read a v1 key-value secret.
var kv1Secret = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("v1-secret-name");

```

##### Key Value - V2

```cs

// Use client to read a v2 key-value secret.
var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("v2-secret-name");

```

#### Identity

Coming soon...

#### Nomad

Coming soon...

#### PKI (Cerificates)

```cs

var certificateCredentialsRequestOptions = new CertificateCredentialsRequestOptions { // initialize };
var certSecret = await vaultClient.V1.Secrets.PKI.GenerateCredentialsAsync(pkiRoleName, certificateCredentialsRequestOptions);

var privateKeyContent = certSecret.Data.PrivateKeyContent;

```

#### RabbitMQ

Coming soon...

#### SSH

Coming soon...

#### TOTP

Coming soon...

#### Transit

Coming soon...

### System Backend

 * The system backend is a default backend in Vault that is mounted at the /sys endpoint. 
 * This endpoint cannot be disabled or moved, and is used to configure Vault and interact with many of Vault's internal features.

VaultSharp already supports several of the System backend features.

```cs

// vaultClient.V1.System.<method> The method you are looking for.

```

Additional documentation coming soon...

### What is the deal with the Versioning of VaultSharp?

* This library is written for Hashicorp's Vault Service
* The Vault service is evolving constantly and the Hashicorp team is rapidly working on it.
* Pretty soon, they should have an 1.0.0 version of the Vault Service from Hashicorp.
* Because this client library is intended to facilititate the Vault Service operations, this library makes it easier for its consumers to relate to the Vault service it supports.
* Hence a version of 0.10.x denotes that this library will support the Vault 0.10.x Service Apis.
* Tomorrow when Vault Service gets upgraded to 1.0.0, this library will be modified accordingly and versioned as 1.0.0

### Can I use it in my PowerShell Automation?

* Absolutely. VaultSharp is a .NET Library. 
* This means, apart from using it in your C#, VB.NET, J#.NET and any .NET application, you can use it in PowerShell automation as well.
* Load up the DLL in your PowerShell code and execute the methods. PowerShell can totally work with .NET Dlls.

### All the methods are async. How do I use them synchronously?

* The methods are async as the defacto implementation. The recommended usage.
* However, there are innumerable scenarios where you would continue to want to use it synchronously.
* For all those cases, there are various options available to you.
* There is a lot of discussion around the right usage, avoiding deadlocks etc.
* This library allows you to set the 'continueAsyncTasksOnCapturedContext' option when you initialize the client.
* It is an optional parameter and defaults to 'false'
* Setting it to false, allows you to access the .Result property of the task with reduced/zero deadlock issues.
* There are other ways as well to invoke it synchronously, and  I leave it to the users of the library. (Task.Run etc.) 
* But please note that as much as possible, use it in an async manner. 

### In Conclusion

* If the above documentation doesn't help you, feel free to create an issue or email me. https://github.com/rajanadar/VaultSharp/issues/new

### Happy Coding folks!