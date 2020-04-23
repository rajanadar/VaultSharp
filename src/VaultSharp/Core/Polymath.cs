using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.Commons;

namespace VaultSharp.Core
{
    internal class Polymath
    {
        private const string NamespaceHeaderKey = "X-Vault-Namespace";
        private const string AuthorizationHeaderKey = "Authorization";
        private const string VaultTokenHeaderKey = "X-Vault-Token";
        private const string VaultWrapTimeToLiveHeaderKey = "X-Vault-Wrap-TTL";

        private readonly HttpClient _httpClient;
        private readonly Lazy<Task<string>> _lazyVaultToken;

        public VaultClientSettings VaultClientSettings { get; }

        public Polymath(VaultClientSettings vaultClientSettings, ICredentials credentials = null)
        {
            VaultClientSettings = vaultClientSettings;

#if NET45
            var handler = new WebRequestHandler();

            // not the best place, but a very convenient place to add cert of certauthmethod.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Cert)
            {
                var certAuthMethodInfo = vaultClientSettings.AuthMethodInfo as CertAuthMethodInfo;
                handler.ClientCertificates.Add(certAuthMethodInfo.ClientCertificate);
            }
#else
            var handler = new HttpClientHandler();

            // not the best place, but a very convenient place to add cert of certauthmethod.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Cert)
            {
                var certAuthMethodInfo = vaultClientSettings.AuthMethodInfo as CertAuthMethodInfo;
                handler.ClientCertificates.Add(certAuthMethodInfo.ClientCertificate);
            }
#endif

            handler.Credentials = credentials;

            vaultClientSettings.PostProcessHttpClientHandlerAction?.Invoke(handler);

            _httpClient = new HttpClient(handler);
            ConfigureHttpClient();
            _lazyVaultToken = GetLazyVaultToken();
        }

        public Polymath(VaultClientSettings vaultClientSettings, HttpClient httpClient)
        {
            VaultClientSettings = vaultClientSettings;
            _httpClient = httpClient;

            ConfigureHttpClient();
            _lazyVaultToken = GetLazyVaultToken();
        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress = new Uri(VaultClientSettings.VaultServerUriWithPort);

            if (VaultClientSettings.VaultServiceTimeout != null)
            {
                _httpClient.Timeout = VaultClientSettings.VaultServiceTimeout.Value;
            }
        }

        private Lazy<Task<string>> GetLazyVaultToken()
        {
            if (VaultClientSettings.AuthMethodInfo == null)
            {
                return null;
            }

            var authProvider = AuthProviderFactory.CreateAuthenticationProvider(VaultClientSettings.AuthMethodInfo, this);
            return new Lazy<Task<string>>(authProvider.GetVaultTokenAsync);
        }

        public async Task MakeVaultApiRequest(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, bool unauthenticated = false)
        {
            await MakeVaultApiRequest<JToken>(resourcePath, httpMethod, requestData, rawResponse, unauthenticated: unauthenticated).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<TResponse> MakeVaultApiRequest<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, Action<HttpResponseMessage> postResponseAction = null, string wrapTimeToLive = null, bool unauthenticated = false) where TResponse : class
        {
            var headers = new Dictionary<string, string>();

            if (!unauthenticated && _lazyVaultToken != null)
            {
                var vaultToken = await _lazyVaultToken.Value.ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                if (VaultClientSettings.UseVaultTokenHeaderInsteadOfAuthorizationHeader)
                {
                    headers.Add(VaultTokenHeaderKey, vaultToken);
                }
                else
                {
                    headers.Add(AuthorizationHeaderKey, "Bearer " + vaultToken);
                }
            }

            if (wrapTimeToLive != null)
            {
                headers.Add(VaultWrapTimeToLiveHeaderKey, wrapTimeToLive);
            }

            if (!string.IsNullOrWhiteSpace(VaultClientSettings.Namespace))
            {
                headers.Add(NamespaceHeaderKey, VaultClientSettings.Namespace);
            }

            return await MakeRequestAsync<TResponse>(resourcePath, httpMethod, requestData, headers, rawResponse, postResponseAction).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public Secret<T2> GetMappedSecret<T1, T2>(Secret<T1> sourceSecret, T2 destinationData)
        {
            return new Secret<T2>
            {
                Data = destinationData,
                LeaseDurationSeconds = sourceSecret.LeaseDurationSeconds,
                RequestId = sourceSecret.RequestId,
                Warnings = sourceSecret.Warnings,
                LeaseId = sourceSecret.LeaseId,
                Renewable = sourceSecret.Renewable,
                AuthInfo = sourceSecret.AuthInfo,
                WrapInfo = sourceSecret.WrapInfo
            };
        }

        /// //////

        protected async Task<TResponse> MakeRequestAsync<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false, Action<HttpResponseMessage> postResponseAction = null) where TResponse : class
        {
            try
            {
                var requestUri = new Uri(_httpClient.BaseAddress, resourcePath);

                var requestContent = requestData != null
                    ? new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8)
                    : null;

                HttpRequestMessage httpRequestMessage = null;

                switch (httpMethod.ToString().ToUpperInvariant())
                {
                    case "GET":

                        httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                        break;

                    case "DELETE":

                        httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
                        break;

                    case "POST":

                        httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
                        {
                            Content = requestContent
                        };

                        break;

                    case "PUT":

                        httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
                        {
                            Content = requestContent
                        };

                        break;

                    case "HEAD":

                        httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, requestUri);
                        break;

                    default:
                        throw new NotSupportedException("The Http Method is not supported: " + httpMethod);
                }

                if (headers != null)
                {
                    foreach (var kv in headers)
                    {
                        httpRequestMessage.Headers.Remove(kv.Key);
                        httpRequestMessage.Headers.Add(kv.Key, kv.Value);
                    }
                }

                VaultClientSettings.BeforeApiRequestAction?.Invoke(_httpClient, httpRequestMessage);

                var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                // internal delegate.
                postResponseAction?.Invoke(httpResponseMessage);

                VaultClientSettings.AfterApiResponseAction?.Invoke(httpResponseMessage);

                var responseText =
                    await
                        httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrWhiteSpace(responseText))
                    {
                        var response = rawResponse ? (responseText as TResponse) : JsonConvert.DeserializeObject<TResponse>(responseText);
                        return response;
                    }

                    return default(TResponse);
                }

                throw new VaultApiException(httpResponseMessage.StatusCode, responseText);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                    {
                        string responseText;

                        using (var stream = new StreamReader(response.GetResponseStream()))
                        {
                            responseText =
                                await stream.ReadToEndAsync().ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
                        }

                        throw new VaultApiException(response.StatusCode, responseText);
                    }

                    throw;
                }

                throw;
            }
        }
    }
}
