VaultSharp
==========

A cross-platform .NET Library for HashiCorp's Vault - A Secret Management System.

**Latest release: [0.10.4000](https://www.nuget.org/packages/VaultSharp) [Install-Package VaultSharp -Version 0.10.4000]**

**Documentation:** [Updated version coming soon...](http://rajanadar.github.io/VaultSharp/)

**Report Issues/Feedback:** [Create an issue](https://github.com/rajanadar/VaultSharp/issues/new)

[![Join the chat at https://gitter.im/rajanadar-VaultSharp/Lobby](https://badges.gitter.im/rajanadar-VaultSharp/Lobby.svg)](https://gitter.im/rajanadar-VaultSharp/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)	
[![License](https://img.shields.io/:license-apache%202.0-brightgreen.svg)](http://www.apache.org/licenses/LICENSE-2.0.html)	
[![Build status](https://ci.appveyor.com/api/projects/status/aldh4a6n2t7hthdv?svg=true)](https://ci.appveyor.com/project/rajanadar/vaultsharp)

### What is VaultSharp?	

* VaultSharp is a .NET 2.0 Standard cross-platform C# Library that can be used in any .NET application to interact with Hashicorp's Vault.	
* The Vault system is a secret management system built as an Http Service by Hashicorp.

VaultSharp has been re-designed ground up, to give a structured user experience across the various auth methods, secrets engines & system apis.
Also, the Intellisense on IVaultClient class should help. I have tried to add a lot of documentation.

### Give me a quick snippet for use!

 * Add a Nuget reference to VaultSharp as follows ```Install-Package VaultSharp -Version 0.10.4000```
 * Instantiate a IVaultClient as follows:

 ```cs	
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod = new TokenAuthMethodInfo("MY_VAULT_TOKEN");

// Initialize settings. You can also set proxies, custom delegates etc. here.
VaultClientSettings vaultClientSettings = new VaultClientSettings("https://MY_VAULT_SERVER:8200", authMethod);

IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// Use client to read a key-value secret.
var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("secret-name");

// Generate a dynamic Consul credential
var consulCreds = await vaultClient.V1.Secrets.Consul.GenerateCredentialsAsync(consulRole, consulMount);	
var consulToken = consulCredentials.Data.Token;
```

### Happy Coding folks!