VaultSharp
==========

A cross-platform .NET Library for HashiCorp's Vault - A Secret Management System.

### <span style="color: red">VaultSharp 0.10.x is in rapid development. And expected to be released by mid-May 2018.</span>

[![Join the chat at https://gitter.im/rajanadar-VaultSharp/Lobby](https://badges.gitter.im/rajanadar-VaultSharp/Lobby.svg)](https://gitter.im/rajanadar-VaultSharp/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![License](https://img.shields.io/:license-apache%202.0-brightgreen.svg)](http://www.apache.org/licenses/LICENSE-2.0.html)

## VaultSharp 0.6.4 is now released and available via NuGet. https://www.nuget.org/packages/VaultSharp/

### VaultSharp is now cross-platform (.NET Standard 1.4 other than .NET 4.5.x and .NET 4.6.x compliant)
### VaultSharp is also strong named starting 0.6.4. So you might have to do a manual NuGet upgrade, as a one time thing.

## Nuget Package: https://www.nuget.org/packages/VaultSharp/

## Documentation Site: http://rajanadar.github.io/VaultSharp/

## Report Feedback/Issues Here: https://github.com/rajanadar/VaultSharp/issues/new

**Table of Contents**

- [VaultSharp](#vaultsharp)
	- [Nuget Package: https://www.nuget.org/packages/VaultSharp/](#nuget-package-httpswwwnugetorgpackagesvaultsharp)
	- [Documentation Site: http://rajanadar.github.io/VaultSharp/](#documentation-site-httprajanadargithubiovaultsharp)
		- [What is VaultSharp?](#what-is-vaultsharp)
		- [VaultSharp 0.4.x should completely support Vault Service 0.4.0](#vaultsharp-04x-should-completely-support-vault-service-040)
		- [What is the deal with the Versioning of VaultSharp? (Y U NO 1.0.0)](#what-is-the-deal-with-the-versioning-of-vaultsharp-y-u-no-100)
		- [How do I use VaultSharp? Give me a code example](#how-do-i-use-vaultsharp-give-me-a-code-example)
		- [Does VaultSharp support all the Authentication, Secret and Audit Backends?](#does-vaultsharp-support-all-the-authentication-secret-and-audit-backends)
		- [VaultSharp and 100% Consul Support](#vaultsharp-and-100-consul-support)
		- [The fundamental READ and WRITE operations on a Vault](#the-fundamental-read-and-write-operations-on-a-vault)
		- [Can I use it in my PowerShell Automation?](#can-i-use-it-in-my-powershell-automation)
		- [All the methods are async. How do I use them synchronously?](#all-the-methods-are-async-how-do-i-use-them-synchronously)
		- [Authentication Backends (All of them are supported)](#authentication-backends-all-of-them-are-supported)
			- [App Id Authentication Backend](#app-id-authentication-backend)
			- [App Role Authentication Backend](#app-role-authentication-backend)
			- [AWS-EC2 Authentication Backend](#aws-ec2-authentication-backend)
			- [GitHub Authentication Backend](#github-authentication-backend)
			- [LDAP Authentication Backend](#ldap-authentication-backend)
			- [Certificate (TLS) Authentication Backend](#certificate-tls-authentication-backend)
			- [Token Authentication Backend](#token-authentication-backend)
			- [Username and Password Authentication Backend](#username-and-password-authentication-backend)
		- [Secret Backends (All of them are supported)](#secret-backends-all-of-them-are-supported)
			- [AWS Secret Backend](#aws-secret-backend)
				- [Configuring an AWS Backend](#configuring-an-aws-backend)
				- [Generate AWS Credentials](#generate-aws-credentials)
			- [Cassandra Secret Backend](#cassandra-secret-backend)
				- [Configuring a Cassandra Backend](#configuring-a-cassandra-backend)
				- [Generate Cassandra Credentials](#generate-cassandra-credentials)
			- [Consul Secret Backend](#consul-secret-backend)
				- [Configuring a Consul Backend](#configuring-a-consul-backend)
				- [Generate Consul Credentials](#generate-consul-credentials)
				- [Deleting Role and Unmounting the Consul backend](#deleting-role-and-unmounting-the-consul-backend)
			- [Cubbyhole Secret Backend](#cubbyhole-secret-backend)
			- [Generic Secret Backend](#generic-secret-backend)
			- [MongoDB Secret Backend](#mongodb-secret-backend)
				- [Configuring a MongoDB Backend](#configuring-a-mongodb-backend)
				- [Generate MongoDB Credentials](#generate-mongodb-credentials)
			- [MSSQL Secret Backend](#mssql-secret-backend)
				- [Configuring a MSSQL Backend](#configuring-a-mssql-backend)
				- [Generate MSSQL Credentials](#generate-mssql-credentials)
			- [MySql Secret Backend](#mysql-secret-backend)
				- [Configuring a MySql Backend](#configuring-a-mysql-backend)
				- [Generate MySql Credentials](#generate-mysql-credentials)
			- [PKI (Certificates) Secret Backend](#pki-certificates-secret-backend)
				- [Configuring a PKI Backend](#configuring-a-pki-backend)
				- [Write/Read PKI Role](#writeread-pki-role)
				- [Generate PKI Credentials](#generate-pki-credentials)
			- [PostgreSql Secret Backend](#postgresql-secret-backend)
				- [Configuring a PostgreSql Backend](#configuring-a-postgresql-backend)
				- [Generate PostgreSql Credentials](#generate-postgresql-credentials)
			- [RabbitMQ Secret Backend](#rabbitmq-secret-backend)
				- [Configuring a RabbitMQ Backend](#configuring-a-rabbitmq-backend)
				- [Generate RabbitMQ Credentials](#generate-rabbitmq-credentials)
			- [SSH Secret Backend](#ssh-secret-backend)
				- [Configuring a SSH Backend](#configuring-a-ssh-backend)
				- [Generate SSH Credentials](#generate-ssh-credentials)
			- [Transit Secret Backend](#transit-secret-backend)
				- [Configuring a Transit Backend](#configuring-a-transit-backend)
				- [Encrypt/Decrypt text](#encryptdecrypt-text)
				- [Other Transit Operations](#other-transit-operations)
		- [Audit Backends (All of them are supported)](#audit-backends-all-of-them-are-supported)
			- [File Audit Backend](#file-audit-backend)
			- [Syslog Audit Backend](#syslog-audit-backend)
		- [More Administrative & Other operations](#more-administrative--other-operations)
		- [Miscellaneous Features](#miscellaneous-features)
			- [Quick mount, unseal and rekey methods](#quick-mount-unseal-and-rekey-methods)
			- [Setting Proxy Settings, custom Message Handlers etc.](#setting-proxy-settings-custom-message-handlers-etc)
		- [In Conclusion](#in-conclusion)

### What is VaultSharp?

* VaultSharp is a C# Library that can be used in any .NET application to interact with Hashicorp's Vault Service.
* The Vault system is a secret management system built as an Http Service by Hashicorp. 
* This library supports all the Vault Service Apis documented here: https://www.vaultproject.io/docs/http/

### VaultSharp 0.6.1 completely supports Hashicorp's Vault 0.6.1

### What is the deal with the Versioning of VaultSharp? (Y U NO 1.0.0)

* This library is written for Hashicorp's Vault Service
* The Vault service is evolving constantly and the Hashicorp team is rapidly working on it.
* Pretty soon, they should have an 1.0.0 version of the Vault Service from Hashicorp.
* Because this client library is intended to facilititate the Vault Service operations, this library makes it easier for its consumers to relate to the Vault service it supports.
* Hence a version of 0.6.1 denotes that this library will completely support the Vault 0.6.1 Service Apis.
* Tomorrow when Vault Service gets upgraded to 0.6.2, this library will be modified accordingly and versioned as 0.6.2

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

### Does VaultSharp support all the Authentication, Secret and Audit Backends?

* YES
* All Authentication, Secret and Audit backends are supported by this library.
* All administrative (seal, unseal, write policy), end-user (generate credentials) and unauthenticated methods (get status, get root CA) are supported by this client.

### VaultSharp and 100% Consul Support

* VaultSharp supports all the secret backends supported by the Vault 0.6.1 Service.
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

await vaultClient.WriteSecretAsync(path, secretData);

var secret = await vaultClient.ReadSecretAsync(path);
var data = secret.Data; // this is the original dictionary back.
```

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

```cs

IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, authenticationInfo, continueAsyncTasksOnCapturedContext: true);

var consulSecret = vaultClient.ConsulGenerateDynamicCredentialsAsync(consulRole).Result;

```

### Authentication Backends (All of them are supported)

* VaultSharp supports all the authentication backends supported by the Vault Service 0.4.0
* Here is a sample to instantiate the vault client with each of the authentication backends.

#### App Id Authentication Backend

* Please note that the app-id auth backend has been deprecated by Vault. They recommend us to use the AppRole backend.
* VaultSharp still lets you use the app-id Apis, for backward compatibility.
* You can use the strongly typed api's to configure the appid and userid as follows.

```cs

// Configure app-id roles and users as follows.
await AdminVaultClient.AppIdAuthenticationConfigureAppId(appId, policy.Name, appId, path);
await AdminVaultClient.AppIdAuthenticationConfigureUserId(userId, appId, authenticationPath: path);

// now, setup the app-id based auth to get the right token.

IAuthenticationInfo appIdAuthenticationInfo = new AppIdAuthenticationInfo(mountPoint, appId, userId);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, appIdAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the app id and user id.

```

#### App Role Authentication Backend

```cs

// setup the AppRole based auth to get the right token.

IAuthenticationInfo appRoleAuthenticationInfo = new AppRoleAuthenticationInfo(mountPoint, roleId, secretId);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, appRoleAuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the app role and secret id.

```

#### AWS-EC2 Authentication Backend

```cs

// setup the AWS-EC2 based auth to get the right token.

IAuthenticationInfo awsEc2AuthenticationInfo = new AwcEc2AuthenticationInfo(mountPoint, pkcs7, nonce, roleName);
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, awsEc2AuthenticationInfo);

// any operations done using the vaultClient will use the vault token/policies mapped to the aws-ec2 role

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

### Secret Backends (All of them are supported)

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
#### Consul Secret Backend

##### Configuring a Consul Backend

```cs
// mount the backend
var consulAddress = "127.0.0.1:8500";
var consulAclMasterToken = "raja";

var backend = new SecretBackend
{
    BackendType = SecretBackendType.Consul,
};

await vaultClient.MountSecretBackendAsync(backend);

// configure access to Consul and create roles
var consulRole = "consulRole";

await vaultClient.ConsulConfigureAccessAsync(new ConsulAccessInfo()
{
    AddressWithPort = consulAddress,
    ManagementToken = consulAclMasterToken
});

// create a named role
await vaultClient.ConsulWriteNamedRoleAsync(consulRole, new ConsulRoleDefinition()
{
    TokenType = ConsulTokenType.management,
});

var readRole = await vaultClient.ConsulReadNamedRoleAsync(consulRole);
Assert.Equal(ConsulTokenType.management, readRole.Data.TokenType);
```

##### Generate Consul Credentials

```cs
var consulCredentials = await vaultClient.ConsulGenerateDynamicCredentialsAsync(consulRole);
var consulToken = consulCredentials.Data.Token;
```

##### Deleting Role and Unmounting the Consul backend

```cs
await vaultClient.ConsulDeleteNamedRoleAsync(consulRole);
await vaultClient.UnmountSecretBackendAsync(SecretBackendType.Consul.Type);

```

#### Cubbyhole Secret Backend

```cs
var path = "cubbyhole/foo1/foo2";
var values = new Dictionary<string, object>
{
    {"foo", "bar"},
    {"foo2", 345 }
};

await vaultClient.CubbyholeWriteSecretAsync(path, values);

var readValues = await vaultClient.CubbyholeReadSecretAsync(path);
var data = readValues.Data; // gives back the dictionary

await vaultClient.CubbyholeDeleteSecretAsync(path);

```

#### Generic Secret Backend

```cs
var mountpoint = "secret" + Guid.NewGuid();

var path = mountpoint + "/foo1/blah2";
var values = new Dictionary<string, object>
{
    {"foo", "bar"},
    {"foo2", 345 }
};

await
    vaultClient.MountSecretBackendAsync(new SecretBackend()
    {
        BackendType = SecretBackendType.Generic,
        MountPoint = mountpoint
    });

await vaultClient.GenericWriteSecretAsync(path, values);

var readValues = await vaultClient.GenericReadSecretAsync(path);
var data = readValues.Data; // gives back the dictionary

await vaultClient.GenericDeleteSecretAsync(path);

```
#### MongoDB Secret Backend

##### Configuring a MongoDB Backend

```cs
// mount the backend
var mountPoint = "mongodb" + Guid.NewGuid();
var backend = new SecretBackend
{
    MountPoint = mountPoint,
    BackendType = SecretBackendType.MongoDB,
};

await vaultClient.MountSecretBackendAsync(backend);

// configure root connection info to create/manage roles and generate credentials

var mongoDbConnectionInfo = new MongoDbConnectionInfo
{
 ConnectionStringUri = "mongodb://root:password@127.0.0.1:27017/admin?ssl=false"
};

await vaultClient.MongoDbConfigureConnectionAsync(mongoDbConnectionInfo, mountPoint);

var lease = new CredentialTimeToLiveSettings
{
    TimeToLive = "1m1s",
    MaximumTimeToLive = "2m1s"
};

await vaultClient.MongoDbConfigureCredentialLeaseSettingsAsync(lease);

// create a named role
var roleName = "mongodb-role";

var role = new MongoDbRoleDefinition
{
    Database = "admin",
    Roles = JsonConvert.SerializeObject(new object[] { "readWrite", new { role = "read", db = "bar" } })
};

await vaultClient.MongoDbWriteNamedRoleAsync(roleName, role);

var queriedRole = await vaultClient.MongoDbReadNamedRoleAsync(roleName);
```

##### Generate MongoDB Credentials

```cs
var generatedCreds = await vaultClient.MongoDbGenerateDynamicCredentialsAsync(roleName, mountPoint);

var username = generatedCreds.Data.Username;
var password = generatedCreds.Data.Password;

```
#### MSSQL Secret Backend

##### Configuring a MSSQL Backend

```cs
// mount the backend
var mountPoint = "mssql" + Guid.NewGuid();
var backend = new SecretBackend
{
    MountPoint = mountPoint,
    BackendType = SecretBackendType.MicrosoftSql,
};

await vaultClient.MountSecretBackendAsync(backend);

// configure root connection info to create/manage roles and generate credentials
var microsoftSqlConnectionInfo = new MicrosoftSqlConnectionInfo
{
    ConnectionString = "server=localhost\sqlexpress;port=1433;user id=sa;password=****;database=master;app name=vault",
    MaximumOpenConnections = 5,
    VerifyConnection = true
};

await vaultClient.MicrosoftSqlConfigureConnectionAsync(microsoftSqlConnectionInfo, mountPoint);

var lease = new CredentialTtlSettings()
{
    TimeToLive = "1m1s",
    MaximumTimeToLive = "2m1s"
};

await vaultClient.MicrosoftSqlConfigureCredentialLeaseSettingsAsync(lease, mountPoint);

// create a named role
var roleName = "msssqlrole";

var role = new MicrosoftSqlRoleDefinition
{
    Sql = "CREATE LOGIN '[{{name}}]' WITH PASSWORD = '{{password}}'; USE master; CREATE USER '[{{name}}]' FOR LOGIN '[{{name}}]'; GRANT SELECT ON SCHEMA::dbo TO '[{{name}}]'"
};

await vaultClient.MicrosoftSqlWriteNamedRoleAsync(roleName, role, mountPoint);

var queriedRole = await vaultClient.MicrosoftSqlReadNamedRoleAsync(roleName, mountPoint);
```

##### Generate MSSQL Credentials

```cs
var msSqlCredentials = await vaultClient.MicrosoftSqlGenerateDynamicCredentialsAsync(roleName, backend.MountPoint);

var msSqlUsername = msSqlCredentials.Data.Username;
var msSqlPassword = msSqlCredentials.Data.Password;

```
#### MySql Secret Backend

##### Configuring a MySql Backend

```cs
// mount the backend
var mountPoint = "mysql" + Guid.NewGuid();
var backend = new SecretBackend
{
    MountPoint = mountPoint,
    BackendType = SecretBackendType.MySql,
};

await vaultClient.MountSecretBackendAsync(backend);

// configure root connection info to create/manage roles and generate credentials
await vaultClient.MySqlConfigureConnectionAsync(new MySqlConnectionInfo()
{
    DataSourceName = "root:root@tcp(127.0.0.1:3306)/"
}, mountPoint);

var sql = "CREATE USER '{{name}}'@'%' IDENTIFIED BY '{{password}}';GRANT SELECT ON *.* TO '{{name}}'@'%';";

await vaultClient.MySqlConfigureCredentialLeaseSettingsAsync(new CredentialLeaseSettings()
{
    LeaseDuration = "1h",
    MaximumLeaseDuration = "2h"
}, mountPoint);

// create a named role
var mySqlRole = "mysql-readonly-role";

await vaultClient.MySqlWriteNamedRoleAsync(mySqlRole, new MySqlRoleDefinition()
{
    Sql = sql
}, mountPoint);

var readRole = await vaultClient.MySqlReadNamedRoleAsync(mySqlRole, mountPoint);
var roleSql = readRole.Data.Sql;
```

##### Generate MySql Credentials

```cs
var mySqlCredentials = await vaultClient.MySqlGenerateDynamicCredentialsAsync(mySqlRole, backend.MountPoint);

var mySqlUsername = mySqlCredentials.Data.Username;
var mySqlPassword = mySqlCredentials.Data.Password;

```
#### PKI (Certificates) Secret Backend

##### Configuring a PKI Backend

```cs
// mount the backend
var mountpoint = "pki" + Guid.NewGuid();
var backend = new SecretBackend
{
    BackendType = SecretBackendType.PKI,
    MountPoint = mountpoint
};

await vaultClient.MountSecretBackendAsync(backend);

// write expiry
var expiry = "124h";
var commonName = "blah.example.com";

await vaultClient.PKIWriteCRLExpirationAsync(expiry, mountpoint);

var readExpiry = await vaultClient.PKIReadCRLExpirationAsync(mountpoint);
Assert.Equal(expiry, readExpiry.Data.Expiry);

// read certificate in various ways
var nocaCert = await vaultClient.PKIReadCACertificateAsync(CertificateFormat.pem, mountpoint);
Assert.Null(nocaCert.CertificateContent);

// generate root certificate
var rootCertificateWithoutPrivateKey =
    await vaultClient.PKIGenerateRootCACertificateAsync(new RootCertificateRequestOptions
    {
        CommonName = commonName,
        ExportPrivateKey = false
    }, mountpoint);

Assert.Null(rootCertificateWithoutPrivateKey.Data.PrivateKey);

var rootCertificate =
    await vaultClient.PKIGenerateRootCACertificateAsync(new RootCertificateRequestOptions
    {
        CommonName = commonName,
        ExportPrivateKey = true
    }, mountpoint);

Assert.NotNull(rootCertificate.Data.PrivateKey);

// read certificate in various ways
var caCert = await vaultClient.PKIReadCACertificateAsync(CertificateFormat.pem, mountpoint);
Assert.NotNull(caCert.CertificateContent);

var caReadCert = await vaultClient.PKIReadCertificateAsync("ca", mountpoint);
Assert.Equal(caCert.CertificateContent, caReadCert.Data.CertificateContent);

var caSerialNumberReadCert = await vaultClient.PKIReadCertificateAsync(rootCertificate.Data.SerialNumber, mountpoint);
Assert.Equal(caCert.CertificateContent, caSerialNumberReadCert.Data.CertificateContent);

var crlCert = await vaultClient.PKIReadCertificateAsync("crl", mountpoint);
Assert.NotNull(crlCert.Data.CertificateContent);

var crlCert2 = await vaultClient.PKIReadCRLCertificateAsync(CertificateFormat.pem, mountpoint);
Assert.NotNull(crlCert2.CertificateContent);

// write and read certificate endpoints

var crlEndpoint = _vaultUri.AbsoluteUri + "/v1/" + mountpoint + "/crl";
var issuingEndpoint = _vaultUri.AbsoluteUri + "/v1/" + mountpoint + "/ca";

var endpoints = new CertificateEndpointOptions
{
    CRLDistributionPointEndpoints = string.Join(",", new List<string> { crlEndpoint }),
    IssuingCertificateEndpoints = string.Join(",", new List<string> { issuingEndpoint }),
};

await vaultClient.PKIWriteCertificateEndpointsAsync(endpoints, mountpoint);

var readEndpoints = await vaultClient.PKIReadCertificateEndpointsAsync(mountpoint);

Assert.Equal(crlEndpoint, readEndpoints.Data.CRLDistributionPointEndpoints.First());
Assert.Equal(issuingEndpoint, readEndpoints.Data.IssuingCertificateEndpoints.First());

// rotate CRL
var rotate = await vaultClient.PKIRotateCRLAsync(mountpoint);
Assert.True(rotate);

await vaultClient.RevokeSecretAsync(rootCertificateWithoutPrivateKey.LeaseId);
```
##### Write/Read PKI Role

```cs
// Create new Role
var roleName = Guid.NewGuid().ToString();

var role = new CertificateRoleDefinition
{
    AllowedDomains = "example.com",
    AllowSubdomains = true,
    MaximumTimeToLive = "72h",
};

await vaultClient.PKIWriteNamedRoleAsync(roleName, role, mountpoint);

var readRole = await vaultClient.PKIReadNamedRoleAsync(roleName, mountpoint);
Assert.Equal(role.AllowedDomains, readRole.Data.AllowedDomains);

```

##### Generate PKI Credentials

```cs
var certificateCredentials =
    await
        vaultClient.PKIGenerateDynamicCredentialsAsync(roleName,
            new CertificateCredentialsRequestOptions
            {
                CommonName = commonName,
                CertificateFormat = CertificateFormat.pem
            }, mountpoint);

var privateKey = certificateCredentials.Data.PrivateKey;

```

#### PostgreSql Secret Backend

##### Configuring a PostgreSql Backend

```cs
// mount the backend
var mountPoint = "postgresql" + Guid.NewGuid();
var backend = new SecretBackend
{
    MountPoint = mountPoint,
    BackendType = SecretBackendType.PostgreSql,
};

await vaultClient.MountSecretBackendAsync(backend);

await vaultClient.PostgreSqlConfigureCredentialLeaseSettingsAsync(new CredentialLeaseSettings()
{
    LeaseDuration = "1h",
    MaximumLeaseDuration = "2h"
}, mountPoint);

// configure root connection info to create/manage roles and generate credentials
await vaultClient.PostgreSqlConfigureConnectionAsync(new PostgreSqlConnectionInfo
{
    ConnectionString = "con_string",
    MaximumOpenConnections = 5
}, mountPoint);

var sql = "CREATE ROLE \"{{name}}\" WITH LOGIN PASSWORD '{{password}}' VALID UNTIL '{{expiration}}'; GRANT SELECT ON ALL TABLES IN SCHEMA public TO \"{{name}}\";";

// create a named role
var postgreSqlRole = "postgresql-readonly-role";

await vaultClient.PostgreSqlWriteNamedRoleAsync(postgreSqlRole, new PostgreSqlRoleDefinition()
{
    Sql = sql
}, mountPoint);

var readRole = await vaultClient.PostgreSqlReadNamedRoleAsync(postgreSqlRole, mountPoint);
Assert.Equal(sql, readRole.Data.Sql);
```

##### Generate PostgreSql Credentials

```cs
var postgreSqlCredentials = await vaultClient.PostgreSqlGenerateDynamicCredentialsAsync(postgreSqlRole, backend.MountPoint);

Assert.NotNull(postgreSqlCredentials.LeaseId);
Assert.NotNull(postgreSqlCredentials.Data.Username);
Assert.NotNull(postgreSqlCredentials.Data.Password);

```
#### RabbitMQ Secret Backend

##### Configuring a RabbitMQ Backend

```cs
// mount the backend
await vaultClient.QuickMountSecretBackendAsync(SecretBackendType.RabbitMQ);

// configure root connection info to create/manage roles and generate credentials
var connectionInfo = new RabbitMQConnectionInfo
{
    ConnectionUri = "http://localhost:15672",
    Username = "guest",
    Password = "guest",
    VerifyConnection = true
};

await vaultClient.RabbitMQConfigureConnectionAsync(connectionInfo);

var lease = new CredentialTimeToLiveSettings
{
    TimeToLive = "1m1s",
    MaximumTimeToLive = "2m1s"
};

await vaultClient.RabbitMQConfigureCredentialLeaseSettingsAsync(lease);
var queriedLease = await vaultClient.RabbitMQReadCredentialLeaseSettingsAsync();

var roleName = "rabbitmqrole";

var role = new RabbitMQRoleDefinition
{
    VirtualHostPermissions = "{\"/\":{\"write\": \".*\", \"read\": \".*\"}}"
};

await vaultClient.RabbitMQWriteNamedRoleAsync(roleName, role);

var queriedRole = await vaultClient.RabbitMQReadNamedRoleAsync(roleName);
```

##### Generate RabbitMQ Credentials

```cs
var generatedCreds = await vaultClient.RabbitMQGenerateDynamicCredentialsAsync(roleName);

Assert.NotNull(generatedCreds.Data.Username);
Assert.NotNull(generatedCreds.Data.Password);

```

#### SSH Secret Backend

##### Configuring a SSH Backend

```cs
// mount the backend
var sshKeyName = Guid.NewGuid().ToString();
var sshRoleName = Guid.NewGuid().ToString();

var mountPoint = "ssh" + Guid.NewGuid();
var backend = new SecretBackend
{
    BackendType = SecretBackendType.SSH,
    MountPoint = mountPoint,
};

await vaultClient.MountSecretBackendAsync(backend);

// configure key and role
var privateKey = @"-----BEGIN RSA PRIVATE KEY----- key ---";

var ip = "127.0.0.1";
var user = "rajan";

await vaultClient.SSHWriteNamedKeyAsync(sshKeyName, privateKey, mountPoint);
await vaultClient.SSHWriteNamedRoleAsync(sshRoleName, new SSHOTPRoleDefinition
{
    RoleDefaultUser = user,
    CIDRValues = "127.0.0.1/10",
}, mountPoint);

var role = await vaultClient.SSHReadNamedRoleAsync(sshRoleName, mountPoint);
Assert.True(role.Data.KeyTypeToGenerate == SSHKeyType.otp);

```

##### Generate SSH Credentials

```cs
var credentials = await
    vaultClient.SSHGenerateDynamicCredentialsAsync(sshRoleName, ip,
        sshBackendMountPoint: mountPoint);

Assert.Equal(user, credentials.Data.Username);

```
#### Transit Secret Backend

##### Configuring a Transit Backend

```cs
// mount the backend
var backend = new SecretBackend
{
    BackendType = SecretBackendType.Transit,
    MountPoint = "transit" + Guid.NewGuid(),
};

await vaultClient.MountSecretBackendAsync(backend);

// create encryption key
var keyName = "test_key" + Guid.NewGuid();
var context = "context1";

var plainText = "raja";
var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

await vaultClient.TransitCreateEncryptionKeyAsync(keyName, true, backend.MountPoint);
var keyInfo = await vaultClient.TransitGetEncryptionKeyInfoAsync(keyName, backend.MountPoint);

Assert.Equal(keyName, keyInfo.Data.Name);
Assert.True(keyInfo.Data.MustUseKeyDerivation);
Assert.False(keyInfo.Data.IsDeletionAllowed);

// configure the key
await vaultClient.TransitConfigureEncryptionKeyAsync(keyName, isDeletionAllowed: true, transitBackendMountPoint: backend.MountPoint);

keyInfo = await vaultClient.TransitGetEncryptionKeyInfoAsync(keyName, backend.MountPoint);
Assert.True(keyInfo.Data.IsDeletionAllowed);

```

##### Encrypt/Decrypt text

```cs
var cipherText = await vaultClient.TransitEncryptAsync(keyName, encodedPlainText, context, transitBackendMountPoint: backend.MountPoint);

var plainText2 = Encoding.UTF8.GetString(Convert.FromBase64String((await vaultClient.TransitDecryptAsync(keyName, cipherText.Data.CipherText, context, backend.MountPoint)).Data.PlainText));

Assert.Equal(plainText, plainText2);
```

##### Other Transit Operations

```cs
await vaultClient.TransitRotateEncryptionKeyAsync(keyName, backend.MountPoint);
var cipherText2 = await vaultClient.TransitEncryptAsync(keyName, encodedPlainText, context, transitBackendMountPoint: backend.MountPoint);

Assert.NotEqual(cipherText.Data.CipherText, cipherText2.Data.CipherText);

var cipherText3 = await vaultClient.TransitRewrapWithLatestEncryptionKeyAsync(keyName, cipherText.Data.CipherText, context, backend.MountPoint);

var newKey1 = await vaultClient.TransitCreateDataKeyAsync(keyName, false, context, 128, backend.MountPoint);
Assert.Null(newKey1.Data.PlainTextKey);

newKey1 = await vaultClient.TransitCreateDataKeyAsync(keyName, true, context, 128, backend.MountPoint);
Assert.NotNull(newKey1.Data.PlainTextKey);

await vaultClient.TransitDeleteEncryptionKeyAsync(keyName, backend.MountPoint);

```
### Audit Backends (All of them are supported)

* VaultSharp supports all the audit backends supported by the Vault Service 0.4.0
* Here is a sample to instantiate the vault client with each of the audit backends.

#### File Audit Backend

```cs

var audits = (await vaultClient.GetAllEnabledAuditBackendsAsync()).ToList();

// enable new file audit
var newFileAudit = new FileAuditBackend
{
    BackendType = AuditBackendType.File,
    Description = "store logs in a file - test cases",
    Options = new FileAuditBackendOptions
    {
        FilePath = "/var/log/file"
    }
};

await vaultClient.EnableAuditBackendAsync(newFileAudit);

// get audits
var newAudits = (await vaultClient.GetAllEnabledAuditBackendsAsync()).ToList();
Assert.Equal(audits.Count + 1, newAudits.Count);

// hash with audit
var hash = await vaultClient.HashWithAuditBackendAsync(newFileAudit.MountPoint, "testinput");
Assert.NotNull(hash);

// disabled audit
await vaultClient.DisableAuditBackendAsync(newFileAudit.MountPoint);
```

#### Syslog Audit Backend

```cs

// enable new syslog audit
var newSyslogAudit = new SyslogAuditBackend
{
    BackendType = AuditBackendType.Syslog,
    Description = "syslog audit - test cases",
    Options = new SyslogAuditBackendOptions()
};

await vaultClient.EnableAuditBackendAsync(newSyslogAudit);

// get audits
var newAudits2 = (await vaultClient.GetAllEnabledAuditBackendsAsync()).ToList();
Assert.Equal(1, newAudits2.Count);

// disabled audit
await vaultClient.DisableAuditBackendAsync(newSyslogAudit.MountPoint);

// get audits
var oldAudits2 = (await vaultClient.GetAllEnabledAuditBackendsAsync()).ToList();
Assert.Equal(audits.Count, oldAudits2.Count);

```

### More Administrative & Other operations

* VaultSharp supports all the operations supported by the Service.
* These include administrative ones like Inititalize, Unseal, Seal etc.
* Here are some samples.

```cs

await noAuthInfoClient.InitializeAsync(5, 3, null);
await vaultClient.SealAsync();

await vaultClient.UnsealAsync(masterKey); // need to run this in a loop for all master keys
await vaultClient.UnsealQuickAsync(allMasterKeys);  // unseals the Vault in 1 shot.

await vaultClient.GetSealStatusAsync();

// all policy operations

// write a new policy
var newPolicy = new Policy
{
    Name = "gubdu",
    Rules = "path \"sys/*\" {  policy = \"deny\" }"
};

await vaultClient.WritePolicyAsync(newPolicy);

// get new policy
var newPolicyGet = await vaultClient.GetPolicyAsync(newPolicy.Name);
Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

// write updates to a new policy
newPolicy.Rules = "path \"sys/*\" {  policy = \"read\" }";

await vaultClient.WritePolicyAsync(newPolicy);

// get new policy
newPolicyGet = await vaultClient.GetPolicyAsync(newPolicy.Name);
Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

// delete policy
await vaultClient.DeletePolicyAsync(newPolicy.Name);

```

### Miscellaneous Features

* VaultSharp supports some awesome features like quick mount, quick unseal, quick rekey etc.
* It also supports setting Proxy settings, custom message handlers for the HttpClient.

#### Quick mount, unseal and rekey methods

```cs

// quickly mount a secret backend
await vaultClient.QuickMountSecretBackendAsync(SecretBackendType.AWS);

// quickly mount an auth backend
await vaultClient.QuickEnableAuthenticationBackendAsync(AuthenticationBackendType.GitHub);

// quickly unseal Vault with a single call.
var sealStatus = await UnauthenticatedVaultClient.QuickUnsealAsync(AllMasterKeys);

// quickly rekey Vault with a single call.
var quick = await UnauthenticatedVaultClient.QuickRekeyAsync(AllMasterKeys, rekeyStatus.Nonce);

```
#### Setting Proxy Settings, custom Message Handlers etc.

```cs

var vaultClient = VaultClientFactory.CreateVaultClient(VaultUriWithPort, new TokenAuthenticationInfo(someToken), postHttpClientInitializeAction:
    httpClient =>
    {
        // set proxy or custom handlers here.
    });
```

### In Conclusion

* If the above documentation doesn't help you, feel free to create an issue or email me. https://github.com/rajanadar/VaultSharp/issues/new
* Also, the Intellisense on IVaultClient class should help. I have tried to add a lot of documentation.

Happy Coding folks!

