using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Backends.Audit.Models;
using VaultSharp.Backends.Audit.Models.File;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.Secret.Models.AWS;
using VaultSharp.Backends.Secret.Models.Cassandra;
using VaultSharp.Backends.Secret.Models.Consul;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests
{
    public class AcceptanceTests
    {
        // Acceptance tests setup tests:
        // 1. Ensure you have the right version of Vault.exe installed on the machine running this test.
        // 2. Fill in the variables below in SetupData.
        // 3. Please ensure this is not your production vault or anything. And this is a box you can play with.
        // 4. The acceptance tests will setup a temporary file backend, run the tests and tear it down finally.

        /// <summary>
        /// Change the data in this class as suitable.
        /// This is the only class you should be modifying for the acceptance tests to run successfully.
        /// </summary>
        private static class SetupData
        {
            // the best thing to do would be create a temp folder, and have vault.exe and any text files within that.
            // that way, all of these acceptance tests will run under the context of that folder.
            // they never delete any folder or tamper with file or credentials.

            // vault.exe that'll be used for this acceptance test run.
            public const string VaultExeFullPath = @"c:\temp\raja\vaultsharp-acceptance-tests\vault.exe";

            // turn on, if you want aws tests. if yes, provide a credential text file,
            // with access-key on line 1 and secret on line 2 and region on line 3.
            // create an IAM user with admin access and use the secret below for least hassle.
            public const bool RunAwsSecretBackendAcceptanceTests = false;
            public const string AwsCredentialsFullPath = @"c:\temp\raja\vaultsharp-acceptance-tests\aws.txt";

            // turn on, if you want cassandra tests. if yes, provide a credential text file,
            // with hosts on line 1 and root-username on line 2 and root-password on line 3.
            // once you install Cassandra, 
            // 1. set authenticator: AllowAllAuthenticator to authenticator: PasswordAuthenticator in cassandra.yaml.
            // 2. also change AllowAllAuthorizer to CassandraAuthorizer
            // 3. default cassandra superuser is cassandra/cassandra, on localhost:9042
            // 4. restart service.
            public const bool RunCassandraSecretBackendAcceptanceTests = false;
            public const string CassandraCredentialsFullPath = @"c:\temp\raja\vaultsharp-acceptance-tests\cassandra.txt";

            // install Consul and start it up
            //  .\consul.exe agent -config-file .\c.json
            public const bool RunConsulSecretBackendAcceptanceTests = false;
            public const string ConsulCredentialsFullPath = @"c:\temp\raja\vaultsharp-acceptance-tests\consul.txt";
        }

        // no need to modify these values.

        private const string FileBackendsFolderName = "per_run_file_backends_delete_anytime";
        private const string VaultConfigsFolderName = "per_run_vault_configs_delete_anytime";
        private const string FileBackendPlaceHolder = "##FILE_BACKEND_PATH##";
        private const string VaultConfigPath = "acceptance-tests-vault-config.hcl";
        private static readonly Uri VaultUriWithPort = new Uri("http://127.0.0.1:8200");

        private static readonly IVaultClient UnauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(
            VaultUriWithPort, null);

        private static IVaultClient _authenticatedVaultClient;

        private static Process _vaultProcess;
        private static MasterCredentials _masterCredentials;

        /// <summary>
        /// The one stop test for all the Vault APIs.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAllAcceptanceTestsAsync()
        {
            try
            {
                StartupVaultServer();

                await RunInitApiTests();
                await RunSealApiTests();
                await RunGenerateRootApiTests();
                await RunSecretBackendMountApiTests();
                await RunAuthenticationBackendMountApiTests();
                await RunPolicyApiTests();
                await RunCapabilitiesApiTests();
                await RunAuditBackendMountApiTests();
                await RunLeaseApiTests();
                await RunLeaderApiTests();
                await RunRekeyApiTests();
                await RunRawSecretApiTests();

                // secret backend tests.

                await RunAwsSecretBackendApiTests();
                await RunCassandraSecretBackendApiTests();
                await RunConsulSecretBackendApiTests();
                await RunCubbyholeSecretBackendApiTests();
            }
            finally
            {
                ShutdownVaultServer();
            }
        }

        private async Task RunCubbyholeSecretBackendApiTests()
        {
            var path = "cubbyhole/path1";
            var values = new Dictionary<string, object>
            {
                {"foo1", "bar"},
                {"foo2", 345 }
            };

            await _authenticatedVaultClient.CubbyholeWriteSecretAsync(path, values);

            var readValues = await _authenticatedVaultClient.CubbyholeReadSecretAsync(path);
            Assert.True(readValues.Data.Count == 2);

            values["foo2"] = 346;

            await _authenticatedVaultClient.CubbyholeWriteSecretAsync(path, values);

            readValues = await _authenticatedVaultClient.CubbyholeReadSecretAsync(path);
            Assert.True(int.Parse(readValues.Data["foo2"].ToString()) == 346);

            var path2 = "cubbyhole/path1/path2";
            var values2 = new Dictionary<string, object>
            {
                {"bar1", "bleh"},
                {"bar2", 42 }
            };

            await _authenticatedVaultClient.CubbyholeWriteSecretAsync(path2, values2);

            var list = await _authenticatedVaultClient.CubbyholeReadSecretListAsync(SecretBackendType.CubbyHole.Type);
            Assert.True(list.Data.Keys.Count == 2);

            await _authenticatedVaultClient.CubbyholeDeleteSecretAsync(path);
            await _authenticatedVaultClient.CubbyholeDeleteSecretAsync(path2);

            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.CubbyholeReadSecretAsync(path));
        }

        private async Task RunConsulSecretBackendApiTests()
        {
            if (SetupData.RunConsulSecretBackendAcceptanceTests)
            {
                try
                {
                    if (!File.Exists(SetupData.ConsulCredentialsFullPath))
                    {
                        throw new Exception("Consul Credential file does not exist: " +
                                            SetupData.ConsulCredentialsFullPath);
                    }

                    var credentialsFileContent = File.ReadAllLines(SetupData.ConsulCredentialsFullPath);

                    if (credentialsFileContent.Count() < 2)
                    {
                        throw new Exception("Consul Credential file needs at least 3 lines: " + credentialsFileContent);
                    }

                    var consulAccessInfo = new ConsulAccessInfo
                    {
                        AddressWithPort = credentialsFileContent[0],
                        ManagementToken = credentialsFileContent[1],
                    };

                    await _authenticatedVaultClient.QuickMountSecretBackendAsync(SecretBackendType.Consul);
                    await _authenticatedVaultClient.ConsulConfigureAccessAsync(consulAccessInfo);

                    var roleName = "consul-role";

                    var role = new ConsulRoleDefinition
                    {
                        LeaseDuration = "1m1s",
                        TokenType = ConsulTokenType.management
                    };

                    await _authenticatedVaultClient.ConsulWriteNamedRoleAsync(roleName, role);
                    var queriedRole = await _authenticatedVaultClient.ConsulReadNamedRoleAsync(roleName);
                    Assert.Equal(role.LeaseDuration, queriedRole.Data.LeaseDuration);

                    role.LeaseDuration = "2m1s";
                    await _authenticatedVaultClient.ConsulWriteNamedRoleAsync(roleName, role);
                    queriedRole = await _authenticatedVaultClient.ConsulReadNamedRoleAsync(roleName);
                    Assert.Equal(role.LeaseDuration, queriedRole.Data.LeaseDuration);

                    var generatedCreds = await _authenticatedVaultClient.ConsulGenerateDynamicCredentialsAsync(roleName);
                    Assert.NotNull(generatedCreds.Data.Token);

                    await _authenticatedVaultClient.ConsulDeleteNamedRoleAsync(roleName);
                }
                finally
                {
                    try
                    {
                        await _authenticatedVaultClient.QuickUnmountSecretBackendAsync(SecretBackendType.Consul);
                    }
                    catch
                    {
                        // you can always go to your Cassandra user base and delete users.
                    }
                }
            }
        }

        private async Task RunCassandraSecretBackendApiTests()
        {
            if (SetupData.RunCassandraSecretBackendAcceptanceTests)
            {
                try
                {
                    if (!File.Exists(SetupData.CassandraCredentialsFullPath))
                    {
                        throw new Exception("Cassandra Credential file does not exist: " +
                                            SetupData.CassandraCredentialsFullPath);
                    }

                    var credentialsFileContent = File.ReadAllLines(SetupData.CassandraCredentialsFullPath);

                    if (credentialsFileContent.Count() < 3)
                    {
                        throw new Exception("Cassandra Credential file needs at least 3 lines: " +
                                            credentialsFileContent);
                    }

                    var cassandraConnectionInfo = new CassandraConnectionInfo
                    {
                        Hosts = credentialsFileContent[0],
                        Username = credentialsFileContent[1],
                        Password = credentialsFileContent[2],
                        CqlProtocolVersion = 4
                    };

                    await _authenticatedVaultClient.QuickMountSecretBackendAsync(SecretBackendType.Cassandra);
                    await _authenticatedVaultClient.CassandraConfigureConnectionAsync(cassandraConnectionInfo);

                    var roleName = "cassandra-role";

                    var role = new CassandraRoleDefinition
                    {
                        CreationCql =
                            @"CREATE USER  '{{username}}' WITH PASSWORD '{{password}}' NOSUPERUSER; GRANT SELECT ON ALL KEYSPACES TO '{{username}}'; ",
                        LeaseDuration = "1m",
                        RollbackCql = "DROP USER '{{username}}';"
                    };

                    await _authenticatedVaultClient.CassandraWriteNamedRoleAsync(roleName, role);
                    var queriedRole = await _authenticatedVaultClient.CassandraReadNamedRoleAsync(roleName);
                    Assert.Equal(role.CreationCql, queriedRole.Data.CreationCql);

                    role.CreationCql =
                        @"CREATE USER '{{username}}' WITH PASSWORD '{{password}}' NOSUPERUSER; GRANT SELECT ON ALL KEYSPACES TO '{{username}}';";

                    await _authenticatedVaultClient.CassandraWriteNamedRoleAsync(roleName, role);
                    queriedRole = await _authenticatedVaultClient.CassandraReadNamedRoleAsync(roleName);
                    Assert.Equal(role.CreationCql, queriedRole.Data.CreationCql);

                    var generatedCreds =
                        await _authenticatedVaultClient.CassandraGenerateDynamicCredentialsAsync(roleName);
                    Assert.NotNull(generatedCreds.Data.Password);

                    await _authenticatedVaultClient.CassandraDeleteNamedRoleAsync(roleName);
                }
                finally
                {
                    try
                    {
                        await _authenticatedVaultClient.QuickUnmountSecretBackendAsync(SecretBackendType.Cassandra);
                    }
                    catch
                    {
                        // you can always go to your Cassandra user base and delete users.
                    }
                }
            }
        }

        private async Task RunAwsSecretBackendApiTests()
        {
            if (SetupData.RunAwsSecretBackendAcceptanceTests)
            {
                try
                {
                    if (!File.Exists(SetupData.AwsCredentialsFullPath))
                    {
                        throw new Exception("AWS Credential file does not exist: " + SetupData.AwsCredentialsFullPath);
                    }

                    var awsCredentialsFileContent = File.ReadAllLines(SetupData.AwsCredentialsFullPath);

                    if (awsCredentialsFileContent.Count() < 3)
                    {
                        throw new Exception("AWS Credential file needs at least 3 lines: " + awsCredentialsFileContent);
                    }

                    var awsRootCredentials = new AWSRootCredentials
                    {
                        AccessKey = awsCredentialsFileContent[0],
                        SecretKey = awsCredentialsFileContent[1],
                        Region = awsCredentialsFileContent[2]
                    };

                    await _authenticatedVaultClient.QuickMountSecretBackendAsync(SecretBackendType.AWS);
                    await _authenticatedVaultClient.AWSConfigureRootCredentialsAsync(awsRootCredentials);

                    var lease = new CredentialLeaseSettings
                    {
                        LeaseTime = "1m",
                        MaximumLeaseTime = "2m"
                    };

                    await _authenticatedVaultClient.AWSConfigureCredentialLeaseSettingsAsync(lease);

                    var awsRoleJsonName = "aws-role-json";
                    var awsRoleJson = new AWSRoleDefinition
                    {
                        PolicyText = JsonConvert.SerializeObject(new
                        {
                            Version = "2012-10-17",
                            Statement = new
                            {
                                Effect = "Allow",
                                Action = "iam:*",
                                Resource = "*"
                            }
                        })
                    };

                    await _authenticatedVaultClient.AWSWriteNamedRoleAsync(awsRoleJsonName, awsRoleJson);
                    var queriedRoleJson = await _authenticatedVaultClient.AWSReadNamedRoleAsync(awsRoleJsonName);
                    Assert.Equal(awsRoleJson.PolicyText, queriedRoleJson.Data.PolicyText);

                    var awsRoleNameArn = "aws-role-arn";
                    var awsRoleArn = new AWSRoleDefinition
                    {
                        ARN = "arn:aws:iam::aws:policy/AmazonEC2ReadOnlyAccess"
                    };

                    await _authenticatedVaultClient.AWSWriteNamedRoleAsync(awsRoleNameArn, awsRoleArn);
                    var queriedRole = await _authenticatedVaultClient.AWSReadNamedRoleAsync(awsRoleNameArn);
                    Assert.Equal(awsRoleArn.ARN, queriedRole.Data.ARN);

                    var roles = await _authenticatedVaultClient.AWSGetRoleListAsync();
                    Assert.True(roles.Data.Keys.Count == 2);

                    var generatedCreds =
                        await _authenticatedVaultClient.AWSGenerateDynamicCredentialsAsync(awsRoleJsonName);
                    Assert.NotNull(generatedCreds.Data.SecretKey);

                    generatedCreds = await _authenticatedVaultClient.AWSGenerateDynamicCredentialsAsync(awsRoleNameArn);
                    Assert.NotNull(generatedCreds.Data.SecretKey);

                    awsRoleJson.PolicyText = JsonConvert.SerializeObject(new
                    {
                        Version = "2012-10-17",
                        Statement = new
                        {
                            Effect = "Allow",
                            Action = "ec2:*",
                            Resource = "*"
                        }
                    });

                    await _authenticatedVaultClient.AWSWriteNamedRoleAsync(awsRoleJsonName, awsRoleJson);

                    queriedRoleJson = await _authenticatedVaultClient.AWSReadNamedRoleAsync(awsRoleJsonName);
                    Assert.Equal(awsRoleJson.PolicyText, queriedRoleJson.Data.PolicyText);

                    generatedCreds =
                        await
                            _authenticatedVaultClient.AWSGenerateDynamicCredentialsWithSecurityTokenAsync(
                                awsRoleJsonName);
                    Assert.NotNull(generatedCreds.Data.SecurityToken);

                    await _authenticatedVaultClient.AWSDeleteNamedRoleAsync(awsRoleJsonName);
                    await _authenticatedVaultClient.AWSDeleteNamedRoleAsync(awsRoleNameArn);
                }
                finally
                {
                    try
                    {
                        await _authenticatedVaultClient.QuickUnmountSecretBackendAsync(SecretBackendType.AWS);
                    }
                    catch
                    {
                        // you can always go to https://console.aws.amazon.com/iam/home#users and clean up any test users.
                    }
                }
            }
        }

        private async Task RunRawSecretApiTests()
        {
            var rawPath = "rawpath";
            var rawValues = new Dictionary<string, object>
            {
                {"foo", "bar"},
                {"foo2", 345 }
            };

            await _authenticatedVaultClient.WriteRawSecretAsync(rawPath, rawValues);

            var readRawValues = await _authenticatedVaultClient.ReadRawSecretAsync(rawPath);
            Assert.True(readRawValues.Data.RawValues.Count == 2);

            await _authenticatedVaultClient.DeleteRawSecretAsync(rawPath);

            await Assert.ThrowsAsync<Exception>(() => _authenticatedVaultClient.ReadRawSecretAsync(rawPath));
        }

        private async Task RunRekeyApiTests()
        {
            var keyStatus = await _authenticatedVaultClient.GetEncryptionKeyStatusAsync();
            Assert.True(keyStatus.SequentialKeyNumber == 1);

            await _authenticatedVaultClient.RotateEncryptionKeyAsync();

            keyStatus = await _authenticatedVaultClient.GetEncryptionKeyStatusAsync();
            Assert.True(keyStatus.SequentialKeyNumber == 2);

            var rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();
            Assert.False(rekeyStatus.Started);

            rekeyStatus = await UnauthenticatedVaultClient.InitiateRekeyAsync(2, 2);
            Assert.True(rekeyStatus.Started);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.NotNull(rekeyStatus.Nonce);

            // raja todo: test the rekey backup API, after giving good pgp encrypted keys.

            // var backups = await _authenticatedVaultClient.GetRekeyBackupKeysAsync();
            // Assert.NotNull(backups);

            await _authenticatedVaultClient.DeleteRekeyBackupKeysAsync();

            var rekeyNonce = rekeyStatus.Nonce;
            var rekeyProgress = await UnauthenticatedVaultClient.ContinueRekeyAsync(_masterCredentials.MasterKeys[0], rekeyNonce);
            Assert.False(rekeyProgress.Complete);
            Assert.Null(rekeyProgress.MasterKeys);

            rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();
            Assert.True(rekeyStatus.Started);
            Assert.True(rekeyStatus.UnsealKeysProvided == 1);

            rekeyProgress = await UnauthenticatedVaultClient.ContinueRekeyAsync(_masterCredentials.MasterKeys[1], rekeyNonce);
            Assert.True(rekeyProgress.Complete);
            Assert.NotNull(rekeyProgress.MasterKeys);

            _masterCredentials.MasterKeys = rekeyProgress.MasterKeys;
            _masterCredentials.Base64MasterKeys = rekeyProgress.Base64MasterKeys;

            rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();

            Assert.False(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 0);
            Assert.True(rekeyStatus.RequiredUnsealKeys == 2);
            Assert.True(rekeyStatus.SecretShares == 0);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.Equal(string.Empty, rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            await UnauthenticatedVaultClient.InitiateRekeyAsync(5, 5);

            rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();
            Assert.True(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 5);
            Assert.True(rekeyStatus.RequiredUnsealKeys == 2);
            Assert.True(rekeyStatus.SecretShares == 5);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.NotNull(rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            await UnauthenticatedVaultClient.CancelRekeyAsync();
            rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();
            Assert.False(rekeyStatus.Started);
            Assert.True(rekeyStatus.SecretThreshold == 0);
            Assert.True(rekeyStatus.RequiredUnsealKeys == 2);
            Assert.True(rekeyStatus.SecretShares == 0);
            Assert.True(rekeyStatus.UnsealKeysProvided == 0);
            Assert.Equal(string.Empty, rekeyStatus.Nonce);
            Assert.False(rekeyStatus.Backup);

            await UnauthenticatedVaultClient.InitiateRekeyAsync(2, 2);
            rekeyStatus = await UnauthenticatedVaultClient.GetRekeyStatusAsync();

            var quick = await UnauthenticatedVaultClient.QuickRekeyAsync(_masterCredentials.MasterKeys, rekeyStatus.Nonce);
            Assert.True(quick.Complete);

            _masterCredentials.MasterKeys = quick.MasterKeys;
            _masterCredentials.Base64MasterKeys = quick.Base64MasterKeys;
        }

        private async Task RunLeaderApiTests()
        {
            var leader = await _authenticatedVaultClient.GetLeaderAsync();
            Assert.NotNull(leader);

            await _authenticatedVaultClient.StepDownActiveNodeAsync();

            leader = await _authenticatedVaultClient.GetLeaderAsync();
            Assert.NotNull(leader);
        }

        private async Task RunLeaseApiTests()
        {
            //var lease = await _authenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions
            //{
            //    CreateAsOrphan = true,
            //    LeaseTimeToLive = "1h",
            //    NoParent = true,
            //    Policies = new List<string> {"read"}
            //});

            //Assert.NotNull(lease.LeaseId);

            //var secret = await _authenticatedVaultClient.RenewSecretAsync(lease.LeaseId, 36000);
            //Assert.NotNull(secret.Data);

            // raja todo: run leasi Api tests with proper lease id.
            // also remove renewtokenasync
            await Task.FromResult(0);
        }

        private async Task RunAuditBackendMountApiTests()
        {
            var audits = await _authenticatedVaultClient.GetAllEnabledAuditBackendsAsync();
            Assert.False(audits.Data.Any());

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

            await _authenticatedVaultClient.EnableAuditBackendAsync(newFileAudit);

            // get audits
            var newAudits = await _authenticatedVaultClient.GetAllEnabledAuditBackendsAsync();
            Assert.Equal(audits.Data.Count() + 1, newAudits.Data.Count());

            // hash with audit
            var hash = await _authenticatedVaultClient.HashWithAuditBackendAsync(newFileAudit.MountPoint, "testinput");
            Assert.NotNull(hash);

            // disabled audit
            await _authenticatedVaultClient.DisableAuditBackendAsync(newFileAudit.MountPoint);

            // get audits
            var oldAudits = await _authenticatedVaultClient.GetAllEnabledAuditBackendsAsync();
            Assert.Equal(audits.Data.Count(), oldAudits.Data.Count());

            // syslog is not supported on windows. so no acceptance tests possible.
        }

        private async Task RunCapabilitiesApiTests()
        {
            var secret1 = await _authenticatedVaultClient.CreateTokenAsync(new TokenCreationOptions { NoParent = true });

            var caps =
                await _authenticatedVaultClient.GetTokenCapabilitiesAsync(secret1.AuthorizationInfo.ClientToken, "sys/mounts");
            Assert.NotNull(caps);

            var caps2 = await _authenticatedVaultClient.GetCallingTokenCapabilitiesAsync("sys/mounts");
            Assert.NotNull(caps2);

            var cap3 =
                await
                    _authenticatedVaultClient.GetTokenAccessorCapabilitiesAsync(
                        secret1.AuthorizationInfo.ClientTokenAccessor, "sys/mounts");
            Assert.NotNull(cap3);
        }

        private async Task RunPolicyApiTests()
        {
            var policies = (await _authenticatedVaultClient.GetAllPoliciesAsync()).ToList();
            Assert.True(policies.Any());

            var policy = await _authenticatedVaultClient.GetPolicyAsync(policies[0]);
            Assert.NotNull(policy);

            // write a new policy
            var newPolicy = new Policy
            {
                Name = "gubdu",
                Rules = "path \"sys/*\" {  policy = \"deny\" }"
            };

            await _authenticatedVaultClient.WritePolicyAsync(newPolicy);

            // get new policy
            var newPolicyGet = await _authenticatedVaultClient.GetPolicyAsync(newPolicy.Name);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

            // write updates to a new policy
            newPolicy.Rules = "path \"sys/*\" {  policy = \"read\" }";

            await _authenticatedVaultClient.WritePolicyAsync(newPolicy);

            // get new policy
            newPolicyGet = await _authenticatedVaultClient.GetPolicyAsync(newPolicy.Name);
            Assert.Equal(newPolicy.Rules, newPolicyGet.Rules);

            // delete policy
            await _authenticatedVaultClient.DeletePolicyAsync(newPolicy.Name);

            // get all policies
            var oldPolicies = (await _authenticatedVaultClient.GetAllPoliciesAsync()).ToList();
            Assert.Equal(policies.Count, oldPolicies.Count);
        }

        private async Task RunAuthenticationBackendMountApiTests()
        {
            // get Authentication backends
            var authenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.True(authenticationBackends.Data.Any());

            var mountConfig = await _authenticatedVaultClient.GetMountedAuthenticationBackendConfigurationAsync(authenticationBackends.Data.First().AuthenticationPath);
            Assert.NotNull(mountConfig);

            // enable new auth
            var newAuth = new AuthenticationBackend
            {
                AuthenticationPath = "github1",
                BackendType = AuthenticationBackendType.GitHub,
                Description = "Github auth - test cases"
            };

            await _authenticatedVaultClient.EnableAuthenticationBackendAsync(newAuth);

            string ttl = "11h";

            await
                _authenticatedVaultClient.TuneAuthenticationBackendConfigurationAsync(newAuth.AuthenticationPath,
                    new MountConfiguration { DefaultLeaseTtl = ttl, MaximumLeaseTtl = ttl });

            mountConfig =
                await
                    _authenticatedVaultClient.GetMountedAuthenticationBackendConfigurationAsync(
                        newAuth.AuthenticationPath);

            Assert.NotNull(mountConfig);

            // get all auths
            var newAuthenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.Equal(authenticationBackends.Data.Count() + 1, newAuthenticationBackends.Data.Count());

            // disable auth
            await _authenticatedVaultClient.DisableAuthenticationBackendAsync(newAuth.AuthenticationPath);

            // get all auths
            var oldAuthenticationBackends = await _authenticatedVaultClient.GetAllEnabledAuthenticationBackendsAsync();
            Assert.Equal(authenticationBackends.Data.Count(), oldAuthenticationBackends.Data.Count());
        }

        private async Task RunSecretBackendMountApiTests()
        {
            var secretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.True(secretBackends.Data.Any());

            var mountConfig = await _authenticatedVaultClient.GetMountedSecretBackendConfigurationAsync(secretBackends.Data.First().MountPoint);
            Assert.NotNull(mountConfig);

            // mount a new secret backend
            var newSecretBackend = new SecretBackend
            {
                BackendType = SecretBackendType.AWS,
                MountPoint = "aws1",
                Description = "e2e tests"
            };

            await _authenticatedVaultClient.MountSecretBackendAsync(newSecretBackend);

            string ttl = "10h";

            await
                _authenticatedVaultClient.TuneSecretBackendConfigurationAsync(newSecretBackend.MountPoint,
                    new MountConfiguration { DefaultLeaseTtl = ttl, MaximumLeaseTtl = ttl });

            // get secret backends
            var newSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count() + 1, newSecretBackends.Data.Count());

            // unmount
            await _authenticatedVaultClient.UnmountSecretBackendAsync(newSecretBackend.MountPoint);

            // get secret backends
            var oldSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count(), oldSecretBackends.Data.Count());

            // mount a new secret backend
            await _authenticatedVaultClient.MountSecretBackendAsync(newSecretBackend);

            // remount
            var newMountPoint = "aws2";
            await _authenticatedVaultClient.RemountSecretBackendAsync(newSecretBackend.MountPoint, newMountPoint);

            // get new secret backend config
            var config = await _authenticatedVaultClient.GetMountedSecretBackendConfigurationAsync(newMountPoint);
            Assert.NotNull(config);

            // unmount
            await _authenticatedVaultClient.UnmountSecretBackendAsync(newMountPoint);

            // quick
            secretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            await _authenticatedVaultClient.QuickMountSecretBackendAsync(SecretBackendType.AWS);
            newSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count() + 1, newSecretBackends.Data.Count());

            await _authenticatedVaultClient.QuickUnmountSecretBackendAsync(SecretBackendType.AWS);
            newSecretBackends = await _authenticatedVaultClient.GetAllMountedSecretBackendsAsync();
            Assert.Equal(secretBackends.Data.Count(), newSecretBackends.Data.Count());
        }

        private async Task RunInitApiTests()
        {
            await AssertInitializationStatusAsync(false);

            var health = await UnauthenticatedVaultClient.GetHealthStatusAsync();
            Assert.False(health.HealthCheckSucceeded);
            Assert.False(health.Initialized);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(uninitializedStatusCode: 300);
            Assert.False(health.HealthCheckSucceeded);
            Assert.False(health.Initialized);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(uninitializedStatusCode: 200);
            Assert.True(health.HealthCheckSucceeded);
            Assert.False(health.Initialized);

            await InitializeVaultAsync();
            await AssertInitializationStatusAsync(true);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync();
            Assert.False(health.HealthCheckSucceeded);
            Assert.True(health.Initialized);
        }

        private async Task RunSealApiTests()
        {
            await AssertSealStatusAsync(true);

            var health = await UnauthenticatedVaultClient.GetHealthStatusAsync();
            Assert.False(health.HealthCheckSucceeded);
            Assert.True(health.Sealed);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(sealedStatusCode: 300);
            Assert.False(health.HealthCheckSucceeded);
            Assert.True(health.Sealed);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(sealedStatusCode: 400);
            Assert.False(health.HealthCheckSucceeded);
            Assert.True(health.Sealed);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(sealedStatusCode: 200);
            Assert.True(health.HealthCheckSucceeded);
            Assert.True(health.Sealed);

            await UnsealAsync();
            await AssertSealStatusAsync(false);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync();
            Assert.True(health.HealthCheckSucceeded);
            Assert.False(health.Sealed);

            health = await UnauthenticatedVaultClient.GetHealthStatusAsync(activeStatusCode: 300);
            Assert.False(health.HealthCheckSucceeded);
            Assert.False(health.Sealed);

            await _authenticatedVaultClient.SealAsync();

            var sealStatus = await UnauthenticatedVaultClient.UnsealAsync(_masterCredentials.MasterKeys[0]);
            Assert.True(sealStatus.Sealed);
            Assert.False(sealStatus.Progress == 0);

            await UnauthenticatedVaultClient.UnsealAsync(resetCompletely: true);
            await AssertSealStatusAsync(true);

            await UnsealAsync();

            await _authenticatedVaultClient.SealAsync();
            await AssertSealStatusAsync(true);

            sealStatus = await UnauthenticatedVaultClient.QuickUnsealAsync(_masterCredentials.MasterKeys);
            Assert.False(sealStatus.Sealed);
        }

        private async Task RunGenerateRootApiTests()
        {
            var rootStatus = await UnauthenticatedVaultClient.GetRootTokenGenerationStatusAsync();
            Assert.False(rootStatus.Started);

            var otp = Convert.ToBase64String(Enumerable.Range(0, 16).Select(i => (byte)i).ToArray());
            rootStatus = await UnauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);

            Assert.True(rootStatus.Started);
            Assert.NotNull(rootStatus.Nonce);

            foreach (var masterKey in _masterCredentials.MasterKeys)
            {
                rootStatus = await UnauthenticatedVaultClient.ContinueRootTokenGenerationAsync(masterKey, rootStatus.Nonce);
            }

            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);

            rootStatus = await UnauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);

            rootStatus = await UnauthenticatedVaultClient.ContinueRootTokenGenerationAsync(_masterCredentials.MasterKeys[0], rootStatus.Nonce);
            Assert.True(rootStatus.Started);

            await UnauthenticatedVaultClient.CancelRootTokenGenerationAsync();

            rootStatus = await UnauthenticatedVaultClient.GetRootTokenGenerationStatusAsync();
            Assert.False(rootStatus.Started);

            rootStatus = await UnauthenticatedVaultClient.InitiateRootTokenGenerationAsync(otp);
            rootStatus =
                await
                    UnauthenticatedVaultClient.QuickRootTokenGenerationAsync(_masterCredentials.MasterKeys,
                        rootStatus.Nonce);

            Assert.True(rootStatus.Complete);
            Assert.NotNull(rootStatus.EncodedRootToken);
        }

        private async Task UnsealAsync()
        {
            SealStatus sealStatus = null;

            foreach (var masterKey in _masterCredentials.MasterKeys)
            {
                sealStatus = await UnauthenticatedVaultClient.UnsealAsync(masterKey);
            }

            Assert.False(sealStatus.Sealed);
            Assert.Equal(0, sealStatus.Progress);
            Assert.NotNull(sealStatus.ClusterId);
            Assert.NotNull(sealStatus.ClusterName);

            _authenticatedVaultClient = VaultClientFactory.CreateVaultClient(VaultUriWithPort, new TokenAuthenticationInfo(_masterCredentials.RootToken));
        }

        private async Task AssertSealStatusAsync(bool expected)
        {
            var actual = await UnauthenticatedVaultClient.GetSealStatusAsync();
            Assert.Equal(expected, actual.Sealed);
        }

        private async Task InitializeVaultAsync()
        {
            _masterCredentials = await UnauthenticatedVaultClient.InitializeAsync(2, 2);
            Assert.NotNull(_masterCredentials);
        }

        private async Task AssertInitializationStatusAsync(bool expectedStatus)
        {
            var actual = await UnauthenticatedVaultClient.GetInitializationStatusAsync();
            Assert.Equal(expectedStatus, actual);
        }

        private void StartupVaultServer()
        {
            if (!File.Exists(SetupData.VaultExeFullPath))
            {
                throw new Exception("Vault EXE full path does not exist: " + SetupData.VaultExeFullPath);
            }

            if (!File.Exists(VaultConfigPath))
            {
                throw new Exception("Vault acceptance tests config file does not exist: " + VaultConfigPath);
            }

            var vaultFolder = Path.GetDirectoryName(SetupData.VaultExeFullPath);
            var fileBackendsFullRootFolderPath = Path.Combine(vaultFolder, FileBackendsFolderName);
            var vaultConfigsFullRootFolderPath = Path.Combine(vaultFolder, VaultConfigsFolderName);

            if (!Directory.Exists(vaultConfigsFullRootFolderPath))
            {
                Directory.CreateDirectory(vaultConfigsFullRootFolderPath);
            }

            var fileBackendFolderName = Guid.NewGuid().ToString();
            var fileBackendFullFolderPath = Path.Combine(fileBackendsFullRootFolderPath, fileBackendFolderName);

            if (Directory.Exists(fileBackendFullFolderPath))
            {
                throw new Exception("A directory of the same name already exists. Please try a new run." +
                                    fileBackendFullFolderPath);
            }

            Directory.CreateDirectory(fileBackendFullFolderPath);

            var config =
                File.ReadAllText(VaultConfigPath)
                    .Replace(FileBackendPlaceHolder, fileBackendFullFolderPath)
                    .Replace("\\", "\\\\");
            var testConfigFileName = fileBackendFolderName + ".hcl";

            var testConfigFullPath = Path.Combine(vaultConfigsFullRootFolderPath, testConfigFileName);
            File.WriteAllText(testConfigFullPath, config);

            var startupCommand = "\"" + SetupData.VaultExeFullPath + "\" server -config \"" + testConfigFullPath + "\"";

            var procStartInfo = new ProcessStartInfo();

            procStartInfo.FileName = "cmd";
            procStartInfo.Arguments = "/k \"" + startupCommand + "\"";
            procStartInfo.WorkingDirectory = Path.GetPathRoot(vaultFolder);

            _vaultProcess = new Process();
            _vaultProcess.StartInfo = procStartInfo;
            _vaultProcess.Start();
        }

        private void ShutdownVaultServer()
        {
            if (_vaultProcess != null && !_vaultProcess.HasExited)
            {
                _vaultProcess.CloseMainWindow();
            }
        }
    }
}