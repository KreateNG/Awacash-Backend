using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class OperationFailureException : CustomException
    {
        public OperationFailureException(string message)
            : base(message, "500", HttpStatusCode.InternalServerError)
        {
        }
    }
}

