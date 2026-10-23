using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.GoogleCloud;
using VaultSharp.V1.SecretsEngines.TOTP;
using VaultSharp.V1.SystemBackend.Enterprise;
using VaultSharp.V1.SystemBackend.Plugin;

namespace VaultSharp.Testing;

internal static class AotApiScenario
{
    internal static async Task RunAffectedApiPathsAsync()
    {
        var handler = new StubHttpMessageHandler(
            new ExpectedRequest(HttpMethod.Put, "/v1/sys/plugin/reload/backend",
                """{"plugin":"sample-plugin","mounts":["first","second"]}""", "{}"),
            new ExpectedRequest(HttpMethod.Put, "/v1/sys/plugins/catalog/sample-plugin",
                """{"sha_256":"abc123","command":"sample-command"}""", "{}"),
            new ExpectedRequest(HttpMethod.Put, "/v1/sys/license",
                """{"text":"license-text"}""", """{"data":{"license_id":"license-id"}}"""),
            new ExpectedRequest(HttpMethod.Put, "/v1/sys/policies/rgp/rgp-policy",
                """{"policy":"rgp-rules"}""", "{}"),
            new ExpectedRequest(HttpMethod.Put, "/v1/sys/policies/egp/egp-policy",
                """{"policy":"egp-rules"}""", "{}"),
            new ExpectedRequest(HttpMethod.Post, "/v1/kubernetes/creds/sample-role",
                """{"kubernetes_namespace":"sample-namespace","cluster_role_binding":true,"ttl":"45m"}""",
                """{"data":{"service_account_name":"service-account","service_account_namespace":"sample-namespace","service_account_token":"service-token"}}"""),
            new ExpectedRequest(HttpMethod.Post, "/v1/gcp/roleset/sample-roleset/key",
                """{"key_algorithm":"KEY_ALG_RSA_1024","key_type":"TYPE_GOOGLE_CREDENTIALS_FILE","ttl":"30m"}""",
                """{"data":{"private_key_data":"private-key"}}"""),
            new ExpectedRequest(HttpMethod.Post, "/v1/totp/keys/vault-key",
                """{"generate":true,"exported":true,"key_size":32,"issuer":"issuer","account_name":"vault-account","period":"60","algorithm":"SHA256","digits":8,"skew":1,"qr_size":256}""",
                """{"data":{"barcode":"barcode","url":"otpauth://vault"}}"""),
            new ExpectedRequest(HttpMethod.Post, "/v1/totp/keys/external-key",
                """{"url":"otpauth://external","key":"external-secret","issuer":"issuer","account_name":"external-account","period":"30","algorithm":"SHA1","digits":6}""",
                """{"data":{"url":"otpauth://external-response"}}"""));

        var client = CreateClient(handler, new JsonSerializerOptions());

        await client.V1.System.Plugins.ReloadBackendsAsync(
            "sample-plugin",
            new[] { "first", "second" });
        await client.V1.System.Plugins.RegisterAsync(new PluginConfig
        {
            Name = "sample-plugin",
            Sha256 = "abc123",
            Command = "sample-command",
        });
        await client.V1.System.Enterprise.InstallLicenseAsync("license-text");
        await client.V1.System.Enterprise.WriteRGPPolicyAsync(new RGPPolicy
        {
            Name = "rgp-policy",
            Policy = "rgp-rules",
        });
        await client.V1.System.Enterprise.WriteEGPPolicyAsync(new EGPPolicy
        {
            Name = "egp-policy",
            Policy = "egp-rules",
        });

        var kubernetes = await client.V1.Secrets.Kubernetes.GetCredentialsAsync(
            "sample-role",
            "sample-namespace",
            clusterRoleBinding: true,
            timeToLive: "45m");
        Require(kubernetes.Data.ServiceAccountName == "service-account", "Kubernetes response was not deserialized.");
        Require(kubernetes.Data.ServiceAccountToken == "service-token", "Kubernetes token was not deserialized.");

        var googleCloud = await client.V1.Secrets.GoogleCloud.GetServiceAccountKeyAsync(
            "sample-roleset",
            ServiceAccountKeyAlgorithm.KEY_ALG_RSA_1024,
            timeToLive: "30m");
        Require(googleCloud.Data.Base64EncodedPrivateKeyData == "private-key", "Google Cloud response was not deserialized.");

        var vaultGenerated = await client.V1.Secrets.TOTP.CreateKeyAsync(
            "vault-key",
            new TOTPCreateKeyRequest
            {
                KeyGenerationOption = new TOTPVaultBasedKeyGeneration
                {
                    Exported = true,
                    KeySize = 32,
                    Skew = 1,
                    QRSize = 256,
                },
                Issuer = "issuer",
                AccountName = "vault-account",
                Period = "60",
                Algorithm = "SHA256",
                Digits = 8,
            });
        Require(vaultGenerated.Data.Barcode == "barcode", "Vault-generated TOTP response was not deserialized.");

        var external = await client.V1.Secrets.TOTP.CreateKeyAsync(
            "external-key",
            new TOTPCreateKeyRequest
            {
                KeyGenerationOption = new TOTPNonVaultBasedKeyGeneration
                {
                    Url = "otpauth://external",
                    Key = "external-secret",
                },
                Issuer = "issuer",
                AccountName = "external-account",
            });
        Require(external.Data.Url == "otpauth://external-response", "External TOTP response was not deserialized.");

        handler.AssertComplete();
    }

