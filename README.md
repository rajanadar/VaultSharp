# VaultSharp
A .NET Library for HashiCorp's Vault - A Secret Management System.

## Nuget Package: https://www.nuget.org/packages/VaultSharp/

## Documentation Site: http://rajanadar.github.io/VaultSharp/

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

### Does VaultSharp support all the Authentication, Secret and Audit Backends?

* YES
* All Authentication, Secret and Audit backends are supported by this library.
* All administrative (seal, unseal, write policy), end-user (generate credentials) and unauthenticated methods (get status, get root CA) are supported by this client.

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
* There are other ways as well to invoke it synchronously, and  I leave it to you guys. (Task.Run etc.) 
* But please note that as much as possible, use it in an async manner. 

```cs

IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, authenticationInfo, continueAsyncTasksOnCapturedContext: true);

var consulSecret = vaultClient.ConsulGenerateDynamicCredentialsAsync(consulRole).Result;

```

### Authentication Backends (All of them are supported)

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
