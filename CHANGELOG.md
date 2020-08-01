## 1.4.0.5 (July 31, 2020)

**BREAKING CHANGES:**

  * The KV2 Backend type changed from `secret` to `kv-v2`

**FEATURES:**

  * [GH-146] Add support for ```GenerateDataKey``` in Transit Engine.

## 1.4.0.4 (June 25, 2020)

**IMPROVEMENTS:**

  * [GH-141] Ability to create, read and delete database roles. (non-static ones)

## 1.4.0.3 (May 07, 2020)

**BUG FIXES:**

  * [GH-135] Fixed a bug with AWS Read Roles.

## 1.4.0.2 (May 07, 2020) - Has a bug on AWS Read Roles. Please use later versions.

**IMPROVEMENTS:**

  * [GH-135] Ability to inject custom ```HttpClient``` to VaultSharp.
  * Add ```StorageType``` field to ```SealStatus```
  * Add ```Description``` field to ```BackendConfig```
  * [GH-136] VaultSharp now supports SourceLink standard for .NET debugging into sources
  * [GH-137] Read AWS Roles

## 1.4.0.1 (April 27, 2020)

**IMPROVEMENTS:**

  * [GH-133] Add support for the optional ```CertificateRoleName``` while doing Cert based Auth.

## 1.4.0 (April 25, 2020)

**FEATURES:**

  * [GH-131] Add support for ```AliCloud``` Secrets Engine.
  * [GH-71] SSH Key Signing
  * [GH-122] CloudFoundry Auth Method: Support for CloudFoundry login tokens including ability to create signatures.
  * [GH-96] Ability to delete secret, delete secret versions and undelete a secret.
  * [GH-113] Extend the TOTP Secrets Engine with more APIs: Create Key, Read, Read All and Delete Key.
  * [GH-109] Support for Static Database Roles and Static Credentials. CRUD Role, create and rotate static credentials.
  * [GH-132] Kerberos Auth Method: Support for Kerberos login tokens.
  * [GH-59] Ability to create tokens: Attached tokens, Orphaned tokens, Role based tokens.
  * [GH-69] Read CA certificate API
  * [GH-117] Add ability to read all keys from Transit backend.
  * [GH-106] Ability to manage LDAP Groups and Users
  * [GH-130] Add support for OCI Auth backend login
  * [GH-125] Add support for ```Google Cloud KMS``` Secrets Engine.
  * [GH-126] Add support for ```Identity``` Secrets Engine.
  * [GH-127] Add support for ```MongoDBAtlas``` Secrets Engine.
  * [GH-128] Add support for ```OpenLDAP``` Secrets Engine.

**ENTERPRISE VAULT FEATURES:**

  * [GH-122] Add support for ```KMIP``` Secrets Engine.
  * [GH-129] Add support for ```Transform``` Secrets Engine.

**BREAKING CHANGES:**

  * ```GetSealStatusAsync``` doesn't throw an exception anymore for a sealed vault.

**IMPROVEMENTS:**

  * Add support to pass Vault token as ```X-Vault-Token``` header or as the standard ```Authorization: Bearer <vault-token>``` header.
    By default, the ```Authorization: Bearer <vault-token>``` scheme is used.
    You can override it using the ```VaultClientSettings.UseVaultTokenHeaderInsteadOfAuthorizationHeader``` flag.
  * Support lease info in Custom Auth Info.
  * Added the ```path``` field to ```FileAuditBackend``` class.
  * Added the ```performance_standby``` field to ```HealthStatus``` class.
  * Added the ```initialized```, ```migration``` and ```recovery_seal``` fields to ```SealStatus``` class.
  * Added the ```options``` field to all the ```Backend``` classes.
  * Added the ```token_type``` field to all the ```BackendConfig``` classes.
  * Added the ```performance_standby``` and ```performance_standby_last_remote_wal``` fields to ```Leader``` class.
  * Added the ```otp``` and ```otp_length``` fields to ```RootTokenGenerationStatus``` class.
  * ```TokenCapability``` class now returns additional fields as well other than the ```capabilities``` field.

## 0.11.1003 (April 22, 2020)

**BREAKING CHANGES:**

  * [GH-86] Fix the wrong default mount name for KV1 and KV2 secret engines. 
    To minimize risks, please ensure you are using explicit mount points.

