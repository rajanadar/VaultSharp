// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using VaultSharp.Core;
using VaultSharp.Samples.TestHelper;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend;
using Xunit;

namespace VaultSharp.Samples.Tests;

public class VaultContainerTestsWithCli
{
    [Fact]
    public async Task VaultApi_ReadWriteKv2_ReturnsSameInput()
    {
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await MountKv2SecretsEngine(rootClient, "testSecrets");
        var secretData = new Dictionary<string, object>
        {
            {"password", "testPass"}, {"additionalInfo", "testInfo"}, {"user", "someUser"}
        };


        await rootClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
            "/test/kv2",
            secretData,
            mountPoint: "testSecrets"
        );


        (await rootClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                "/test/kv2",
                mountPoint: "testSecrets"
            ))
            .Data.Data.Should()
            .BeEquivalentTo(secretData);
    }

    [Fact]
    public async Task VaultApi_WriteDtoToKv2_ReturnsSameInput()
    {
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await MountKv2SecretsEngine(rootClient, "testSecrets");
        var data = new TestDto
        {
            TestString1 = "Hallo",
            TestString2 = "TestValues",
            TestUint = 123,
            TestInt = 456,
            TestDouble = 78.910
        };
        var secretData = new Dictionary<string, object> {{"password", data}};

        await rootClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
            "/test/kv2",
            secretData,
            mountPoint: "testSecrets"
        );

        var resultDictionary = (await rootClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            "/test/kv2",
            mountPoint: "testSecrets"
        )).Data.Data;
        var result = resultDictionary.ToType<TestDto>("password");
        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task VaultApi_WriteMultiLevelDtoToKv2_ReturnsSameInput()
    {
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await MountKv2SecretsEngine(rootClient, "testSecrets");
        var data = new MultiLevelDto
        {
            SecondLevelDto =
                new TestDto
                {
                    TestString1 = "Hallo",
                    TestString2 = "TestValues",
                    TestUint = 123,
                    TestInt = 456,
                    TestDouble = 78.910
                },
            TestString = "TestValues",
            TestUint = 123,
            TestInt = 456,
            TestDouble = 78.910,
            RecursiveMultiLevelDto = new MultiLevelDto {SecondLevelDto = new TestDto {TestDouble = 999, TestInt = -1}}
        };
        var secretData = new Dictionary<string, object> {{"password", data}};

        await rootClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
            "/test/kv2",
            secretData,
            mountPoint: "testSecrets"
        );

        var resultDictionary = (await rootClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            "/test/kv2",
            mountPoint: "testSecrets"
        )).Data.Data;
        var result = resultDictionary.ToType<MultiLevelDto>("password");
        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task VaultApi_ReadWriteKv1_ReturnsSameInput()
    {
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await MountKv1SecretsEngine(rootClient, "testSecrets");


        var secretData = new Dictionary<string, object>
        {
            {"password", "testPass"}, {"additionalInfo", "testInfo"}, {"user", "someUser"}
        };


        await rootClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(
            "/test/kv1",
            secretData,
            "testSecrets"
        );


        (await rootClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(
                "/test/kv1",
                "testSecrets"
            ))
            .Data.Should()
            .BeEquivalentTo(secretData);
    }


    [Fact]
    public async Task VaultApi_SetAppRolePermissions_TokenYieldsPolicy()
    {
        const string roleName = "testRole";
        const string appRolePath = "testAppRole";
        const string testSecretPath = "testSecrets";
        const string policyName = "testpolicy";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });
        // create read only permission policy
        await rootClient.V1.System.WritePolicyAsync(new Policy
        {
            Name = policyName,
            Rules =
                $"# read only permission for test secrets:\r\npath \"{testSecretPath}/data/*\" {{\r\n  capabilities = [\"read\"]\r\n}}"
        });
        // CLI Interactions
        var loginResult =
            await container.ExecCommandWithResult(new List<string> {"vault", "login", "testRoot", "-no-store=true"});
        var createTestRole = await container.ExecCommandWithResult(new List<string>
        {
            "vault",
            "write",
            $"auth/{appRolePath}/role/{roleName}",
            $"role_name={roleName}",
            $"token_policies={policyName}"
        });
        var roleIdResult =
            await container.ExecCommandWithResult(new List<string>
            {
                "vault", "read", $"auth/{appRolePath}/role/{roleName}/role-id"
            });
        var secretIdResult = await container.ExecCommandWithResult(new List<string>
        {
            "vault", "write", "-force", $"auth/{appRolePath}/role/{roleName}/secret-id"
        });
        TryGetUuid(roleIdResult.Stdout, out var roleId);
        TryGetUuid(secretIdResult.Stdout, out var secretId);
        // login with appRole Auth
        IAuthMethodInfo appRoleAuthMethodInfo = new AppRoleAuthMethodInfo(appRolePath, roleId, secretId);
        var vaultClientSettings = new VaultClientSettings(
            $"http://127.0.0.1:{8210}",
            appRoleAuthMethodInfo);
        var appRoleClient = new VaultClient(vaultClientSettings);
        await appRoleClient.V1.Auth.PerformImmediateLogin();

        var token = (await appRoleClient.V1.Auth.Token.LookupSelfAsync()).Data.Policies;

        token.Should().ContainMatch("testpolicy");
    }

    [Fact]
    public async Task VaultApi_AppRoleAuthWithResponseWrappedToken_TokenIsValid()
    {
        const string roleName = "testRole";
        const string appRolePath = "testAppRole";
        const string testSecretPath = "testSecrets";
        const string policyName = "testpolicy";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        const string vaultAdr = "http://127.0.0.1";

        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);

        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });
        // create read only permission policy
        await rootClient.V1.System.WritePolicyAsync(new Policy
        {
            Name = policyName,
            Rules =
                $"# read only permission for test secrets:\r\npath \"{testSecretPath}/data/*\" {{\r\n  capabilities = [\"read\"]\r\n}}"
        });
        // CLI Interactions
        var loginResult =
            await container.ExecCommandWithResult(new List<string> {"vault", "login", "testRoot", "-no-store=true"});
        var createTestRole = await container.ExecCommandWithResult(new List<string>
        {
            "vault",
            "write",
            $"auth/{appRolePath}/role/{roleName}",
            $"role_name={roleName}",
            $"token_policies={policyName}"
        });
        var roleIdResult =
            await container.ExecCommandWithResult(new List<string>
            {
                "vault", "read", $"auth/{appRolePath}/role/{roleName}/role-id"
            });
        TryGetUuid(roleIdResult.Stdout, out var roleId);
        // create Response Wrapped Token for Secret Id ( update permission needed on "auth/{appRolePath}/role/{roleName}/secret-id" )
        var wrappedTokenResult = await container.ExecCommandWithResult(new List<string>
        {
            "vault",
            "write",
            "-wrap-ttl=10s",
            "-force",
            $"auth/{appRolePath}/role/{roleName}/secret-id"
        });

        var splitResult = wrappedTokenResult.Stdout.Split();
        var wrappingToken = splitResult
            .Where(x => x.Length > 3)
            .FirstOrDefault(x => x[..3] == "hvs");

        // Authenticate with wrapping token
        IAuthMethodInfo wrappedTokenAuthMethod = new TokenAuthMethodInfo(wrappingToken);
        var vaultClientSettingsForUnwrapping =
            new VaultClientSettings($"{vaultAdr}:{port}", wrappedTokenAuthMethod);
        IVaultClient vaultClientForUnwrapping = new VaultClient(vaultClientSettingsForUnwrapping);
        // Use Wrapping token auth to unwrap secretId
        var secretIdData = await vaultClientForUnwrapping.V1.System
            .UnwrapWrappedResponseDataAsync<Dictionary<string, object>>(null);
        var secretId = secretIdData.Data["secret_id"].As<string>();

        IEnumerable<string> apiErrors = new List<string>();
        // try unwrap token a second time --> needs to throw exception
        try
        {
            wrappedTokenAuthMethod = new TokenAuthMethodInfo(wrappingToken);
            vaultClientSettingsForUnwrapping =
                new VaultClientSettings($"{vaultAdr}:{port}", wrappedTokenAuthMethod);
            vaultClientForUnwrapping = new VaultClient(vaultClientSettingsForUnwrapping);
            secretIdData = await vaultClientForUnwrapping.V1.System
                .UnwrapWrappedResponseDataAsync<Dictionary<string, object>>(null);
            var secondSecretId = secretIdData.Data["secret_id"].As<string>();
        }
        catch (VaultApiException e)
        {
            apiErrors = e.ApiErrors;
        }

        // login with appRole Auth
        IAuthMethodInfo appRoleAuthMethodInfo = new AppRoleAuthMethodInfo(appRolePath, roleId, secretId);
        var vaultClientSettings = new VaultClientSettings(
            $"{vaultAdr}:{port}",
            appRoleAuthMethodInfo);
        var appRoleClient = new VaultClient(vaultClientSettings);
        await appRoleClient.V1.Auth.PerformImmediateLogin();

        var token = (await appRoleClient.V1.Auth.Token.LookupSelfAsync()).Data.Policies;

        token.Should().ContainMatch("testpolicy");
        apiErrors.Should().Contain("wrapping token is not valid or does not exist");
    }

    [Fact(Skip = "Manual Token creation process")]
    public async Task VaultApi_UseUseGitHubAuth_TokenYieldsPolicy()
    {
        const string githubPath = "testGithub";
        const string teamName = "acceliox-developers";
        const string testSecretPath = "testSecrets";
        const string policyName = "testpolicy";
        const string tempToken = "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";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithCLI";
        const int port = 8210;
        const string vaultAdr = "http://127.0.0.1";

        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = githubPath, Type = AuthMethodType.GitHub
        });
        // create read only permission policy
        await rootClient.V1.System.WritePolicyAsync(new Policy
        {
            Name = policyName,
            Rules =
                $"# read only permission for test secrets:\r\npath \"{testSecretPath}/data/*\" {{\r\n  capabilities = [\"read\"]\r\n}}"
        });
        // CLI Interactions
        var loginResult =
            await container.ExecCommandWithResult(new List<string> {"vault", "login", "testRoot", "-no-store=true"});
        var configureCompany = await container.ExecCommandWithResult(new List<string>
        {
            "vault", "write", $"auth/{githubPath}/config", "organization=acceliox"
        });
        var applyDevPolicy = await container.ExecCommandWithResult(new List<string>
        {
            "vault", "write", $"auth/{githubPath}/map/teams/{teamName}", $"value={policyName}"
        });

        // login with appRole Auth
        IAuthMethodInfo gitHubAuthMethodInfo = new GitHubAuthMethodInfo(githubPath, tempToken);
        var vaultClientSettings = new VaultClientSettings(
            $"{vaultAdr}:{port}",
            gitHubAuthMethodInfo);
        var gitHubClient = new VaultClient(vaultClientSettings);
        await gitHubClient.V1.Auth.PerformImmediateLogin();

        var token = (await gitHubClient.V1.Auth.Token.LookupSelfAsync()).Data.Policies;

        token.Should().ContainMatch("testpolicy");
    }


    private static bool TryGetUuid(string input, out string? guid)
    {
        var match = Regex.Match(input,
            @"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}?");
        if (match.Success)
        {
            guid = match.Value;
            return true;
        }

        guid = null;
        return false;
    }


    private static async Task<IVaultClient> CreateVaultRootClient(int port = 8200, string rootTokenId = "testRoot")
    {
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(rootTokenId);
        var vaultClientSettings = new VaultClientSettings(
            $"http://127.0.0.1:{port}",
            authMethod);
        return new VaultClient(vaultClientSettings);
    }

    private static async Task MountKv2SecretsEngine(IVaultClient client, string path)
    {
        var kv2SecretsEngine = new SecretsEngine
        {
            Type = SecretsEngineType.KeyValueV2,
            Config = new Dictionary<string, object> {{"version", "2"}},
            Path = path
        };

        await client.V1.System.MountSecretBackendAsync(kv2SecretsEngine);
    }

    private static async Task MountKv1SecretsEngine(IVaultClient client, string path)
    {
        var kv1SecretsEngine = new SecretsEngine
        {
            Type = SecretsEngineType.KeyValueV1,
            Config = new Dictionary<string, object> {{"version", "1"}},
            Path = path
        };

        await client.V1.System.MountSecretBackendAsync(kv1SecretsEngine);
    }
}