    internal static async Task RunConsumerDtoPathsAsync()
    {
        var handler = new StubHttpMessageHandler(
            new ExpectedRequest(HttpMethod.Post, "/v1/kv-v2/data/sample",
                """{"data":{"value":"write-value"}}""",
                """{"data":{"version":7}}"""),
            new ExpectedRequest(HttpMethod.Get, "/v1/kv-v2/data/sample",
                null,
                """{"data":{"data":{"value":"read-value"},"metadata":{"version":8}}}"""));

        var options = new JsonSerializerOptions();
        options.TypeInfoResolverChain.Insert(0, ConsumerVaultJsonContext.Default);
        var client = CreateClient(handler, options);

        Require(
            !options.TypeInfoResolverChain.Any(resolver => resolver is DefaultJsonTypeInfoResolver),
            "Consumer DTO coverage must run without reflection fallback.");

        var written = await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(
            "sample",
            new ConsumerSecret { Value = "write-value" });
        Require(written.Data.Version == 7, "KV v2 write response was not deserialized.");

        var read = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<ConsumerSecret>("sample");
        Require(read.Data.Data.Value == "read-value", "KV v2 consumer DTO was not deserialized.");
        Require(read.Data.Metadata.Version == 8, "KV v2 metadata was not deserialized.");

        handler.AssertComplete();
    }

    internal static async Task RunCancellationPathAsync()
    {
        var client = CreateClient(new HangingHttpMessageHandler(), new JsonSerializerOptions());
        await RequireCancelledAsync(
            token => client.V1.Secrets.KeyValue.V2.ReadSecretAsync("sample", cancellationToken: token),
            "Cancellation token was not forwarded to the HTTP request.");

        var loginClient = CreateClient(new HangingHttpMessageHandler(), new JsonSerializerOptions(), new UserPassAuthMethodInfo("user", "password"));
        await RequireCancelledAsync(
            token => loginClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("sample", cancellationToken: token),
            "Cancellation token did not stop waiting for the login.");
        await RequireCancelledAsync(
            token => loginClient.V1.Auth.PerformImmediateLogin(token),
            "Cancellation token did not stop waiting for the immediate login.");

        var slowLogin = new SlowLoginHttpMessageHandler();
        var slowLoginClient = CreateClient(slowLogin, new JsonSerializerOptions(), new UserPassAuthMethodInfo("user", "password"));
        await RequireCancelledAsync(
            token => slowLoginClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("sample", cancellationToken: token),
            "Cancellation token did not stop waiting for the slow login.");
        slowLogin.CompleteLogin();
        await slowLoginClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("sample");
        Require(slowLogin.LoginRequestCount == 1, $"Expected the cancelled caller's login to be reused, found {slowLogin.LoginRequestCount} login requests.");
    }

