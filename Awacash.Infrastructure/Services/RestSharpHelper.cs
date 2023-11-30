using Awacash.Infrastructure.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Services
{
    internal class RestSharpHelper : IRestSharpHelper
    {
        public async Task<RestResponse> MakeRequest(object requestObject, string baseAddress, string requestUri, Method method, Dictionary<string, string>? headers = null)
        {
            try
            {
                var client = new RestClient(baseAddress);
                var request = new RestRequest(requestUri, method);
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                string data = string.Empty;
                switch (method)
                {
                    case Method.Get:
                        return await client.ExecuteAsync(request);
                    //break;
                    case Method.Post:
                        data = JsonConvert.SerializeObject(requestObject);
                        request.AddHeader("content-type", "application/json");
                        request.AddParameter("application/json", data, ParameterType.RequestBody);
                        //HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        return await client.ExecuteAsync(request);

                    //if (isJson)
                    //{

                    //}
                    //else
                    //{
                    //    if (encoded)
                    //    {
                    //        string data = JsonConvert.SerializeObject(request);
                    //        HttpContent content = new FormUrlEncodedContent(request as Dictionary<string, string>);
                    //        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    //        content.Headers.ContentType.CharSet = "UTF-8";
                    //        return await _httpClient.PostAsync(requestUri, content);
                    //    }
                    //    else
                    //    {
                    //        string data = JsonConvert.SerializeObject(request);
                    //        HttpContent content = request as MultipartFormDataContent;
                    //        content.Headers.ContentType.MediaType = "multipart/form-data";
                    //        content.Headers.ContentType.CharSet = "UTF-8";
                    //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    //        return await _httpClient.PostAsync(requestUri, content);
                    //    }
                    //}
                    //break;
                    case Method.Put:
                        data = JsonConvert.SerializeObject(request);
                        //HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        return await client.PutAsync(request);
                    //break;
                    //case Method.Delete:
                    //    break;
                    //case Method.Head:
                    //    break;
                    //case Method.Options:
                    //    break;
                    //case Method.Patch:
                    //    break;
                    //case Method.Merge:
                    //    break;
                    //case Method.Copy:
                    //    break;
                    //case Method.Search:
                    //    break;
                    default:
                        return null;
                        //break;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
