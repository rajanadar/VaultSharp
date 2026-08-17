using System.Runtime.CompilerServices;
using System.Text.Json;
using VaultSharp.Testing;

try
{
    AotApiScenario.Require(
        !RuntimeFeature.IsDynamicCodeSupported,
        "The canary must run as a native binary.");
    AotApiScenario.Require(
        !JsonSerializer.IsReflectionEnabledByDefault,
        "JSON reflection must be disabled.");

    await AotApiScenario.RunAffectedApiPathsAsync();
    await AotApiScenario.RunConsumerDtoPathsAsync();
    Console.WriteLine("NativeAOT canary passed.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
    return 1;
}
