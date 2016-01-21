using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VaultSharp.DataAccess
{
    internal interface IDataAccessManager
    {
        Task<TResponse> MakeRequestAsync<TResponse>(string resourcePath, HttpMethod httpMethod,
            object requestData = null, IDictionary<string, string> headers = null, bool rawResponse = false, Action<HttpStatusCode, string> failureDelegate = null) where TResponse : class;
    }
}