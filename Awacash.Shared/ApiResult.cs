using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Shared
{
    public class ApiResult<T>
    {
        public bool HasError { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public int TotalCount { get; set; }

        public static ApiResult<MessageOut> GetResponse(bool hasError, string message, bool isSuccessful)
        {
            ApiResult<MessageOut> response = new ApiResult<MessageOut>
            {
                HasError = hasError,
                Message = message,
                Result = new MessageOut
                {
                    IsSuccessful = isSuccessful,
                    Message = message
                }
            };
            return response;
        }
    }

    public class MessageOut
    {
        public string? Message { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string>? Errors { get; set; }
    }
}