    private static async Task RequireCancelledAsync(Func<CancellationToken, Task> action, string message)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        var request = action(cancellation.Token);
        var finished = await Task.WhenAny(request, Task.Delay(TimeSpan.FromSeconds(10)));
        Require(finished == request, message);

        try
        {
            await request;
            throw new InvalidOperationException("Cancelled request completed successfully.");
        }
        catch (OperationCanceledException)
        {
        }
    }

    internal static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static VaultClient CreateClient(HttpMessageHandler handler, JsonSerializerOptions options, IAuthMethodInfo? authMethodInfo = null)
    {
        var settings = new VaultClientSettings(
            "https://vault.invalid",
            authMethodInfo ?? new TokenAuthMethodInfo("token"))
        {
            JsonSerializerOptions = options,
            MyHttpClientProviderFunc = _ => new HttpClient(handler) { Timeout = Timeout.InfiniteTimeSpan },
        };

        var client = new VaultClient(settings);
        Require(ReferenceEquals(settings.JsonSerializerOptions, options), "VaultClient replaced the configured serializer options.");
        return client;
    }

    private sealed class ExpectedRequest
    {
        internal ExpectedRequest(HttpMethod method, string pathAndQuery, string? json, string responseJson)
        {
            Method = method;
            PathAndQuery = pathAndQuery;
            Json = json;
            ResponseJson = responseJson;
        }

        internal HttpMethod Method { get; }
        internal string PathAndQuery { get; }
        internal string? Json { get; }
        internal string ResponseJson { get; }
    }

    private sealed class HangingHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("Hanging handler must only finish by cancellation.");
        }
    }

    private sealed class SlowLoginHttpMessageHandler : HttpMessageHandler
    {
        private readonly TaskCompletionSource _loginGate = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _loginRequestCount;

        internal int LoginRequestCount => _loginRequestCount;

        internal void CompleteLogin() => _loginGate.SetResult();

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri!.AbsolutePath.Contains("/login/"))
            {
                Interlocked.Increment(ref _loginRequestCount);
                await _loginGate.Task;
                return JsonResponse("""{"auth":{"client_token":"login-token"}}""");
            }

            Require(request.Headers.Authorization?.Parameter == "login-token", "Request was sent without the login token.");
            return JsonResponse("""{"data":{"data":{},"metadata":{"version":1}}}""");
        }

        private static HttpResponseMessage JsonResponse(string json) =>
            new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<ExpectedRequest> _requests;

        internal StubHttpMessageHandler(params ExpectedRequest[] requests)
        {
            _requests = new Queue<ExpectedRequest>(requests);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Require(_requests.Count > 0, $"Unexpected request: {request.Method} {request.RequestUri}");
            var expected = _requests.Dequeue();
            var actualJson = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);

            Require(request.Method == expected.Method,
                $"Expected HTTP method {expected.Method}, found {request.Method}.");
            Require(request.RequestUri?.PathAndQuery == expected.PathAndQuery,
                $"Expected path {expected.PathAndQuery}, found {request.RequestUri?.PathAndQuery}.");
            Require(JsonEquals(actualJson, expected.Json),
                $"Expected JSON {expected.Json ?? "<none>"}, found {actualJson ?? "<none>"}.");

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expected.ResponseJson, Encoding.UTF8, "application/json"),
            };
        }

        internal void AssertComplete()
        {
            Require(_requests.Count == 0, $"{_requests.Count} expected request(s) were not sent.");
        }

        private static bool JsonEquals(string? actual, string? expected)
        {
            if (actual is null || expected is null)
            {
                return actual == expected;
            }

            return JsonNode.DeepEquals(JsonNode.Parse(actual), JsonNode.Parse(expected));
        }
    }
}

internal sealed class ConsumerSecret
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified)]
[JsonSerializable(typeof(ConsumerSecret))]
[JsonSerializable(typeof(Secret<SecretData<ConsumerSecret>>))]
internal partial class ConsumerVaultJsonContext : JsonSerializerContext
{
}
