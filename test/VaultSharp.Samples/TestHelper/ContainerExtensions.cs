// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Containers.Modules;

namespace VaultSharp.Samples.TestHelper;

public static class ContainerExtensions
{
    public static async Task<ExecResult> ExecCommandWithResult(this TestcontainersContainer container,
        List<string> command)
    {
        using var client = new DockerClientConfiguration()
            .CreateClient();

        var execCreateParameters = new ContainerExecCreateParameters
        {
            Cmd = command, AttachStdout = true, AttachStderr = true
        };

        var ct = new CancellationToken();
        var execCreateResponse = await client.Exec.ExecCreateContainerAsync(container.Id, execCreateParameters, ct)
            .ConfigureAwait(false);
        ExecResult execResult;
        using (var stdOutAndErrStream = await client.Exec
                   .StartAndAttachContainerExecAsync(execCreateResponse.ID, false, ct)
                   .ConfigureAwait(false))
        {
            var (stdout, stderr) = await stdOutAndErrStream.ReadOutputToEndAsync(ct)
                .ConfigureAwait(false);

            var execInspectResponse = await client.Exec.InspectContainerExecAsync(execCreateResponse.ID, ct)
                .ConfigureAwait(false);

            return new ExecResult(stdout, stderr, execInspectResponse.ExitCode);
        }
    }
}