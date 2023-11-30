using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string message)
            : base(message, "403", HttpStatusCode.Forbidden)
        {
        }
    }
}