**FEATURES:**

  * [GH-75] Add possibility to revoke certificate by serial number.
  * [GH-76] Add expiration to response of GetCertificateCredentials
  * [GH-79] Ability to tidy up the certificate storage
  * [GH-84] Ability to delete KV2 secret metadata and all versions

**BUG FIXES:**

  * [GH-94] Add namespace support.
  * [GH-82] Fix the GetSecretBackendsAsync() deserialization error
  * [GH-116] [GH-107] Fix deadlock issues
  * [GH-85] Add checks for private key and better documentation
  * [GH-80] Fix deserialization error on ReturnedLoginAuthInfo
  * [GH-63] WriteSecretAsync now returns an output

**DOC IMPROVEMENTS:**

  * [GH-85] Add documentation to clearly mention that VaultSharp doesn't support automatic client side failovers.

## 0.11.1002 (April 21, 2019)

**FEATURES:**

  * [GH-74] Added support for .NET Standard 2.0 as well.
  * [GH-53] Added Wrapping support to the library.

## 0.11.1001 (April 20, 2019)

**BUG FIXES:**

  * Fixes default path of KeyValue version 1 to be kv.
  * Fixes a bug with IAM Login, for the shorter overload of the ```IAMAWSAuthMethodInfo``` class. Fixes [GH-61].

**DOC IMPROVEMENTS:**

  * Fixes [GH-57]. Replaced the use of ```var``` in docs with type info, where the type is hard to infer.

## 0.11.1000 (April 15, 2019)

**BUG FIXES:**

  * Fixes [GH-67] to read array of ca_chain instead of single string.
  * Removed the misleading default value in IAM Auth requestHeaders. Caller needs to explicitly pass a list of signed IAM STS Headers. Please see docs to generate this.

## 0.11.1-beta1 (March 17, 2019)

**BUG FIXES:**

  * Fixes [GH-61] to supply all the necessary values for IAM Auth login.

## 0.11.0 (August 31, 2018)

**FEATURES:**

  * Azure Secrets Engine: Add support for generating dynamic Azure credentials.
  * AliCloud Auth Method: Support for AliCloud login tokens.

## 0.10.4003 (August 22, 2018)

**FEATURES:**

  * Secrets Engine: Key Value: Version 1: Add support for Writing & Deleting of secrets.
  * Secrets Engine: Key Value: Version 2: Add support for Writing & Destroying of secrets.

**BREAKING CHANGES:**

  * Secrets Engine: Key Value: The ```ReadSecretPathListAsync``` method name changes to ```ReadSecretPathsAsync```. Apologies.

## 0.10.4002 (August 16, 2018)

**FEATURES:**

  * Add support for Azure Auth method login.
  * Add support for GoogleCloud Auth method login.
  * Add support for JWT/OIDC Auth method login.
  * Add support for Kubernetes Auth method login.
  * Add support for Okta Auth method login.
  * Add support for RADIUS Auth method login.

  * Transit Secrets Engine: Add support for Encrypt & Decrypt including Batched input.
  * Active Directory Secrets Engine: Add support for offering credentials.
  * AWS Secrets Engine: Add support for generating dynamic IAM credentials & STS IAM credentials.
  * Cubbhole Secrets Engine: Add support for read secret, read paths, write secret and delete secret APIs.
  * Database Secrets Engine: Add support for generating dynamic DB credentials.
  * GoogleCloud Secrets Engine: Add support for generating OAuth2 Token & Service Account Key.
  * Nomad Secrets Engine: Add support for generating dynamic credentials.
  * RabbitMQ Secrets Engine: Add support for generating dynamic credentials.
  * SSH Secrets Engine: Add support for generating dynamic credentials.
  * TOTP Secrets Engine: Add support for generating and validating TOTP code.
  
  * Supports .Net Standard 1.3 and .NET Framework 4.5. This enables supports for a wide range of platforms.
  
**BREAKING CHANGES:**

  * The ```GenerateCredentialsAsync``` method name changes to ```GetCredentialsAsync```. Apologies.

## 0.10.4001 (August 10, 2018)

**FEATURES:**

  * Add support for PKI dynamic credentials.
  * Add support for GitHub auth method.
  * Add support for Get, Renew and Revoke calling token info.
  * Add docs for VaultSharp 0.6.x
  * Add docs for current VaultSharp
 
