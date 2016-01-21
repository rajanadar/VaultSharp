# VaultSharp
A .NET Library for HashiCorp's Vault - A Secret Management System.

## Nuget Package: https://www.nuget.org/packages/VaultSharp/

### What is VaultSharp?

VaultSharp is a C# Library that can be used in any .NET application to interact with a Vault Service. It supports all the Vault Service Apis documented here: https://www.vaultproject.io/docs/http/

### How do I use VaultSharp? Give me a code example

* Add a Nuget reference from here: https://www.nuget.org/packages/VaultSharp/
* Instantiate a IVaultClient as follows:

```cs
// instantiate VaultClient
IVaultClient vaultClient = VaultClientFactory.CreateVaultClient(vaultUriWithPort, authenticationInfo);

// use it for operations.
var consulCredentials = await vaultClient.ConsulGenerateDynamicCredentialsAsync(consulRoleName, consulMountPoint);
var consulToken = consulCredentials.Data.Token;
```

### Authentication Backends

* VaultSharp supports all the authentication backends supported by the Vault Service 0.4.0
* Here is a sample to instantiate the vault client with each of the authentication types.

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
