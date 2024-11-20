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
            var roleListResponse = _authenticatedVaultClient.V1.Secrets.PKI.ListRolesAsync(pkiMountPath).Result;

            Assert.Empty(roleListResponse.Data?.Keys);

            // write a new role
            var allowedDomain = "test.com";
            var roleName = "testRole";

            var newRole = new PKIRole()
            {
                AllowedDomains = new List<string>() { allowedDomain },
                AllowSubdomains = true,
            };

            var newRoleResponse = _authenticatedVaultClient.V1.Secrets.PKI.WriteRoleAsync(roleName, newRole, pkiMountPath).Result;
            Assert.True(newRoleResponse.Data.AllowedDomains.Exists(d => d == allowedDomain));

            // verify it shows up in the list now
            roleListResponse = _authenticatedVaultClient.V1.Secrets.PKI.ListRolesAsync(pkiMountPath).Result;

            Assert.Single(roleListResponse.Data.Keys);

            // generate a new root CA
            var newRoot = new GenerateRootRequest()
            {
                CommonName = "test.com",
                IssuerName = "testroot_test_com",
                KeyName = "testroot_key",                
            };

            var newRootResponse = _authenticatedVaultClient.V1.Secrets.PKI.GenerateRootAsync("exported", newRoot, pkiMountPath).Result;

            Assert.NotEmpty(newRootResponse.Data.PrivateKey);

            // issue a cert

            var signCertOptions = new SignCertificatesRequestOptions()
            {
                CommonName = "my.test.com",
                Csr = "-----BEGIN CERTIFICATE REQUEST-----\r\nMIIChDCCAWwCAQAwFjEUMBIGA1UEAwwLbXkudGVzdC5jb20wggEiMA0GCSqGSIb3\r\nDQEBAQUAA4IBDwAwggEKAoIBAQCQj/loI/Us7ayc/GOQlDWWv/lH+pcJ9g3w2Q/U\r\nzl8LBR7CD6Lve7TzBHxXU77gpg/lrCksr9LfE85FofhMy2WdEDTQw2BqdA/xwphh\r\nGsmaV+gfniZT96KzTOTRfMLE8Lf88bw5us7ha12MdJEhVX72kqXs7r/Hx5wz6gyw\r\niKcuezeTjp3r0qBPUIgHDgSg2TCVPs+THuhozQd4InQFU4HIWrDnR8unm5udRWId\r\nXOfxcQcgPp+UhzHzj+H/TfhqVeDzSvjVyir0llRAg7mNZctb/8lyOFPOTZkm4dWW\r\nzWVowYctnWgL05KcFK2wOJry16+gyzJaDE4EoxA/+YXVlnsfAgMBAAGgKTAnBgkq\r\nhkiG9w0BCQ4xGjAYMBYGA1UdEQQPMA2CC215LnRlc3QuY29tMA0GCSqGSIb3DQEB\r\nCwUAA4IBAQA41h0RFx8NLNSq92sgQwAh+FjZNqcjq8sLj7P0jwYi0CJrGf2fFfw1\r\nRhcZefwc3uGPsa/LXyGeBO4k88q8hA6x0B3yUhH26nC+OI1jhfj97x5pl+JFGkYT\r\nAP5Su9vwDKc22T8cv9K7Yzor3mfGc+Vs4HPH/pasg5cTqTageXNiBMd9VZKXzvrb\r\nhQyjW1uomsKCTlqCcjkRSA6eppxdAYCOKR/cfNNai2cIhZXuLZ4Y3gLosk/J8MGP\r\npRgz1SBls41Vk9gP8XF9e8eAKXVvnxSoCbifdNfjBkLaQgfaYpCV+NKBwQBmz7ad\r\nHjirOTU4elr/qUq91gyYuakOCZGWKm0k\r\n-----END CERTIFICATE REQUEST-----",
                TimeToLive = "24h"
            };

            var newCertResponse = _authenticatedVaultClient.V1.Secrets.PKI.SignCertificateAsync(roleName, signCertOptions, pkiMountPath).Result;

            Assert.NotEmpty(newCertResponse.Data.CertificateContent);

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(pkiMountPath).Wait();
        }
    }
}