**BREAKING CHANGES:**

  * Changed the property names in VaultClient.V1 from SecretsEngine to just Secrets, AuthMethod to Auth and SystemBackend to System. Apologies.

## 0.10.4000 (July 30, 2018)

**FEATURES:**

  * Secret Engines: Consul, KeyValue, PKI for dynamic credentials
  * Auth methods: AppRole, AWS, LDAP, TLS Certificates, Tokens, Username & password - Login method.
  * System Backend Apis.

**BREAKING CHANGES:**

  * VaultSharp has been redesigned from scratch to make the usage very intuitive and structured across the Secrets Engines, Auth Methods and Sys Apis.

## 0.6.5-beta1 (Unreleased)

MISC:

DEPRECATIONS/CHANGES:

FEATURES:

IMPROVEMENTS:

  * Add nonce to `SealStatus` type to allow seeing if the operation has reset. [https://github.com/hashicorp/vault/pull/2276/]
  * Add support for batch Transit operations:
    * `TransitEncryptAsync`

BUG FIXES:

## 0.6.4 (January 18, 2017)

MISC:

  * **VaultSharp 0.6.4 is now cross-platform! It supports .NET Standard 1.4 along with .NET 4.5.x and .NET 4.6.x.**
  * VaultSharp 0.6.4 is also strongly named now. This means your previous NuGet package may not automatically upgrade. 
    **YOU MAY NEED TO MANUALLY UPGRADE THE VAULTSHARP NUGET PACKAGE ONCE.**
  * Basically the change logs for  VaultSharp till 0.6.4 and follows the changelog 
    for Vault here. (https://github.com/hashicorp/vault/blob/master/CHANGELOG.md)
  * Some of the changes are called out below. But all of Vault 0.6.4 cumulative changes are supported by VaultSharp 0.6.4
  * A major breaking change in VaultSharp 0.6.4 is STRONG NAMING of VaultSharp. 
    Now both strong named and non-strong named assemblies can refer to VaultSharp. 
    This does mean that Nuget will not detect any upgrade from VaultSharp less than 0.6.4 to 0.6.4. You need to do this MANUALLY!

DEPRECATIONS/CHANGES:

  * VaultSharp 0.6.4 is also strongly named now. This means your previous NuGet package may not automatically upgrade. 
    **YOU MAY NEED TO MANUALLY UPGRADE THE VAULTSHARP NUGET PACKAGE ONCE.**
  * The `InitializeAsync` method now takes a single container object for all parameters, instead of primitive parameters.
    This single container object now has support for the additional recovery fields supported by Vault 0.6.2's initialization.
  * The File Audit backend path json key internally changed from `path` to `file_path`.
  * The `MongoDbGenerateDynamicCredentialsAsync` method now returns `MongoDbUsernamePasswordCredentials` instead of `UsernamePasswordCredentials`.
    This ensures you get the `database` field back as well.
  * The `MicrosoftSqlReadCredentialLeaseSettingsAsync` method now returns the `CredentialTimeToLiveSettings` instead of the deprecated `CredentialTtlSettings` type.
    This is in alignment with Vault deprecating `ttl_max` in favor of `max_ttl`.
  * VaultSharp 0.6.4 is now strongly named. This breaks compatibility between VaultSharp 0.6.1 and 0.6.4. 
    In fact, NuGet is going to treat them as 2 separate assemblies. I thought through a couple of options like
    releasing 2 NuGet packages and adding strongly named packages as part of a major version upgrade, but finally
    decided to just do it now and have just 1 Package.
    Because we are at the 0.x.x versions, I am thinking we can get away with this. :)
    A bit of pain now, for a lot less hassles later. (pretty much the whole conundrum of life!)
  * The `GetCallingTokenInfoAsync` now returns a new response type `CallingTokenInfo` instead of the previous `TokenInfo`.
    This supports the latest fields for Vault 0.6.4. [GH-18] 
  * The `TransitCreateEncryptionKeyAsync` now supports the `transitKeyType` parameter to specify the type of key needed.
  * The `TransitGetEncryptionKeyInfoAsync` method now returns `TransitEncryptionKeyInfo` with a lot more fields like `KeyDerivationFunction`, `ConvergentEncryptionVersion`, etc.

FEATURES:

  * New `/sys/wrapping` Apis: Wrap, Rewrap, Lookup and Unwrap.
  * The `UnwrapWrappedResponseDataAsync` method also supports a generic return type to give you strongly typed data back.
    So if you wrapped `AWSCredentials`, then you can unwrap `Secret<AWSCredentials>` instead of `Secret<Dictionary<string, object>>`.
    And at any time if you need the non-generic method, you can always fallback to the non-generic version returning a dictionary.
  * Add support for stored shares, recovery parameters etc. during the initialization of Vault.
  * Supports the new fields (`hmac_accessor`, `jsonx` format etc.) for File and SysLog Audit Backends.
  * The `AWSGenerateDynamicCredentialsWithSecurityTokenAsync` method now supports the `timeToLive` parameter.
  * The `Consul` backend now supports the listing functionality to roles `ConsulReadRoleListAsync`. (https://github.com/hashicorp/vault/issues/2065)
  * The `Transit` backend now supports the new Apis for `List of keys`, `Random`, `Hash`, `Digest`, `Sign`, `Verify` etc.
  * All the secret backends now support wrapping of the secret into a cubbyhole token. Wrapping support added for:
    * AWS Secret Backend
    * Cassandra Secret Backend
    * Consul Secret Backend
    * Cubbyhole Secret Backend
    * Generic Secret Backend
    * MongoDB Secret Backend
    * Microsoft SQL Secret Backend
    * MySql Secret Backend
    * TBD for PKI Secret Backend
    * PostgreSQL Secret Backend
    * RabbitMQ Secret Backend
    * SSH Secret Backend
    * Transit Secret Backend

IMPROVEMENTS:

  * Overall intellisense comments are updated to match the Vault documentation site.
  * The `CassandraRoleDefinition` now supports a consistency level parameter. (defaults to `Quorum`)
  * The `MongoDbGenerateDynamicCredentialsAsync` now returns the database name as well, related to the credentials. 
  * The `MySqlRoleDefinition` now supports the `RevocationSql` parameter to revoke an user.
  * Added `RevocationSql` parameter on the `PostgreSqlRoleDefinition` type to enable customization of user revocation SQL statements.
  * The WriteSecretAsync method now returns data if the underlying data allows for it. [GH-16]

BUG FIXES:

  * Fixed a race condition in the API calls to add the Vault Client Token Header. A single call involves removal and addition of the client token header. 
    Between the removal and addition, some other thread could make the API call, resulting in 401 errors. This is because the HttpClient is shared by all threads.
    The fallacy happens due to messing with Headers at the HttpClient level. 
    The fix was to compose a per thread HttpRequestMessage and set its headers. [GH-13](https://github.com/rajanadar/VaultSharp/issues/13)

## 0.6.1 (October 03, 2016)

MISC:

  * Basically the change log for  VaultSharp 0.6.1 follows the changelog 
    for Vault 0.6.1 here. (https://github.com/hashicorp/vault/blob/master/CHANGELOG.md#061-august-22-2016)
  * Some of the changes are called out below. But all of Vault 0.6.1 changes is supported by VaultSharp 0.6.1

DEPRECATIONS/CHANGES:

  * AppId backend is now deprecated, but still supported. Use AppRole instead.

FEATURES:

  * All the new Authentication backends are now supported: AppRole and AWS-EC2 based login.
  * All the new Secret backends are now supported: MongoDB, MSSQL and RabbitMQ based secret backends
  * All the List Apis are now supported.
  * New Token Apis pertaining to token-roles are now supported.
  * Advanced Health check Api changes
  * Rekey Api changes for Nonce
  * Add convergent encryption support
  * Add support for step-down Api
  * Add support for the capabilities Apis.
  * Add support for the token accessor Apis.

IMPROVEMENTS:

  * You can now provide a delegate for HttpClient to be executed. Use this to set proxy settings, message handlers etc.
  * Upgraded the Json Package dependency to 9.
  * Quick rekey Api is available now.
  * Quick Mount Api is available now.

BUG FIXES:

  * Fixed deadlock issue with Auth login. #5

## 0.4.1 (January 21, 2016)

IMPROVEMENTS:

  * Added extensive XML documentation to the Apis.
  
This is a documentation-addition-only release; other than the version number 
there are no changes from 0.4.0.
  
## 0.4.0 (January 21, 2016)

  * Initial release
  * Parity with Hashicorp's Vault 0.4.1 Api features
