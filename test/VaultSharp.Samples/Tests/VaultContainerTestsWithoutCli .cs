// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using VaultSharp.Core;
using VaultSharp.Samples.TestHelper;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.AppRole.Models;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.GitHub.Models;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass.Models;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.Identity.Models;
using VaultSharp.V1.SystemBackend;
using Xunit;

namespace VaultSharp.Samples.Tests;

public class VaultContainerTestsWithoutCli
{
    [Fact]
    public async Task VaultApi_ReadWriteKv2_ReturnsSameInput()
    {
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleName, token_policies = new[] {policyName}},
            appRolePath);
        var roleIdResultTest = await rootClient.V1.Auth.AppRole.ReadRoleIdAsync(roleName, appRolePath);
        var secretIdResultTest = await rootClient.V1.Auth.AppRole.CreateSecretId(roleName, appRolePath);

        // login with appRole Auth
        IAuthMethodInfo appRoleAuthMethodInfo =
            new AppRoleAuthMethodInfo(appRolePath, roleIdResultTest.Data.Role_Id, secretIdResultTest.Data.Secret_Id);
        var vaultClientSettings = new VaultClientSettings(
            $"http://127.0.0.1:{port}",
            appRoleAuthMethodInfo);
        var appRoleClient = new VaultClient(vaultClientSettings);
        await appRoleClient.V1.Auth.PerformImmediateLogin();

        var token = (await appRoleClient.V1.Auth.Token.LookupSelfAsync()).Data.Policies;

        token.Should().ContainMatch("testpolicy");
    }

    [Fact]
    public async Task VaultApi_ReadAllAppRoles_ReturnsAllRoles()
    {
        const string roleNameA = "testrolea";
        const string roleNameB = "testroleb";
        const string appRolePath = "dev__testAppRole";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);

        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleNameA},
            appRolePath);

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleNameB},
            appRolePath);

        //rootClient.V1.Secrets.Identity.

        var appRolesSecret = await rootClient.V1.Auth.AppRole.ReadAllAppRoles(appRolePath);
        var appRoles = appRolesSecret.Data;

        appRoles.Keys.Should().ContainMatch(roleNameA);
        appRoles.Keys.Should().ContainMatch(roleNameB);
    }

    [Fact]
    public async Task VaultApi_CreateCustomRoleId_ReturnsCustomId()
    {
        const string roleName = "testRole";
        const string appRolePath = "dev__testAppRole";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const string customRoleId = "historian";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);

        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleName},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleName, customRoleId, appRolePath);

        var roleIdResultTest = await rootClient.V1.Auth.AppRole.ReadRoleIdAsync(roleName, appRolePath);
        roleIdResultTest.Data.Role_Id.Should().Match(customRoleId);
    }


    [Fact]
    public async Task VaultApi_AppRoleAuthWithResponseWrappedToken_TokenIsValid()
    {
        const string roleName = "testRole";
        const string appRolePath = "testAppRole";
        const string testSecretPath = "testSecrets";
        const string policyName = "testpolicy";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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

        // create test Role
        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleName, token_policies = new[] {policyName}},
            appRolePath);


        var roleId = (await rootClient.V1.Auth.AppRole.ReadRoleIdAsync(roleName, appRolePath)).Data.Role_Id;
        var responseWrappedTokenResponse =
            await rootClient.V1.Auth.AppRole.CreateResponseWrappedSecretId("10s", roleName, appRolePath);

        var responseWrappedToken = responseWrappedTokenResponse.WrapInfo.Token;

        var secretIdResultTest = await rootClient.V1.Auth.AppRole.CreateSecretId(roleName, appRolePath);

        // Authenticate with wrapping token
        IAuthMethodInfo wrappedTokenAuthMethod = new TokenAuthMethodInfo(responseWrappedToken);
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
            wrappedTokenAuthMethod = new TokenAuthMethodInfo(responseWrappedToken);
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

    [Fact(Skip = "Manual Github Token creation process")]
    public async Task VaultApi_UseUseGitHubAuth_TokenYieldsPolicy()
    {
        const string githubPath = "testGithub";
        const string teamName = "";
        const string organization = "testOrganization";
        const string userName = "";

        const string testSecretPath = "testSecrets";
        const string policyName = "testpolicy";
        const string tempToken = "PERSONAL TOKEN";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
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
        await rootClient.V1.Auth.GitHub.WriteGitHubConfig(
            new GitHubConfig {organization = organization, token_no_default_policy = true}, githubPath);

        var readConfig = await rootClient.V1.Auth.GitHub.ReadGitHubConfig(organization, githubPath);

        await rootClient.V1.Auth.GitHub.WriteGitHubTeamMap(new GitHubTeamMap {team_name = teamName, value = policyName},
            githubPath);
        var readTeamMap = await rootClient.V1.Auth.GitHub.ReadGitHubTeamMap(teamName, githubPath);

        await rootClient.V1.Auth.GitHub.WriteGitHubUserMap(new GitHubUserMap {user_name = userName, value = policyName},
            githubPath);
        var readUserMap = await rootClient.V1.Auth.GitHub.ReadGitHubUserMap(userName, githubPath);

        // login with github Auth
        IAuthMethodInfo gitHubAuthMethodInfo = new GitHubAuthMethodInfo(githubPath, tempToken);
        var vaultClientSettings = new VaultClientSettings(
            $"{vaultAdr}:{port}",
            gitHubAuthMethodInfo);
        var gitHubClient = new VaultClient(vaultClientSettings);
        await gitHubClient.V1.Auth.PerformImmediateLogin();

        var token = (await gitHubClient.V1.Auth.Token.LookupSelfAsync()).Data.Policies;

        token.Should().ContainMatch("testpolicy");
    }

    [Fact]
    public async Task VaultApi_CreateGithubGroupAlias_ReturnsCanonicalId()
    {
        const string githubPath = "testGithub";
        const string teamName = "test-developers";
        const string groupName = "developer";
        const string organization = "testOrganization";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;

        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = githubPath, Type = AuthMethodType.GitHub
        });

        await rootClient.V1.Auth.GitHub.WriteGitHubConfig(
            new GitHubConfig {organization = organization, token_no_default_policy = true}, githubPath);

        var readConfig = await rootClient.V1.Auth.GitHub.ReadGitHubConfig(organization, githubPath);

        await rootClient.V1.Auth.GitHub.WriteGitHubTeamMap(new GitHubTeamMap {team_name = teamName},
            githubPath);
        var readTeamMap = await rootClient.V1.Auth.GitHub.ReadGitHubTeamMap(teamName, githubPath);

        var getAuthBackendResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        getAuthBackendResponse.TryGetValue($"{githubPath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var createGroupResponse =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand
            {
                Name = groupName, Type = "external"
            })).Data;

        var response =
            (await rootClient.V1.Secrets.Identity.CreateGroupAlias(new CreateGroupAliasCommand
            {
                Name = teamName, CanonicalId = createGroupResponse.Id, MountAccessor = accessor
            })).Data;

        response.CanonicalId.Should().Match(createGroupResponse.Id);
    }

    [Fact]
    public async Task VaultApi_CreateEntityById_ReturnsEntityName()
    {
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var response =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityById(new CreateOrUpdateEntityByIdCommand
            {
                Name = entityName, Disabled = false
            })).Data;

        response.Name.Should().Match(entityName);
    }

    [Fact]
    public async Task VaultApi_CreateGroup_ReturnsGroupName()
    {
        const string groupName = "testGroupName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var response =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand {Name = groupName})).Data;

        response.Name.Should().Match(groupName);
    }

    [Fact]
    public async Task VaultApi_CreateEntityByName_ReturnsEntityName()
    {
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var response =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(new CreateOrUpdateEntityByNameCommand
            {
                Name = entityName, Disabled = false
            })).Data;

        response.Name.Should().Match(entityName);
    }

    [Fact]
    public async Task VaultApi_CreateUserPassUser_ReturnsEntityName()
    {
        const string userName = "testUserA";
        const string testPassword = "testUserA";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const string userPassPath = "userPassTestPath";
        const string TokenMaxTtl = "3000";
        const string TokenTtl = "123";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = userPassPath, Type = AuthMethodType.UserPass
        });

        await rootClient.V1.Auth.UserPass.CreateOrUpdateUser(
            new UserPassUser
            {
                Username = userName, Password = testPassword, TokenMaxTtl = TokenMaxTtl, TokenTtl = TokenTtl
            }, userPassPath);

        var readUserResponse = (await rootClient.V1.Auth.UserPass.ReadUser(userName, userPassPath)).Data;

        readUserResponse.TokenMaxTtl.Should().Match(TokenMaxTtl);
        readUserResponse.TokenTtl.Should().Match(TokenTtl);
    }

    [Fact]
    public async Task VaultApi_ListUser_ReturnsUsers()
    {
        const string userNameA = "testUserA";
        const string userNameB = "testUserB";

        const string testPasswordA = "testUserA";
        const string testPasswordB = "testUserB";

        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const string userPassPath = "userPassTestPath";
        const string TokenMaxTtl = "3000";
        const string TokenTtl = "123";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = userPassPath, Type = AuthMethodType.UserPass
        });

        await rootClient.V1.Auth.UserPass.CreateOrUpdateUser(
            new UserPassUser
            {
                Username = userNameA, Password = testPasswordA, TokenMaxTtl = TokenMaxTtl, TokenTtl = TokenTtl
            }, userPassPath);

        await rootClient.V1.Auth.UserPass.CreateOrUpdateUser(
            new UserPassUser
            {
                Username = userNameB, Password = testPasswordB, TokenMaxTtl = TokenMaxTtl, TokenTtl = TokenTtl
            }, userPassPath);


        var readUserResponse = (await rootClient.V1.Auth.UserPass.ListUsers(userPassPath)).Data.Keys.ToList();

        readUserResponse.Should().ContainEquivalentOf(userNameA.ToLower());
        readUserResponse.Should().ContainEquivalentOf(userNameB.ToLower());
    }

    [Fact]
    public async Task VaultApi_UpdateAndDeleteUser_ReturnsEmtpyList()
    {
        const string userNameA = "testUserA";
        const string testPolicy = "testPolicy";

        const string testPasswordA = "testpasswordxy";
        const string testPasswordB = "testpasswordxyz3";

        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const string userPassPath = "userPassTestPath";
        const string TokenMaxTtl = "3000";
        const string TokenTtl = "123";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = userPassPath, Type = AuthMethodType.UserPass
        });

        await rootClient.V1.Auth.UserPass.CreateOrUpdateUser(
            new UserPassUser
            {
                Username = userNameA, Password = testPasswordA, TokenMaxTtl = TokenMaxTtl, TokenTtl = TokenTtl
            }, userPassPath);

        await rootClient.V1.Auth.UserPass.UpdatePasswordOnUser(userNameA, testPasswordB, userPassPath);

        await rootClient.V1.Auth.UserPass.UpdatePoliciesOnUser(userNameA, new List<string> {testPolicy},
            userPassPath);

        var listUserBeforeResponse = (await rootClient.V1.Auth.UserPass.ListUsers(userPassPath)).Data.Keys.ToList();

        var readUserResponse = (await rootClient.V1.Auth.UserPass.ReadUser(userNameA, userPassPath)).Data;

        await rootClient.V1.Auth.UserPass.DeleteUser(userNameA, userPassPath);

        var listUserAfterResponse = (await rootClient.V1.Auth.UserPass.ListUsers(userPassPath)).Data.Keys.ToList();

        listUserBeforeResponse.Should().ContainEquivalentOf(userNameA.ToLower());
        readUserResponse.TokenPolicies.Should().ContainEquivalentOf(testPolicy.ToLower());
        listUserAfterResponse.Should().BeEmpty();
    }

    [Fact]
    public async Task VaultApi_CreateAlias_ReturnsCanonicalId()
    {
        const string roleName = "testrolea";
        const string appRolePath = "dev__testAppRole";
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var result =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(
                new CreateOrUpdateEntityByNameCommand {Name = entityName, Disabled = false})).Data;
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleName},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleName, roleName, appRolePath);
        var getAuthBackendResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        getAuthBackendResponse.TryGetValue($"{appRolePath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var response =
            (await rootClient.V1.Secrets.Identity.CreateEntityAlias(new CreateAliasCommand
            {
                Name = roleName, CanonicalId = result.Id, MountAccessor = accessor
            })).Data;

        response.CanonicalId.Should().Match(result.Id);
    }

    [Fact]
    public async Task VaultApi_ReadEntityById_ReturnsEntity()
    {
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var response =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(new CreateOrUpdateEntityByNameCommand
            {
                Name = entityName, Disabled = false
            })).Data;

        var readResponse = (await rootClient.V1.Secrets.Identity.ReadEntityById(response.Id)).Data;

        readResponse.Name.Should().Match(entityName);
    }

    [Fact]
    public async Task VaultApi_ReadEntityAliasById_ReturnsAppRoleRole()
    {
        const string roleName = "testrolea";
        const string appRolePath = "dev__testAppRole";
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var result =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(
                new CreateOrUpdateEntityByNameCommand {Name = entityName, Disabled = false})).Data;
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleName},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleName, roleName, appRolePath);
        var appRoleResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        appRoleResponse.TryGetValue($"{appRolePath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var response =
            (await rootClient.V1.Secrets.Identity.CreateEntityAlias(new CreateAliasCommand
            {
                Name = roleName, CanonicalId = result.Id, MountAccessor = accessor
            })).Data;

        var readResponse = (await rootClient.V1.Secrets.Identity.ReadEntityAliasById(response.Id)).Data;

        readResponse.Name.Should().Match(roleName);
    }

    [Fact]
    public async Task VaultApi_UpdateEntityAliasById_ReturnsNewAppRoleRole()
    {
        const string roleNameA = "dev__testrolea";
        const string roleNameB = "dev__testroleb";
        const string appRolePath = "dev__testAppRole";
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var result =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(
                new CreateOrUpdateEntityByNameCommand {Name = entityName, Disabled = false})).Data;
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleNameA},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleNameA, roleNameA, appRolePath);

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleNameB},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleNameB, roleNameB, appRolePath);

        var appRoleResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        appRoleResponse.TryGetValue($"{appRolePath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var response =
            (await rootClient.V1.Secrets.Identity.CreateEntityAlias(new CreateAliasCommand
            {
                Name = roleNameA, CanonicalId = result.Id, MountAccessor = accessor
            })).Data;

        var beforeReadResponse = (await rootClient.V1.Secrets.Identity.ReadEntityAliasById(response.Id)).Data;

        var updateResponse = (await rootClient.V1.Secrets.Identity.UpdateEntityAliasById(response.Id,
            new CreateAliasCommand {Name = roleNameB, CanonicalId = result.Id, MountAccessor = accessor})).Data;

        var afterReadResponse = (await rootClient.V1.Secrets.Identity.ReadEntityAliasById(updateResponse.Id)).Data;

        var readEntityResponse = (await rootClient.V1.Secrets.Identity.ReadEntityById(result.Id)).Data;

        beforeReadResponse.Name.Should().Match(roleNameA);
        afterReadResponse.Name.Should().Match(roleNameB);
    }

    [Fact]
    public async Task VaultApi_UpdateGroup_ReturnsNewName()
    {
        const string groupNameA = "groupNameA";
        const string groupNameB = "groupNameB";
        const string groupNameC = "groupNameC";
        const string groupNameD = "groupNameD";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var initialResponse =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand {Name = groupNameA})).Data;

        var initialReadResponse = (await rootClient.V1.Secrets.Identity.ReadGroupById(initialResponse.Id)).Data;

        await rootClient.V1.Secrets.Identity.UpdateGroupById(initialResponse.Id,
            new CreateGroupCommand {Name = groupNameB});

        var firstReadResponse = (await rootClient.V1.Secrets.Identity.ReadGroupById(initialResponse.Id)).Data;

        await rootClient.V1.Secrets.Identity.UpdateGroupById(initialResponse.Id,
            new CreateGroupCommand {Name = groupNameC});

        var secondReadResponse = (await rootClient.V1.Secrets.Identity.ReadGroupByName(groupNameC)).Data;

        var createAdditionalGroup =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand {Name = groupNameD})).Data;

        var listById = (await rootClient.V1.Secrets.Identity.ListGroupsById()).Data.Keys;
        var listByName = (await rootClient.V1.Secrets.Identity.ListGroupsByName()).Data.Keys;

        await rootClient.V1.Secrets.Identity.DeleteGroupById(initialReadResponse.Id);
        await rootClient.V1.Secrets.Identity.DeleteGroupByName(groupNameD);

        var listByIdAfterDelete = (await rootClient.V1.Secrets.Identity.ListGroupsById()).Data.Keys;
        var listByNameAfterDelete = (await rootClient.V1.Secrets.Identity.ListGroupsByName()).Data.Keys;

        initialReadResponse.Name.Should().Match(groupNameA);
        firstReadResponse.Name.Should().Match(groupNameB);
        secondReadResponse.Name.Should().Match(groupNameC);
        secondReadResponse.Name.Should().Match(groupNameC);
        listById.Should().ContainEquivalentOf(initialReadResponse.Id);
        listByName.Should().ContainEquivalentOf(groupNameD);
        listByIdAfterDelete.Should().BeEmpty();
        listByNameAfterDelete.Should().BeEmpty();
    }

    [Fact]
    public async Task VaultApi_DeleteEntityAliasById_IsNotReturnedInList()
    {
        const string roleNameA = "testrolea";
        const string appRolePath = "dev__testAppRole";
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        var result =
            (await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(
                new CreateOrUpdateEntityByNameCommand {Name = entityName, Disabled = false})).Data;
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = appRolePath, Type = AuthMethodType.AppRole
        });

        await rootClient.V1.Auth.AppRole.WriteAppRoleRoleAsync(
            new AppRoleRole {role_name = roleNameA},
            appRolePath);
        await rootClient.V1.Auth.AppRole.WriteCustomAppRoleId(roleNameA, roleNameA, appRolePath);

        var appRoleResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        appRoleResponse.TryGetValue($"{appRolePath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var response =
            (await rootClient.V1.Secrets.Identity.CreateEntityAlias(new CreateAliasCommand
            {
                Name = roleNameA, CanonicalId = result.Id, MountAccessor = accessor
            })).Data;

        var beforeDeleteResponse = (await rootClient.V1.Secrets.Identity.ListEntityAliasesById()).Data.Keys.ToList();

        await rootClient.V1.Secrets.Identity.DeleteEntityAliasById(response.Id);

        var afterDeleteResponse = (await rootClient.V1.Secrets.Identity.ListEntityAliasesById()).Data.Keys;

        beforeDeleteResponse.Should().ContainEquivalentOf(response.Id);
        afterDeleteResponse.Should().NotContainEquivalentOf(response.Id);
    }

    [Fact]
    public async Task VaultApi_DeleteGroupAliasById_IsNotReturnedInList()
    {
        const string githubPath = "testGithub";
        const string teamName = "acceliox-developers";
        const string groupName = "developer";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = githubPath, Type = AuthMethodType.GitHub
        });

        await rootClient.V1.Auth.GitHub.WriteGitHubConfig(
            new GitHubConfig {organization = "acceliox", token_no_default_policy = true}, githubPath);

        var readConfig = await rootClient.V1.Auth.GitHub.ReadGitHubConfig("acceliox", githubPath);

        await rootClient.V1.Auth.GitHub.WriteGitHubTeamMap(new GitHubTeamMap {team_name = teamName},
            githubPath);
        var readTeamMap = await rootClient.V1.Auth.GitHub.ReadGitHubTeamMap(teamName, githubPath);

        var getAuthBackendResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        getAuthBackendResponse.TryGetValue($"{githubPath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var createGroupResponse =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand
            {
                Name = groupName, Type = "external"
            })).Data;

        var response =
            (await rootClient.V1.Secrets.Identity.CreateGroupAlias(new CreateGroupAliasCommand
            {
                Name = teamName, CanonicalId = createGroupResponse.Id, MountAccessor = accessor
            })).Data;

        var beforeDeleteResponse = (await rootClient.V1.Secrets.Identity.ListGroupAliasesById()).Data.Keys.ToList();

        await rootClient.V1.Secrets.Identity.DeleteGroupAliasById(response.Id);

        var afterDeleteResponse = (await rootClient.V1.Secrets.Identity.ListGroupAliasesById()).Data.Keys;

        beforeDeleteResponse.Should().ContainEquivalentOf(response.Id);
        afterDeleteResponse.Should().NotContainEquivalentOf(response.Id);
    }

    [Fact]
    public async Task VaultApi_UpdateGroupAliasById_ReturnsNewTeam()
    {
        const string githubPath = "testGithub";
        const string teamNameA = "test-developers-teamA";
        const string teamNameB = "test-developers-teamB";
        const string organization = "testOrganization";
        const string groupName = "developer";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.System.MountAuthBackendAsync(new AuthMethod
        {
            Path = githubPath, Type = AuthMethodType.GitHub
        });

        await rootClient.V1.Auth.GitHub.WriteGitHubConfig(
            new GitHubConfig {organization = organization, token_no_default_policy = true}, githubPath);

        var readConfig = await rootClient.V1.Auth.GitHub.ReadGitHubConfig(organization, githubPath);

        await rootClient.V1.Auth.GitHub.WriteGitHubTeamMap(new GitHubTeamMap {team_name = teamNameA},
            githubPath);
        var readTeamMap = await rootClient.V1.Auth.GitHub.ReadGitHubTeamMap(teamNameA, githubPath);

        var getAuthBackendResponse = (await rootClient.V1.System.GetAuthBackendsAsync()).Data;
        getAuthBackendResponse.TryGetValue($"{githubPath}/", out var authMethod);
        var accessor = authMethod.Accessor;
        var createGroupResponse =
            (await rootClient.V1.Secrets.Identity.CreateGroup(new CreateGroupCommand
            {
                Name = groupName, Type = "external"
            })).Data;

        var response =
            (await rootClient.V1.Secrets.Identity.CreateGroupAlias(new CreateGroupAliasCommand
            {
                Name = teamNameA, CanonicalId = createGroupResponse.Id, MountAccessor = accessor
            })).Data;

        var beforeDeleteResponse = (await rootClient.V1.Secrets.Identity.ReadGroupAliasById(response.Id)).Data;

        var updateResponse =
            (await rootClient.V1.Secrets.Identity.UpdateGroupAliasById(response.Id,
                new CreateGroupAliasCommand
                {
                    Name = teamNameB, CanonicalId = createGroupResponse.Id, MountAccessor = accessor
                })).Data;

        var afterUpdateResponse = (await rootClient.V1.Secrets.Identity.ReadGroupAliasById(response.Id)).Data;
        var readGroupResponse = (await rootClient.V1.Secrets.Identity.ReadGroupByName(groupName)).Data;
        beforeDeleteResponse.Name.Should().BeEquivalentTo(teamNameA);
        afterUpdateResponse.Name.Should().BeEquivalentTo(teamNameB);
    }

    [Fact]
    public async Task VaultApi_ReadEntityByName_ReturnsEntity()
    {
        const string entityName = "testEntityName";
        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(new CreateOrUpdateEntityByNameCommand
        {
            Name = entityName, Disabled = false
        });

        var readResponse = (await rootClient.V1.Secrets.Identity.ReadEntityByName(entityName)).Data;

        readResponse.Name.Should().Match(entityName);
    }

    [Fact]
    public async Task VaultApi_ListEntitiesByName_ReturnsAllEntities()
    {
        const string entityNameA = "testEntityNameA";
        const string entityNameB = "testEntityNameB";

        const string rootTokenId = "testRoot";
        const string containerName = "VaultTestsWithoutCLI";
        const int port = 8220;
        await using var container =
            VaultTestServer.BuildVaultServerContainer(port, rootTokenId: rootTokenId, containerName: containerName);
        await container.StartAsync();
        var rootClient = await CreateVaultRootClient(port);
        await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(new CreateOrUpdateEntityByNameCommand
        {
            Name = entityNameA, Disabled = false
        });

        await rootClient.V1.Secrets.Identity.CreateOrUpdateEntityByName(new CreateOrUpdateEntityByNameCommand
        {
            Name = entityNameB, Disabled = false
        });

        var readResponse = (await rootClient.V1.Secrets.Identity.ListEntitiesByName()).Data.Keys.ToList();

        readResponse.Should().ContainMatch(entityNameA);
        readResponse.Should().ContainMatch(entityNameB);
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