// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

namespace VaultSharp.Samples.TestHelper;

/// <summary>
///     A command exec result.
/// </summary>
public readonly struct ExecResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ExecResult" /> struct.
    /// </summary>
    /// <param name="stdout">The stdout output.</param>
    /// <param name="stderr">The stderr output.</param>
    /// <param name="exitCode">The exit code.</param>
    public ExecResult(string stdout, string stderr, long exitCode)
    {
        Stdout = stdout;
        Stderr = stderr;
        ExitCode = exitCode;
    }

    /// <summary>
    ///     Gets the stdout output.
    /// </summary>
    public string Stdout { get; }

    /// <summary>
    ///     Gets the stderr output.
    /// </summary>
    public string Stderr { get; }

    /// <summary>
    ///     Gets the exit code.
    /// </summary>
    public long ExitCode { get; }
}