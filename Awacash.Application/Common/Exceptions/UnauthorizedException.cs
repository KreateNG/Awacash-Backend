using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message) : base(message, "401", HttpStatusCode.Unauthorized)
        {
        }
    }
}

