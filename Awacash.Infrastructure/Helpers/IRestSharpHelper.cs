using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Helpers
{
    public interface IRestSharpHelper
    {
        public Task<RestResponse> MakeRequest(object request, string baseAddress, string requestUri, Method method, Dictionary<string, string>? headers = null);
    }
}
