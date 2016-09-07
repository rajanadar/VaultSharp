using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.AppId;
using VaultSharp.Backends.Authentication.Models.GitHub;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Authentication.Models.UsernamePassword;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.Secret.Models.PKI;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests.End2End
{
    public class VaultClientEnd2EndTests
    {
        private const string MasterKey = "86332b94ffc41576c967d177f069ab52540f165b2821d1dbf4267a4b43b1370e";
        private const string RootToken = "a3d54c99-75fc-5bf3-e1e9-b6cb5b775e92";

        private static readonly bool DevServer = false;

        [Fact(Skip = "making proper rerunnable safe acceptance tests.")]
        public async Task AllTests()
        {
            if (!DevServer)
            {
                await RawSecretAndMoreTests();
            }

            await TokenTests();

            await SecretTests();
            await AppIdAuthenticationProviderTests();
            await UsernamePasswordAuthenticationProviderTests();
            await TokenAuthenticationProviderTests();
            await MySqlCredentialTests();

            // await SSHDynamicTests();
            await PKITests();
        }

        private async Task PKITests()
        {
            var mountpoint = "pki" + Guid.NewGuid();
            var backend = new SecretBackend
            {
                BackendType = SecretBackendType.PKI,
                MountPoint = mountpoint
            };

            await _authenticatedVaultClient.MountSecretBackendAsync(backend);

            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.PKIReadCRLExpirationAsync(mountpoint));

            var expiry = "124h";
            var commonName = "blah.example.com";

            await _authenticatedVaultClient.PKIWriteCRLExpirationAsync(expiry, mountpoint);

            var readExpiry = await _authenticatedVaultClient.PKIReadCRLExpirationAsync(mountpoint);
            Assert.Equal(expiry, readExpiry.Data.Expiry);

            var nocaCert = await _unauthenticatedVaultClient.PKIReadCACertificateAsync(CertificateFormat.pem, mountpoint);
            Assert.Null(nocaCert.CertificateContent);

            // generate root certificate
            var rootCertificateWithoutPrivateKey =
                await _authenticatedVaultClient.PKIGenerateRootCACertificateAsync(new RootCertificateRequestOptions
                {
                    CommonName = commonName,
                    ExportPrivateKey = false
                }, mountpoint);

            Assert.Null(rootCertificateWithoutPrivateKey.Data.PrivateKey);

            var rootCertificate =
                await _authenticatedVaultClient.PKIGenerateRootCACertificateAsync(new RootCertificateRequestOptions
                {
                    CommonName = commonName,
                    ExportPrivateKey = true
                }, mountpoint);

            Assert.NotNull(rootCertificate.Data.PrivateKey);

            var caCert = await _unauthenticatedVaultClient.PKIReadCACertificateAsync(CertificateFormat.pem, mountpoint);
            Assert.NotNull(caCert.CertificateContent);

            var caReadCert = await _unauthenticatedVaultClient.PKIReadCertificateAsync("ca", mountpoint);
            Assert.Equal(caCert.CertificateContent, caReadCert.Data.CertificateContent);

            var caSerialNumberReadCert = await _unauthenticatedVaultClient.PKIReadCertificateAsync(rootCertificate.Data.SerialNumber, mountpoint);
            Assert.Equal(caCert.CertificateContent, caSerialNumberReadCert.Data.CertificateContent);

            var crlCert = await _unauthenticatedVaultClient.PKIReadCertificateAsync("crl", mountpoint);
            Assert.NotNull(crlCert.Data.CertificateContent);

            var crlCert2 = await _unauthenticatedVaultClient.PKIReadCRLCertificateAsync(CertificateFormat.pem, mountpoint);
            Assert.NotNull(crlCert2.CertificateContent);

            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.PKIReadCertificateEndpointsAsync(mountpoint));

            var crlEndpoint = _vaultUri.AbsoluteUri + "/v1/" + mountpoint + "/crl";
            var issuingEndpoint = _vaultUri.AbsoluteUri + "/v1/" + mountpoint + "/ca";

            var endpoints = new CertificateEndpointOptions
            {
                CRLDistributionPointEndpoints = string.Join(",", new List<string> { crlEndpoint }),
                IssuingCertificateEndpoints = string.Join(",", new List<string> { issuingEndpoint }),
            };

            await _authenticatedVaultClient.PKIWriteCertificateEndpointsAsync(endpoints, mountpoint);

            var readEndpoints = await _authenticatedVaultClient.PKIReadCertificateEndpointsAsync(mountpoint);

            Assert.Equal(crlEndpoint, readEndpoints.Data.CRLDistributionPointEndpoints.First());
            Assert.Equal(issuingEndpoint, readEndpoints.Data.IssuingCertificateEndpoints.First());

            var rotate = await _authenticatedVaultClient.PKIRotateCRLAsync(mountpoint);
            Assert.True(rotate);

            await _authenticatedVaultClient.RevokeSecretAsync(rootCertificateWithoutPrivateKey.LeaseId);

            var roleName = Guid.NewGuid().ToString();

            var role = new CertificateRoleDefinition
            {
                AllowedDomains = "example.com",
                AllowSubdomains = true,
                MaximumTimeToLive = "72h",
            };

            await _authenticatedVaultClient.PKIWriteNamedRoleAsync(roleName, role, mountpoint);

            var readRole = await _authenticatedVaultClient.PKIReadNamedRoleAsync(roleName, mountpoint);
            Assert.Equal(role.AllowedDomains, readRole.Data.AllowedDomains);

            var credentials =
                await
                    _authenticatedVaultClient.PKIGenerateDynamicCredentialsAsync(roleName,
                        new CertificateCredentialsRequestOptions
                        {
                            CommonName = commonName,
                            CertificateFormat = CertificateFormat.pem
                        }, mountpoint);

            Assert.NotNull(credentials.Data.PrivateKey);

            var credCert =
                await _unauthenticatedVaultClient.PKIReadCertificateAsync(credentials.Data.SerialNumber, mountpoint);

            // \n differences in the content.
            Assert.True(credCert.Data.CertificateContent.Contains(credentials.Data.CertificateContent));

            var pemBundle = rootCertificate.Data.CertificateContent + "\n" + rootCertificate.Data.PrivateKey;

            await _authenticatedVaultClient.PKIConfigureCACertificateAsync(pemBundle, mountpoint);

            var derCaCert =
                await _unauthenticatedVaultClient.PKIReadCACertificateAsync(CertificateFormat.der, mountpoint);
            Assert.NotNull(derCaCert.CertificateContent);

            await _authenticatedVaultClient.PKIRevokeCertificateAsync(credentials.Data.SerialNumber, mountpoint);
            await _authenticatedVaultClient.PKIDeleteNamedRoleAsync(roleName, mountpoint);

            var revocationData = await _authenticatedVaultClient.PKIRevokeCertificateAsync(rootCertificate.Data.SerialNumber, mountpoint);
            Assert.True(revocationData.Data.RevocationTime > 0);

            await _authenticatedVaultClient.UnmountSecretBackendAsync(mountpoint);
        }

        private static Uri _vaultUri;
        private static MasterCredentials _masterCredentials;

        private static IVaultClient _authenticatedVaultClient;
        private static IVaultClient _unauthenticatedVaultClient;
        private static int _vaultServerProcessId;

        static VaultClientEnd2EndTests()
        {
            InitializeVault();
        }

        ~VaultClientEnd2EndTests()
        {
            if (!DevServer)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            // Run at end

            var process = Process.GetProcesses().FirstOrDefault(p => p.Id == _vaultServerProcessId);

            if (process != null)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
        }

        private static void InitializeVault()
        {
            // for all the tests to run smoothly, this is the only method,
            // you need to setup your initial values.
            // Ensure you have an instance of Vault Server started up but not initialized.

            /*

                backend "file" {
                  path = "e:\raja\work\vault\file_backend"
                }

                listener "tcp" {
                  address = "127.0.0.1:8200"
                  tls_disable = 1
                }

                rd file_backend /S /Q
                vault server -config f.hcl

            */

            var vaultUriWithPort = "http://127.0.0.1:8200";

            _vaultUri = new Uri(vaultUriWithPort);
            _unauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(_vaultUri, null);

            if (DevServer)
            {
                _masterCredentials = new MasterCredentials
                {
                    MasterKeys = new[] { MasterKey },
                    RootToken = RootToken
                };

                IAuthenticationInfo devRootTokenInfo = new TokenAuthenticationInfo(_masterCredentials.RootToken);
                _authenticatedVaultClient = VaultClientFactory.CreateVaultClient(_vaultUri, devRootTokenInfo);

                return;
            }

            // BEGIN setting values

            var fileName = "E:\\raja\\work\\vault0.6.1\\StartVault.cmd";

            var process = Process.Start(new ProcessStartInfo(fileName));

            Assert.NotNull(process);

            _vaultServerProcessId = process.Id;

            // END setting values

            var initialized = _unauthenticatedVaultClient.GetInitializationStatusAsync().Result;
            Assert.False(initialized);

            var healthStatus = _unauthenticatedVaultClient.GetHealthStatusAsync().Result;
            Assert.False(healthStatus.Initialized);

            // try to initialize with mismatched PGP Key
            var invalidPgpException =
                Assert.Throws<AggregateException>(
                    () =>
                        _masterCredentials =
                            _unauthenticatedVaultClient.InitializeAsync(5, 3,
                                new[] { Convert.ToBase64String(Encoding.UTF8.GetBytes("pgp_key1")) }).Result);

            Assert.NotNull(invalidPgpException.InnerException);
            Assert.True(invalidPgpException.InnerException.Message.Contains("400 BadRequest"));

            _masterCredentials = _unauthenticatedVaultClient.InitializeAsync(7, 6).Result;

            Assert.NotNull(_masterCredentials);
            Assert.NotNull(_masterCredentials.RootToken);
            Assert.NotNull(_masterCredentials.MasterKeys);

            Assert.True(_masterCredentials.MasterKeys.Length == 7);

            process.CloseMainWindow();
            process.WaitForExit();

            process = Process.Start(new ProcessStartInfo(fileName));
            Assert.NotNull(process);

            _vaultServerProcessId = process.Id;

            // todo find valid PGP keys
            //var pgpKeys = new[] { Convert.ToBase64String(Encoding.UTF8.GetBytes("pgp_key1")), Convert.ToBase64String(Encoding.UTF8.GetBytes("pgp_key2")) };

            //_masterCredentials = _unauthenticatedVaultClient.InitializeAsync(2, 2, pgpKeys).Result;

            //Assert.NotNull(_masterCredentials);
            //Assert.NotNull(_masterCredentials.RootToken);
            //Assert.NotNull(_masterCredentials.MasterKeys);

            //Assert.True(_masterCredentials.MasterKeys.Length == 5);

            //process.CloseMainWindow();
            //process.WaitForExit();

            //process = Process.Start(new ProcessStartInfo(fileName));
            //Assert.NotNull(process);

            //_vaultServerProcessId = process.Id;

            _masterCredentials = _unauthenticatedVaultClient.InitializeAsync(5, 3).Result;

            Assert.NotNull(_masterCredentials);
            Assert.NotNull(_masterCredentials.RootToken);
            Assert.NotNull(_masterCredentials.MasterKeys);

            Assert.True(_masterCredentials.MasterKeys.Length == 5);

            healthStatus = _unauthenticatedVaultClient.GetHealthStatusAsync().Result;
            Assert.True(healthStatus.Initialized);
            Assert.True(healthStatus.Sealed);

            // try to initialize an already initialized vault.
            var aggregateException =
                Assert.Throws<AggregateException>(
                    () => _masterCredentials = _unauthenticatedVaultClient.InitializeAsync(5, 3).Result);

            Assert.NotNull(aggregateException.InnerException);
            Assert.True(aggregateException.InnerException.Message.Contains("Vault is already initialized"));

            var sealStatus = _unauthenticatedVaultClient.GetSealStatusAsync().Result;

            if (sealStatus.Sealed)
            {
                foreach (var masterKey in _masterCredentials.MasterKeys)
                {
                    sealStatus = _unauthenticatedVaultClient.UnsealAsync(masterKey).Result;

                    if (!sealStatus.Sealed)
                    {
                        healthStatus = _unauthenticatedVaultClient.GetHealthStatusAsync().Result;
                        Assert.True(healthStatus.Initialized);
                        Assert.False(healthStatus.Sealed);

                        // we are acting as the root user here.

                        IAuthenticationInfo tokenAuthenticationInfo =
                            new TokenAuthenticationInfo(_masterCredentials.RootToken);
                        _authenticatedVaultClient = VaultClientFactory.CreateVaultClient(_vaultUri, tokenAuthenticationInfo);

                        break;
                    }
                }
            }
        }

        private async Task TokenTests()
        {
            var secret1 = await _authenticatedVaultClient.CreateTokenAsync();
            Assert.NotNull(secret1);

            var secret2 = await _authenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions { NoParent = true });
            Assert.NotNull(secret2);

            var accessors = await _authenticatedVaultClient.GetTokenAccessorListAsync();
            Assert.True(accessors.Data.Keys.Any());

            var tokenInfoByAccessor = await _authenticatedVaultClient.GetTokenInfoByAccessorAsync(accessors.Data.Keys.First());
            Assert.NotNull(tokenInfoByAccessor);

            await _authenticatedVaultClient.RevokeTokenByAccessorAsync(accessors.Data.Keys.First());

            var accessors2 = await _authenticatedVaultClient.GetTokenAccessorListAsync();
            Assert.True(accessors.Data.Keys.Count - 1 == accessors2.Data.Keys.Count);

            var secret3 = await _authenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions { NoParent = true });
            Assert.NotNull(secret3);

            var callingTokenInfo = await _authenticatedVaultClient.GetCallingTokenInfoAsync();
            Assert.Equal(_masterCredentials.RootToken, callingTokenInfo.Data.Id);

            var tokenInfo1 = await _authenticatedVaultClient.GetTokenInfoAsync(secret1.AuthorizationInfo.ClientToken);
            Assert.Equal(secret1.AuthorizationInfo.ClientToken, tokenInfo1.Data.Id);

            var tokenInfo2 = await _authenticatedVaultClient.GetTokenInfoAsync(secret2.AuthorizationInfo.ClientToken);
            Assert.Equal(secret2.AuthorizationInfo.ClientToken, tokenInfo2.Data.Id);

            await _authenticatedVaultClient.RevokeTokenAsync(secret1.AuthorizationInfo.ClientToken, true);
            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.GetTokenInfoAsync(secret1.AuthorizationInfo.ClientToken));

            // check if renewal of same token calls renew-self.
            // do it with lease id.

            // await _authenticatedVaultClient.RenewTokenAsync(_masterCredentials.RootToken);

            // renew calls need a lease id. raja todo
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

        private async Task SecretTests()
        {
            var path = "cubbyhole/foo/test";

            var secretData = new Dictionary<string, object>
            {
                {"1", "1"},
                {"2", 2},
                {"3", false},
            };

            await _authenticatedVaultClient.WriteSecretAsync(path, secretData);

            var secret = await _authenticatedVaultClient.ReadSecretAsync(path);
            Assert.True(secret.Data.Count == 3);

            await _authenticatedVaultClient.DeleteSecretAsync(path);

            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.ReadSecretAsync(path));
        }

        private async Task AppIdAuthenticationProviderTests()
        {
            // app-id auth 

            var path = "app-id" + Guid.NewGuid();
            var prefix = "auth/" + path;
            var appId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var appIdAuthenticationInfo = new AppIdAuthenticationInfo(path, appId, userId);
            var appidClient = VaultClientFactory.CreateVaultClient(_vaultUri, appIdAuthenticationInfo);
            var appIdAuthBackend = new AuthenticationBackend
            {
                BackendType = AuthenticationBackendType.AppId,
                AuthenticationPath = path
            };

            await _authenticatedVaultClient.EnableAuthenticationBackendAsync(appIdAuthBackend);
            await _authenticatedVaultClient.WriteSecretAsync(prefix + "/map/app-id/" + appId, new Dictionary<string, object> { { "value", "root" }, { "display_name", appId } });
            await _authenticatedVaultClient.WriteSecretAsync(prefix + "/map/user-id/" + userId, new Dictionary<string, object> { { "value", appId } });

            var authBackends = await appidClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());

            await _authenticatedVaultClient.DisableAuthenticationBackendAsync(appIdAuthBackend.AuthenticationPath);
        }

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

            await _authenticatedVaultClient.EnableAuthenticationBackendAsync(githubAuthBackend);
            await _authenticatedVaultClient.WriteSecretAsync(prefix + "/config", new Dictionary<string, object> { { "organization", organization } });
            await _authenticatedVaultClient.WriteSecretAsync(prefix + "/map/teams/" + team, new Dictionary<string, object> { { "value", "root" } });

            var authBackends = await githubClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());

            await _authenticatedVaultClient.DisableAuthenticationBackendAsync(githubAuthBackend.AuthenticationPath);
        }

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

            await _authenticatedVaultClient.EnableAuthenticationBackendAsync(authBackend);
            await _authenticatedVaultClient.WriteSecretAsync(prefix + "/users/" + username, new Dictionary<string, object>
                    {
                        { "password", password },
                        { "policies", "root" }
                    });

            var authBackends = await userPassClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());

            await _authenticatedVaultClient.DisableAuthenticationBackendAsync(authBackend.AuthenticationPath);
        }

        private async Task TokenAuthenticationProviderTests()
        {
            // token auth 

            var secret = await _authenticatedVaultClient.CreateTokenAsync();

            var tokenAuthenticationInfo = new TokenAuthenticationInfo(secret.AuthorizationInfo.ClientToken);
            var tokenClient = VaultClientFactory.CreateVaultClient(_vaultUri, tokenAuthenticationInfo);

            var authBackends = await tokenClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authBackends.Data.Any());
        }

        private async Task MySqlCredentialTests()
        {
            var mountPoint = "mysql" + Guid.NewGuid();
            var backend = new SecretBackend
            {
                MountPoint = mountPoint,
                BackendType = SecretBackendType.MySql,
            };

            var role = "readonly";

            await _authenticatedVaultClient.MountSecretBackendAsync(backend);
            await _authenticatedVaultClient.WriteSecretAsync(mountPoint + "/config/connection", new Dictionary<string, object>
            {
                {"value", "root:root@tcp(127.0.0.1:3306)/"}
            });
            await _authenticatedVaultClient.WriteSecretAsync(mountPoint + "/config/lease", new Dictionary<string, object>
            {
                {"lease", "1h"},
                {"lease_max", "24h"}
            });
            await _authenticatedVaultClient.WriteSecretAsync(mountPoint + "/roles/" + role, new Dictionary<string, object>
            {
                {"sql", "CREATE USER '{{name}}'@'%' IDENTIFIED BY '{{password}}';GRANT SELECT ON *.* TO '{{name}}'@'%';"}
            });

            var credentials =
                await _authenticatedVaultClient.MySqlGenerateDynamicCredentialsAsync(role, backend.MountPoint);

            Assert.NotNull(credentials.LeaseId);
            Assert.NotNull(credentials.Data.Username);
            Assert.NotNull(credentials.Data.Password);

            await _authenticatedVaultClient.UnmountSecretBackendAsync(backend.MountPoint);
        }

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