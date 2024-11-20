using System;
using System.Collections.Generic;
using System.Linq;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.PKI;
using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunPKISecretsBackendSamples()
        {
            var pkiMountPath = Guid.NewGuid().ToString();

            // mount a new v1 kv
            var pkiSecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.PKI,
                Config = new Dictionary<string, object>(),
                Path = pkiMountPath
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(pkiSecretsEngine).Wait();

            var secretBackends = _authenticatedVaultClient.V1.System.GetSecretBackendsAsync().Result;
            DisplayJson(secretBackends);
            Assert.True(secretBackends.Data.Any());
            var pkiPath = Guid.NewGuid().ToString();

            // verify the role list is initially empty
            var roleListResponse = _authenticatedVaultClient.V1.Secrets.PKI.ListRolesAsync().Result;

            Assert.Empty(roleListResponse.Data.Keys);


            // write a new role
            var allowedDomain = "test.com";
            var roleName = "testRole";

            var newRole = new PKIRole()
            {
                AllowedDomains = new List<string>() { allowedDomain },
                AllowSubdomains = true
            };

            var newRoleResponse = _authenticatedVaultClient.V1.Secrets.PKI.WriteRoleAsync(roleName, newRole, pkiMountPath).Result;
            Assert.True(newRoleResponse.Data.AllowedDomains.Exists(d => d == allowedDomain));

            // verify it shows up in the list now
            roleListResponse = _authenticatedVaultClient.V1.Secrets.PKI.ListRolesAsync().Result;

            Assert.Single(roleListResponse.Data.Keys);

            // generate a new root CA
            var newRoot = new GenerateRootRequest()
            {
                CommonName = "test.com"
            };

            var newRootResponse = _authenticatedVaultClient.V1.Secrets.PKI.GenerateRootAsync("exported", newRoot).Result;

            Assert.NotEmpty(newRootResponse.Data.PrivateKey);

            // issue a cert

            var signCertOptions = new SignCertificatesRequestOptions()
            {
                CommonName = "my.test.com"
            };

            var newCertResponse = _authenticatedVaultClient.V1.Secrets.PKI.SignCertificateAsync(roleName, signCertOptions, pkiMountPath).Result;

            Assert.NotEmpty(newCertResponse.Data.CertificateContent);

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(pkiMountPath).Wait();
        }
    }
}
