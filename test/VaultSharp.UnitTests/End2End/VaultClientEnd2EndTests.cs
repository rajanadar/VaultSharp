using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.GitHub;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Authentication.Models.UsernamePassword;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests.End2End
{
    /// <summary>
    /// This class expects the currently supported version of vault.exe file 
    /// in the bin/Debug folder of the Testing solution.
    /// The config is Configs\end2end.hcl
    /// </summary>
    [Collection("Sequential")]
    public class VaultClientEnd2EndTests : IDisposable
    {
        private const string MasterKey = "86332b94ffc41576c967d177f069ab52540f165b2821d1dbf4267a4b43b1370e";
        private const string RootToken = "a3d54c99-75fc-5bf3-e1e9-b6cb5b775e92";
        private const string VaultUriWithPort = "http://127.0.0.1:8200";
        private DebugVaultClient _vaultClient;
        private Uri _vaultUri;


        /// <summary>
        ///  Parameterless setup method that will run before each test
        ///  Per method teardown is justified in order to have a clean
        ///   Vault server for each test
        /// </summary>
        /// <remarks>
        /// https://xunit.github.io/docs/comparisons.html
        /// http://mrshipley.com/2018/01/10/implementing-a-teardown-method-in-xunit/
        /// </remarks>
        public VaultClientEnd2EndTests()
        {
            _vaultUri = new Uri(VaultUriWithPort);
            _vaultClient = new DebugVaultClient(new Uri(VaultUriWithPort));
        }

        [Fact]
        private void MasterCredentialsAreNotNull()
        {
            Assert.NotNull(_vaultClient.MasterCredentials);
            Assert.NotNull(_vaultClient.MasterCredentials.RootToken);
            Assert.NotNull(_vaultClient.MasterCredentials.MasterKeys);
            Assert.True(_vaultClient.MasterCredentials.MasterKeys.Length == 5);
        }

        [Fact]
        private async Task TokenTests()
        {
            var secret1 = await _vaultClient.AuthenticatedVaultClient.CreateTokenAsync();
            Assert.NotNull(secret1);

            var secret2 = await _vaultClient.AuthenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions { NoParent = true });
            Assert.NotNull(secret2);

            var accessors = await _vaultClient.AuthenticatedVaultClient.GetTokenAccessorListAsync();
            Assert.True(accessors.Data.Keys.Any());

            var tokenInfoByAccessor = await _vaultClient.AuthenticatedVaultClient.GetTokenInfoByAccessorAsync(accessors.Data.Keys.First());
            Assert.NotNull(tokenInfoByAccessor);

            // TODO: Write another tests that tests revoking the token in isolation
            //await _authenticatedVaultClient.RevokeTokenByAccessorAsync(accessors.Data.Keys.First());

            var accessors2 = await _vaultClient.AuthenticatedVaultClient.GetTokenAccessorListAsync();
            Assert.True(accessors.Data.Keys.Count == accessors2.Data.Keys.Count);

            var secret3 = await _vaultClient.AuthenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions { NoParent = true });
            Assert.NotNull(secret3);

            var callingTokenInfo = await _vaultClient.AuthenticatedVaultClient.GetCallingTokenInfoAsync();
            Assert.Equal(_vaultClient.MasterCredentials.RootToken, callingTokenInfo.Data.Id);

            var tokenInfo1 = await _vaultClient.AuthenticatedVaultClient.GetTokenInfoAsync(secret1.AuthorizationInfo.ClientToken);
            Assert.Equal(secret1.AuthorizationInfo.ClientToken, tokenInfo1.Data.Id);

            var tokenInfo2 = await _vaultClient.AuthenticatedVaultClient.GetTokenInfoAsync(secret2.AuthorizationInfo.ClientToken);
            Assert.Equal(secret2.AuthorizationInfo.ClientToken, tokenInfo2.Data.Id);

            await _vaultClient.AuthenticatedVaultClient.RevokeTokenAsync(secret1.AuthorizationInfo.ClientToken, true);
            await Assert.ThrowsAsync<Exception>(() => _vaultClient.AuthenticatedVaultClient.GetTokenInfoAsync(secret1.AuthorizationInfo.ClientToken));

            // check if renewal of same token calls renew-self.
            // do it with lease id.

            // await _authenticatedVaultClient.RenewTokenAsync(_masterCredentials.RootToken);

            // renew calls need a lease id. raja todo
        }

        [Fact]
        private async Task TokenAuthenticationProviderTests()
        {
            var secret = await _vaultClient.AuthenticatedVaultClient.CreateTokenAsync();

            var tokenAuthenticationInfo = new TokenAuthenticationInfo(secret.AuthorizationInfo.ClientToken);
            var tokenClient = VaultClientFactory.CreateVaultClient(_vaultUri, tokenAuthenticationInfo);

            var authBackends = await tokenClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());
        }

        [Fact]
        private async Task UsernamePasswordAuthenticationProviderTests()
        {
            // userpass auth
            var path = "userpass" + Guid.NewGuid();
            var prefix = "auth/" + path;
            var username = "user1";
            var password = "pass1";

            var authenticationInfo = new UsernamePasswordAuthenticationInfo(path, username, password);

            var userPassClient = VaultClientFactory.CreateVaultClient(_vaultUri, authenticationInfo);
            var authBackend = new AuthenticationBackend { BackendType = AuthenticationBackendType.UsernamePassword, AuthenticationPath = authenticationInfo.MountPoint };

            await _vaultClient.AuthenticatedVaultClient.EnableAuthenticationBackendAsync(authBackend);
            await _vaultClient.AuthenticatedVaultClient.WriteSecretAsync(prefix + "/users/" + username, new Dictionary<string, object>
                    {
                        { "password", password },
                        { "policies", "root" }
                    });

            var authBackends = await _vaultClient.AuthenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());

            await _vaultClient.AuthenticatedVaultClient.DisableAuthenticationBackendAsync(authBackend.AuthenticationPath);
        }

        [Fact]
        private void CannotInitializeMoreThanOnce()
        {
            // try to initialize an already initialized vault.
            var aggregateException =
                Assert.Throws<AggregateException>(
                    () => _vaultClient.UnauthenticatedVaultClient.InitializeAsync(new InitializeOptions
                    {
                        SecretShares = 5,
                        SecretThreshold = 3
                    }).Result);

            Assert.NotNull(aggregateException.InnerException);
            Assert.True(aggregateException.InnerException.Message.Contains("Vault is already initialized"));
        }

        [Fact(Skip = "Need Auth Credentials")]
        private async Task GithubAuthenticationProviderTests()
        {
            // github auth
            var personalAccessToken = "2ba5fecdddbf09b2b6facc495bdae12f3067d9d4";
            var path = "github" + Guid.NewGuid();
            var prefix = "auth/" + path;
            var organization = "testingalib";
            var team = "testalibteam1";

            var githubAuthenticationInfo = new GitHubAuthenticationInfo(path, personalAccessToken);

            var githubClient = VaultClientFactory.CreateVaultClient(_vaultUri, githubAuthenticationInfo);
            var githubAuthBackend = new AuthenticationBackend { BackendType = AuthenticationBackendType.GitHub, AuthenticationPath = githubAuthenticationInfo.MountPoint };

            await _vaultClient.AuthenticatedVaultClient.EnableAuthenticationBackendAsync(githubAuthBackend);
            await _vaultClient.AuthenticatedVaultClient.WriteSecretAsync(prefix + "/config", new Dictionary<string, object> { { "organization", organization } });
            await _vaultClient.AuthenticatedVaultClient.WriteSecretAsync(prefix + "/map/teams/" + team, new Dictionary<string, object> { { "value", "root" } });

            var authBackends = await githubClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());

            await _vaultClient.AuthenticatedVaultClient.DisableAuthenticationBackendAsync(githubAuthBackend.AuthenticationPath);

            _vaultClient.Dispose();
        }


        private async Task RawSecretAndMoreTests()
        {
            // renew secret
            //var secret = await _authenticatedVaultClient.RenewSecretAsync(readRawValues.LeaseId);
            // Assert.NotNull(secret);

            // revoke secret (only applicable for secrets with leases)
            // renew secret
            // await _authenticatedVaultClient.RevokeSecretAsync(readRawValues.LeaseId);
            // await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.ReadRawSecretAsync(rawPath));

            // write 2 secrets and revoke prefix.

            //var path1 = rawPath + "a/key1";
            //var path2 = rawPath + "a/key2";

            //var rawValues2 = new Dictionary<string, object>(rawValues);
            //rawValues2.Add("foo2", 10);

            //await _authenticatedVaultClient.WriteRawSecretAsync(path1, rawValues);
            //await _authenticatedVaultClient.WriteRawSecretAsync(path2, rawValues2);

            //await _authenticatedVaultClient.RevokeAllSecretsUnderPrefixAsync(rawPath + "a");

            //var v1 = await _authenticatedVaultClient.ReadRawSecretAsync(path1);

            //await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.ReadRawSecretAsync(path1));
            //await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.ReadRawSecretAsync(path2));
        }

        public void Dispose()
        {
            _vaultClient.Dispose();
        }

        //[Fact]
        //private async Task PGPOptionsMustMatch()
        //{
        //    // try to initialize with mismatched PGP Key
        //    var vaultUri =
        //    var unauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(_vaultUri, null);
        //    var invalidPgpException = await Assert.ThrowsAsync<System.Exception>(
        //            () => unauthenticatedVaultClient.InitializeAsync(new InitializeOptions
        //            {
        //                SecretShares = 5,
        //                SecretThreshold = 3,
        //                PgpKeys = new[] { Convert.ToBase64String(Encoding.UTF8.GetBytes("pgp_key1")) }
        //            }));

        //    Assert.NotNull(invalidPgpException.InnerException);
        //    Assert.True(invalidPgpException.InnerException.Message.Contains("400 BadRequest"));
        //    // todo find valid PGP keys
        //}

        //        [Fact(Skip = "no ldap")]
        //        public async Task LDAPAuthenticationProviderTest()
        //        {
        //            // ldap auth 

        //            var path = "ldap" + Guid.NewGuid();
        //            var prefix = "auth/" + path;
        //            var username = "rajnadar";
        //            var password = "*******";

        //            var ldapGroup = "scientists";

        //            var authenticationInfo = new LDAPAuthenticationInfo(path, username, password);

        //            var ldapClient = VaultClientFactory.CreateVaultClient(VaultUri, authenticationInfo);
        //            var authBackend = new AuthenticationBackend { Type = "ldap", Path = authenticationInfo.Path };

        //            await _client.EnableAuthenticationBackendAsync(authBackend);
        //            await _client.WriteSecretAsync(prefix + "/config", new Dictionary<string, object>
        //            {
        //                { "url", "ldap://ldap.forumsys.com" },
        //                { "userattr", "uid" },
        //                { "userdn", "dc=example,dc=com" },
        //                { "groupdn", "dc=example,dc=com" },
        //                { "upndomain", "forumsys.com" },
        //                { "certificate", "@ldap_ca_cert.pem" },
        //                { "insecure_tls", false },
        //                { "starttls", true },
        //            });

        //            await _client.WriteSecretAsync(prefix + "/groups/" + ldapGroup, new Dictionary<string, object> { { "policies", "root" } });
        //            await _client.WriteSecretAsync(prefix + "/users/" + username, new Dictionary<string, object> { { "groups", ldapGroup } });

        //            var authBackends = await ldapClient.GetAuthenticationBackendsAsync();
        //            Assert.True(authBackends.Any());

        //            await _client.DisableAuthenticationBackendAsync(authBackend.Path);
        //        }

        //        [Fact(Skip = "figuring out a cert issue")]
        //        public async Task CertificateAuthenticationProviderTest()
        //        {
        //            // cert auth 

        //            var path = "cert" + Guid.NewGuid();
        //            var prefix = "auth/" + path;

        //            var certificatePath = "vault_test.pfx";
        //            var certificatePassword = "test";

        //            var clientCertificate = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

        //            var authenticationInfo = new CertificateAuthenticationInfo(path, clientCertificate);
        //            var certClient = VaultClientFactory.CreateVaultClient(VaultUri, authenticationInfo);
        //            var authBackend = new AuthenticationBackend { Type = "cert", Path = authenticationInfo.Path };

        //            //RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)clientCertificate.PrivateKey;
        //            //AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);

        //            //var pemBuilder = new StringBuilder();

        //            //using (var stringWriter = new StringWriter(pemBuilder))
        //            //{
        //            //    var pemWriter = new PemWriter(stringWriter);
        //            //    pemWriter.WriteObject(keyPair.Private);
        //            //    pemWriter.Writer.Flush();
        //            //    pemWriter.Writer.Close();
        //            //}

        //            //var pemContents = pemBuilder.ToString();

        //            await _client.EnableAuthenticationBackendAsync(authBackend);
        //            //await _client.WriteAsync(prefix + "/certs/web", new Dictionary<string, object>
        //            //{
        //            //    { "display_name", "web" },
        //            //    { "policies", "root" },
        //            //    { "certificate", pemContents },
        //            //    { "ttl", "1h" }
        //            //});

        //            var authBackends = await certClient.GetAuthenticationBackendsAsync();
        //            Assert.True(authBackends.Any());

        //            await _client.DisableAuthenticationBackendAsync(authBackend.Path);
        //        }
    }
}