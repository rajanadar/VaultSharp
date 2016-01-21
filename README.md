# VaultSharp
A .NET Library for HashiCorp's Vault - A Secret Management System.

## Nuget Package: https://www.nuget.org/packages/VaultSharp/

### What is VaultSharp?

* VaultSharp is a C# Library that can be used in any .NET application to interact with Hashicorp's Vault Service.
* The Vault system is a secret management system built as an Http Service by Hashicorp. 
* This library supports all the Vault Service Apis documented here: https://www.vaultproject.io/docs/http/

### What is the deal with the Versioning of VaultSharp? (Y U NO 1.0.0)

* This library is written for the Vault Service version 0.4.0
* The Vault service is evolving constantly and the Hashicorp team is rapidly working on it.
* Pretty soon, we should have an 1.0.0 version of the Vault Service from Hashicorp.
* Because this client library is intended to facilititate the Vault Service operations, this library makes it easier for its consumers to relate to the Vault service it supports.
* Hence a version of 0.4.x denotes that this library will completely support the Vault 0.4.x Service Apis.
* Tomorrow when Vault Service gets upgraded to 0.5.x, this library will be modified accordingly and versioned as 0.5.x

### How do I use VaultSharp? Give me a code example

* Add a Nuget reference from here: https://www.nuget.org/packages/VaultSharp/
* Instantiate a IVaultClient as follows:

```cs
// instantiate VaultClient with one of the various authentication options available.
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, authenticationInfo);

// use it for operations.
var consulCredentials = await vaultClient.ConsulGenerateDynamicCredentialsAsync(consulRoleName, consulMountPoint);
var consulToken = consulCredentials.Data.Token;
```

### VaultSharp and 100% Consul Support

* VaultSharp supports all the secret backends supported by the Vault 0.4.0 Service.
* This includes 100% support for a Consul Secret backend, which is the recommended secret backend for Vault.
* Please look at the API usage in the 'Consul' section of 'Secret Backends' below, to see all the Consul related methods in action.

### The fundamental READ and WRITE operations on a Vault

* The generic READ/WRITE Apis of vault allow you to do a variety of operations.
* A lot or almost all of these operations are supported in a strongly typed manner with dedicated methods for them in this library.
* However, for some reason, if you want to use the generic READ and WRITE methods of Vault, you can use them as follows:

```cs
var path = "cubbyhole/foo/test";

var secretData = new Dictionary<string, object>
{
    {"1", "1"},
    {"2", 2},
    {"3", false},
};

await _authenticatedClient.WriteSecretAsync(path, secretData);

var secret = await _authenticatedClient.ReadSecretAsync(path);
var data = secret.Data; // this is the original dictionary back.
```

### Authentication Backends

* VaultSharp supports all the authentication backends supported by the Vault Service 0.4.0
* Here is a sample to instantiate the vault client with each of the authentication backends.

#### App Id Authentication Backend

```cs
IAuthenticationInfo appIdAuthenticationInfo = new AppIdAuthenticationInfo(mountPoint, appId, userId);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, appIdAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the app id and user id.

```

#### GitHub Authentication Backend

```cs
IAuthenticationInfo gitHubAuthenticationInfo = new GitHubAuthenticationInfo(mountPoint, personalAccessToken);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, gitHubAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the github token.

```

#### LDAP Authentication Backend

```cs
IAuthenticationInfo ldapAuthenticationInfo = new LDAPAuthenticationInfo(mountPoint, username, password);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, ldapAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the LDAP username and password.

```

#### GitHub Authentication Backend

```cs
IAuthenticationInfo gitHubAuthenticationInfo = new GitHubAuthenticationInfo(mountPoint, personalAccessToken);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, gitHubAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the github token.

```

#### Certificate (TLS) Authentication Backend

```cs
var clientCertificate = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

IAuthenticationInfo certificateAuthenticationInfo = new CertificateAuthenticationInfo(mountPoint, clientCertificate);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, certificateAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the client certificate.

```

#### Token Authentication Backend

```cs
IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(mountPoint, vaultToken);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, tokenAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the vault token.

```

#### Username and Password Authentication Backend

```cs
IAuthenticationInfo usernamePasswordAuthenticationInfo = new UsernamePasswordAuthenticationInfo(mountPoint, username, password);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, usernamePasswordAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the username/password.

```

### Secret Backends

* VaultSharp supports all the secret backends supported by the Vault Service 0.4.0
* Here is a sample to instantiate the vault client with each of the secret backends.

#### AWS Secret Backend

##### Configuring an AWS Backend

```cs
// mount the backend
await vaultClient.MountSecretBackendAsync(new SecretBackend
{
    BackendType = SecretBackendType.AWS
});

// configure root credentials to create/manage roles and generate credentials
await vaultClient.AWSConfigureRootCredentialsAsync(new AWSRootCredentials
{
    AccessKey = "access-key",
    SecretKey = "secret-key",
    Region = "region"
});

// create a named role with the IAM policy
await vaultClient.AWSWriteNamedRoleAsync("myAwsRole", new AWSRoleDefinition
{
    Policy = "iam-policy-contents"
});
```

##### Generate AWS Credentials

```cs
var awsCredentials = await vaultClient.AWSGenerateDynamicCredentialsAsync("myAwsRole");
var awsAccessKey = awsCredentials.Data.AccessKey;
var awsSecretKey = awsCredentials.Data.SecretKey;

```

#### Cassandra Secret Backend

##### Configuring a Cassandra Backend

```cs
// mount the backend
await vaultClient.MountSecretBackendAsync(new SecretBackend
{
    BackendType = SecretBackendType.Cassandra
});

// configure root connection info to create/manage roles and generate credentials
await vaultClient.CassandraConfigureConnectionAsync(new CassandraConnectionInfo
{
    Hosts = "hosts",
    Username = "username",
    Password = "password"
});

// create a named role
await vaultClient.CassandraWriteNamedRoleAsync("myCassandraRole", new CassandraRoleDefinition
{
    CreationCql = "csql"
});
```

##### Generate Cassandra Credentials

```cs
var cassandraCredentials = await vaultClient.CassandraGenerateDynamicCredentialsAsync("myCassandraRole");
var cassandraUsername = cassandraCredentials.Data.Username;
var cassandraPassword = cassandraCredentials.Data.Password;

```

