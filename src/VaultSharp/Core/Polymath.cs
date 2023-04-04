﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.Kerberos;
using VaultSharp.V1.Commons;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace VaultSharp.Core
{
    internal class Polymath
    {
        private const string VaultRequestHeaderKey = "X-Vault-Request";
        private const string NamespaceHeaderKey = "X-Vault-Namespace";
        private const string AuthorizationHeaderKey = "Authorization";
        private const string VaultTokenHeaderKey = "X-Vault-Token";
        private const string VaultWrapTimeToLiveHeaderKey = "X-Vault-Wrap-TTL";

        private const string VaultSharpV1Path = "v1/";

        private readonly HttpClient _httpClient;
        private Lazy<Task<string>> _lazyVaultToken;
        private readonly IAuthMethodLoginProvider _authMethodLoginProvider;

        public HttpMethod ListHttpMethod { get; } = new HttpMethod("LIST");

        public VaultClientSettings VaultClientSettings { get; }

        public Polymath(VaultClientSettings vaultClientSettings)
        {
            VaultClientSettings = vaultClientSettings;

#if NET45
            var handler = new WebRequestHandler();

            // if auth method is kerberos, then set the credentials in the handler.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Kerberos)
            {
                var kerberosAuthMethodInfo = vaultClientSettings.AuthMethodInfo as KerberosAuthMethodInfo;
                handler.PreAuthenticate = kerberosAuthMethodInfo.PreAuthenticate;
                handler.Credentials = kerberosAuthMethodInfo.Credentials;
            }

#elif NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

            var handler = new WinHttpHandler();

            // if auth method is kerberos, then set the credentials in the handler.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Kerberos)
            {
                var kerberosAuthMethodInfo = vaultClientSettings.AuthMethodInfo as KerberosAuthMethodInfo;
                handler.PreAuthenticate = kerberosAuthMethodInfo.PreAuthenticate;
                handler.ServerCredentials = kerberosAuthMethodInfo.Credentials;
            }

#else
            var handler = new HttpClientHandler();

            // if auth method is kerberos, then set the credentials in the handler.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Kerberos)
            {
                var kerberosAuthMethodInfo = vaultClientSettings.AuthMethodInfo as KerberosAuthMethodInfo;
                handler.PreAuthenticate = kerberosAuthMethodInfo.PreAuthenticate;
                handler.Credentials = kerberosAuthMethodInfo.Credentials;
            }

#endif

            // not the best place, but a very convenient place to add cert of certauthmethod.
            if (vaultClientSettings.AuthMethodInfo?.AuthMethodType == AuthMethodType.Cert)
            {
                var certAuthMethodInfo = vaultClientSettings.AuthMethodInfo as CertAuthMethodInfo;

                if (certAuthMethodInfo.ClientCertificate != null)
                {
                    handler.ClientCertificates.Add(certAuthMethodInfo.ClientCertificate);
                }
                else
                {
                    if (certAuthMethodInfo.ClientCertificateCollection != null)
                    {
                        handler.ClientCertificates.AddRange(certAuthMethodInfo.ClientCertificateCollection);
                    }
                }
            }

            vaultClientSettings.PostProcessHttpClientHandlerAction?.Invoke(handler);

            _httpClient = VaultClientSettings.MyHttpClientProviderFunc == null ? new HttpClient(handler) : VaultClientSettings.MyHttpClientProviderFunc(handler);

            _httpClient.BaseAddress = new Uri(VaultClientSettings.VaultServerUriWithPort);

            if (VaultClientSettings.VaultServiceTimeout != null)
            {
                _httpClient.Timeout = VaultClientSettings.VaultServiceTimeout.Value;
            }

            if (VaultClientSettings.AuthMethodInfo != null)
            {
                _authMethodLoginProvider = AuthProviderFactory.CreateAuthenticationProvider(VaultClientSettings.AuthMethodInfo, this);

                SetVaultTokenDelegate();
            }
        }

        internal void SetVaultTokenDelegate()
        {
            if (_authMethodLoginProvider != null)
            {
                _lazyVaultToken = new Lazy<Task<string>>(_authMethodLoginProvider.GetVaultTokenAsync, LazyThreadSafetyMode.PublicationOnly);
            }
        }

        internal async Task PerformImmediateLogin()
        {
            if (_authMethodLoginProvider != null)
            {
                // make a dummy call, that will force a login.
                await _lazyVaultToken.Value.ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            }
        }

        public async Task MakeVaultApiRequest(string mountPoint, string path, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, bool unauthenticated = false)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            
            await MakeVaultApiRequest(VaultSharpV1Path + mountPoint.Trim('/') + path, httpMethod, requestData, rawResponse, unauthenticated: unauthenticated).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task MakeVaultApiRequest(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, bool unauthenticated = false)
        {
            await MakeVaultApiRequest<JsonObject>(resourcePath, httpMethod, requestData, rawResponse, unauthenticated: unauthenticated).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<TResponse> MakeVaultApiRequest<TResponse>(string mountPoint, string path, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, Action<HttpResponseMessage> postResponseAction = null, string wrapTimeToLive = null, bool unauthenticated = false) where TResponse : class 
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await MakeVaultApiRequest<TResponse>(VaultSharpV1Path + mountPoint.Trim('/') + path, httpMethod, requestData, rawResponse, postResponseAction, wrapTimeToLive, unauthenticated).ConfigureAwait(VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
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

        protected async Task<TResponse> MakeRequestAsync<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false, Action<HttpResponseMessage> postResponseAction = null) where TResponse : class
        {
            try
            {
                var requestUri = new Uri(_httpClient.BaseAddress, new Uri(resourcePath, UriKind.Relative));

                var requestContent = requestData != null
                    ? new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8)
                    : null;

                HttpRequestMessage httpRequestMessage = null;

                switch (httpMethod.ToString().ToUpperInvariant())
                {
                    case "LIST":
                    case "GET":
                    case "DELETE":
                    case "HEAD":

                        httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri);
                        break;

                    case "POST":
                    case "PUT":

                        httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri)
                        {
                            Content = requestContent
                        };

                        break;

                    case "PATCH":

                        // We cannot directly add content type for httpRequestMessage
                        // It is added via the RequestContent
                        // https://stackoverflow.com/a/70593566/1174414

                        httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri)
                        {
                            Content = requestData != null 
                            ? new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/merge-patch+json")
                            : null
                        };

                        break;

                    default:
                        throw new NotSupportedException("The Http Method is not supported: " + httpMethod);
                }

                httpRequestMessage.Headers.Add(VaultRequestHeaderKey, "true");

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
                        var response = rawResponse ? (responseText as TResponse) : JsonSerializer.Deserialize<TResponse>(responseText);
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
