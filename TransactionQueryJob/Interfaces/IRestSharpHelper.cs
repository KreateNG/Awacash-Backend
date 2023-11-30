using System;
using RestSharp;

namespace TransactionQueryJob.Interfaces
{
    public interface IRestSharpHelper
    {
        public Task<RestResponse> MakeRequest(object request, string baseAddress, string requestUri, Method method, Dictionary<string, string>? headers = null);
    }
}

