using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AwaCash.Application.Common.Exceptions
{
    public class CustomException : Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public CustomException(string errorMessage, string errorCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public CustomException(string errorMessage, string errorCode, HttpStatusCode httpStatus) : this(errorMessage, errorCode)
        {
            StatusCode = httpStatus;
        }
    }
}
