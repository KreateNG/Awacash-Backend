using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class FileUploadException : CustomException
    {
        public FileUploadException(string message)
            : base(message, "423", HttpStatusCode.FailedDependency)
        {
        }
    }
}

