// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.WaitStrategies;

namespace VaultSharp.Samples.TestHelper;

public static class VaultTestServer
{
    public static TestcontainersContainer BuildVaultServerContainer(int port = 8200, string address = "0.0.0.0",
        string rootTokenId = "testRoot", string containerName = "VaultServer")
    {
        var container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("vault:latest")
            .WithName(containerName)
            .WithPortBinding(port, 8200)
            .WithEnvironment("VAULT_DEV_ROOT_TOKEN_ID", rootTokenId)
            .WithEnvironment("VAULT_DEV_LISTEN_ADDRESS", $"{address}:8200")
            .WithEnvironment("VAULT_ADDR", $"http://{address}:8200")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8200));

        return container.Build();
    }
}