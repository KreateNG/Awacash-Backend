using System;
using System.Net;

namespace AwaCash.Application.Common.Exceptions
{
    public class FileTypeNotSupportedException : CustomException
    {
        public FileTypeNotSupportedException()
            : base("File type not supported for operation", "415", HttpStatusCode.UnsupportedMediaType)
        {
        }
    }
}

