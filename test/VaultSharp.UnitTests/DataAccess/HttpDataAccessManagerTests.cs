using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using VaultSharp.DataAccess;
using Xunit;

namespace VaultSharp.UnitTests.DataAccess
{
    public class HttpDataAccessManagerTests
    {
        private static readonly Uri DummyUri = new Uri("http://127.0.0.1/v1/");

        [Fact]
        public void CanInstantiate()
        {
            var httpDataAccessManager = new HttpDataAccessManager(DummyUri);
            Assert.NotNull(httpDataAccessManager);

            httpDataAccessManager = new HttpDataAccessManager(DummyUri, new HttpClientHandler());
            Assert.NotNull(httpDataAccessManager);

            httpDataAccessManager = new HttpDataAccessManager(DummyUri, new HttpClientHandler(), true);
            Assert.NotNull(httpDataAccessManager);

            httpDataAccessManager = new HttpDataAccessManager(DummyUri, new HttpClientHandler(), true,
                TimeSpan.FromMinutes(1));
            Assert.NotNull(httpDataAccessManager);

            httpDataAccessManager = new HttpDataAccessManager(DummyUri, new HttpClientHandler(), true,
                TimeSpan.FromMinutes(1), client => { });
            Assert.NotNull(httpDataAccessManager);
        }

        private static readonly IDictionary<string, string> Headers = new Dictionary<string, string>
        {
            {"X-Vault-Token", "dummy"}
        };

        [Fact]
        public async Task MakeRequestAsyncTests()
        {
            var resourcePath = "sys/unseal";

            await MakeRequestAsync(resourcePath, HttpMethod.Get);
            await MakeRequestAsync(resourcePath, HttpMethod.Get, Headers);

            await MakeRequestAsync(resourcePath, HttpMethod.Put);
            await MakeRequestAsync(resourcePath, HttpMethod.Put, Headers);

            await MakeRequestAsync(resourcePath, HttpMethod.Post);
            await MakeRequestAsync(resourcePath, HttpMethod.Post, Headers);

            await MakeRequestAsync(resourcePath, HttpMethod.Delete);
            await MakeRequestAsync(resourcePath, HttpMethod.Delete, Headers);
        }

        private async Task MakeRequestAsync(string resourcePath, HttpMethod httpMethod, object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false)
        {
            var handler = new TestHttpMessageHandler();
            var manager = new HttpDataAccessManager(DummyUri, handler);

            await manager.MakeRequestAsync<dynamic>(resourcePath, httpMethod, requestData, headers, rawResponse);

            Assert.Equal(handler.RequestMessage.Method, httpMethod);
            Assert.Equal(handler.RequestMessage.Headers.Count(), headers != null ? headers.Count : 0);
            Assert.Equal(handler.RequestMessage.RequestUri.AbsoluteUri, DummyUri.AbsoluteUri.TrimEnd('/') + "/" + resourcePath);
        }

        private class TestHttpMessageHandler : HttpMessageHandler
        {
            public HttpRequestMessage RequestMessage { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestMessage = request;

                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) };
                return Task.FromResult(response);
            }
        }
    }
}