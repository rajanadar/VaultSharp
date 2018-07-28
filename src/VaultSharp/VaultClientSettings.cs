using System;
using System.Net.Http;
using VaultSharp.V1.AuthMethods;

namespace VaultSharp
{
    public class VaultClientSettings
    {
        // raja todo: make all params be from constructor.
        public VaultClientSettings(string vaultServerUriWithPort, IAuthMethodInfo authMethodInfo)
        {
            VaultServerUriWithPort = vaultServerUriWithPort;
            AuthMethodInfo = authMethodInfo;
        }

        public string VaultServerUriWithPort { get; }

        public IAuthMethodInfo AuthMethodInfo { get; }

        public bool ContinueAsyncTasksOnCapturedContext { get; set; }

        public TimeSpan? VaultServiceTimeout { get; set; }

        public Action<HttpClientHandler> PostProcessHttpClientHandlerAction { get; set; }

        public Action<HttpClient, HttpRequestMessage> BeforeApiRequestAction { get; set; }

        public Action<HttpResponseMessage> AfterApiResponseAction { get; set; }

        public Action<HttpResponseMessage> OnErrorApiResponseAction { get; set; }
    }
}
