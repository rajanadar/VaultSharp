using System;
using System.Net.Http;
using VaultSharp.V1.AuthMethods;

namespace VaultSharp
{
    public class VaultClientSettings
    {
        public string VaultServerUriWithPort { get; set; }

        public IAuthInfo AuthInfo { get; set; }

        public bool ContinueAsyncTasksOnCapturedContext { get; set; }

        public TimeSpan? VaultServiceTimeout { get; set; }

        public Action<HttpClientHandler> PostProcessHttpClientHandlerAction { get; set; }

        public Action<HttpClient, HttpRequestMessage> BeforeApiRequestAction { get; set; }

        public Action<HttpResponseMessage> AfterApiResponseAction { get; set; }

        public Action<HttpResponseMessage> OnErrorApiResponseAction { get; set; }
    }
}
