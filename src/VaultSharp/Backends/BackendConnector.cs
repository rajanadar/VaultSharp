using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VaultSharp.Backends
{
    internal class BackendConnector
    {
        private const string VaultTokenHeaderKey = "X-Vault-Token";
        private const string VaultWrapTimeToLiveHeaderKey = "X-Vault-Wrap-TTL";

        private readonly HttpClient _httpClient;
        private readonly Lazy<Task<string>> _lazyVaultToken;

        protected readonly bool _continueAsyncTasksOnCapturedContext;

        protected async Task<TResponse> MakeVaultApiRequest<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, bool rawResponse = false, Func<int, string, TResponse> customProcessor = null, string wrapTimeToLive = null) where TResponse : class
        {
            var headers = new Dictionary<string, string>();

            if (_lazyVaultToken != null)
            {
                headers.Add(VaultTokenHeaderKey, await _lazyVaultToken.Value);
            }

            if (wrapTimeToLive != null)
            {
                headers.Add(VaultWrapTimeToLiveHeaderKey, wrapTimeToLive);
            }

            return await MakeRequestAsync(resourcePath, httpMethod, requestData, headers, rawResponse, customProcessor);
        }

        protected async Task<TResponse> MakeRequestAsync<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false, Func<int, string, TResponse> customProcessor = null) where TResponse : class
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

                var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
                var responseText =
                    await
                        httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrWhiteSpace(responseText))
                    {
                        var response = rawResponse ? (responseText as TResponse) : JsonConvert.DeserializeObject<TResponse>(responseText);
                        return response;
                    }

                    return default(TResponse);
                }

                if (customProcessor != null)
                {
                    return customProcessor((int)httpResponseMessage.StatusCode, responseText);
                }

                throw new Exception(string.Format(CultureInfo.InvariantCulture,
                    "{0} {1}. {2}",
                    (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode, responseText));
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;

                    if (response != null)
                    {
                        string responseText;

                        using (var stream = new StreamReader(response.GetResponseStream()))
                        {
                            responseText =
                                await stream.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
                        }

                        if (customProcessor != null)
                        {
                            return customProcessor((int)response.StatusCode, responseText);
                        }

                        throw new Exception(string.Format(CultureInfo.InvariantCulture,
                            "{0} {1}. {2}",
                            (int)response.StatusCode, response.StatusCode, responseText), ex);
                    }

                    throw;
                }

                throw;
            }
        }
    }
}
