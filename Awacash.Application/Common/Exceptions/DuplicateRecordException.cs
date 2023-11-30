using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class DuplicateRecordException : CustomException
    {
        public DuplicateRecordException(string message)
            : base(message, "400", HttpStatusCode.BadRequest)
        {
        }
    }
}

