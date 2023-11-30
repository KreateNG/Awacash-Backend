using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class ResourceNotFoundException : CustomException
    {
        public ResourceNotFoundException(string message)
            : base(message, "404", HttpStatusCode.NotFound)
        {
        }
    }
}

