using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VaultSharp.DataAccess
{
    internal class HttpDataAccessManager : IDataAccessManager
    {
        private readonly HttpClient _httpClient;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public HttpDataAccessManager(Uri baseAddress, HttpMessageHandler httpMessageHandler = null, bool continueAsyncTasksOnCapturedContext = false, TimeSpan? serviceTimeout = null)
        {
            _httpClient = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler);
            _httpClient.BaseAddress = baseAddress;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;

            if (serviceTimeout != null)
            {
                _httpClient.Timeout = serviceTimeout.Value;
            }
        }

        public async Task<TResponse> MakeRequestAsync<TResponse>(string resourcePath, HttpMethod httpMethod, object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false, Action<HttpStatusCode, string> failureDelegate = null) where TResponse : class
        {
            try
            {
                var requestContent = requestData != null
                    ? new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8)
                    : null;

                Func<Task<HttpResponseMessage>> apiFunc = null;

                switch (httpMethod.ToString().ToUpperInvariant())
                {
                    case "GET":

                        apiFunc = () => _httpClient.GetAsync(resourcePath);
                        break;

                    case "DELETE":

                        apiFunc = () => _httpClient.DeleteAsync(resourcePath);
                        break;

                    case "POST":

                        apiFunc = () => _httpClient.PostAsync(resourcePath, requestContent);
                        break;

                    case "PUT":

                        apiFunc = () => _httpClient.PutAsync(resourcePath, requestContent);
                        break;

                    default:
                        throw new NotSupportedException("The Http Method is not supported: " + httpMethod);
                }

                if (headers != null)
                {
                    foreach (var kv in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(kv.Key);
                        _httpClient.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                    }
                }

                var httpResponseMessage = await apiFunc().ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
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

                if (failureDelegate != null)
                {
                    failureDelegate(httpResponseMessage.StatusCode, responseText);
                }

                throw new Exception(string.Format(CultureInfo.InvariantCulture,
                    "{0} {1}. {2}",
                    (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode, responseText));
            }
            catch(Exception ex){
                throw;
            }
            // catch (WebException ex)
            // {
            //     if (ex.Status == WebExceptionStatus.ProtocolError)
            //     {
            //         var response = ex.Response as HttpWebResponse;

            //         if (response != null)
            //         {
            //             var responseText = string.Empty;

            //             using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            //             {
            //                 responseText =
            //                     await stream.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            //             }

            //             throw new Exception(string.Format(CultureInfo.InvariantCulture,
            //                 "{0} {1}. {2}",
            //                 (int)response.StatusCode, response.StatusCode, responseText), ex);
            //         }

            //         throw;
            //     }

            //     throw;
            // }
        }
    }
}