using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using VaultSharp;
using VaultSharp.Testing;
using VaultSharp.V1.AuthMethods.Token;

try
{
    VerifyClosedPublicResponseRoots();
    await AotApiScenario.RunAffectedApiPathsAsync();
    await AotApiScenario.RunConsumerDtoPathsAsync();
    Console.WriteLine("All NativeAOT regression tests passed.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
    return 1;
}

static void VerifyClosedPublicResponseRoots()
{
    // Snapshot of closed Task<T> roots exposed by beta1; API additions must update the context and this count.
    const int ExpectedClosedPublicResponseRootCount = 142;

    var settings = new VaultClientSettings(
        "https://vault.invalid",
        new TokenAuthMethodInfo("token"))
    {
        JsonSerializerOptions = new JsonSerializerOptions(),
    };

    _ = new VaultClient(settings);
    var options = settings.JsonSerializerOptions!;

    AotApiScenario.Require(
        !options.TypeInfoResolverChain.Any(resolver => resolver is DefaultJsonTypeInfoResolver),
        "Response coverage must run without reflection fallback.");

    var responseRoots = typeof(VaultClient).Assembly
        .GetExportedTypes()
        .Where(type => type.IsInterface &&
                       type.Namespace?.StartsWith("VaultSharp.V1", StringComparison.Ordinal) == true)
        .SelectMany(type => type.GetMethods())
        .Select(method => UnwrapTask(method.ReturnType))
        .Where(type => type is not null && !type.ContainsGenericParameters)
        .Distinct()
        .OrderBy(type => type!.FullName, StringComparer.Ordinal)
        .ToList();

    var missingRoots = responseRoots
        .Where(type =>
        {
            try
            {
                _ = options.GetTypeInfo(type!);
                return false;
            }
            catch (NotSupportedException)
            {
                return true;
            }
        })
        .Select(type => type!.FullName)
        .ToList();

    Console.WriteLine($"Closed public API response roots checked: {responseRoots.Count}");
    Console.WriteLine($"Missing roots: {missingRoots.Count}");

    AotApiScenario.Require(
        responseRoots.Count == ExpectedClosedPublicResponseRootCount,
        $"Expected {ExpectedClosedPublicResponseRootCount} closed public response roots, found {responseRoots.Count}.");
    AotApiScenario.Require(
        missingRoots.Count == 0,
        "Missing response roots: " + string.Join(", ", missingRoots));
}

static Type? UnwrapTask(Type returnType)
{
    return returnType.IsGenericType &&
           returnType.GetGenericTypeDefinition() == typeof(Task<>)
        ? returnType.GetGenericArguments()[0]
        : null;
}
