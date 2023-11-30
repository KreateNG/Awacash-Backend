using System;
using AwaCash.Application.Common.Exceptions;
using System.Net;

namespace Awacash.Application.Common.Exceptions
{
    public class ErrorSavingRecordException : CustomException
    {
        public ErrorSavingRecordException() : base("Error occurred while saving record", null, HttpStatusCode.BadRequest)
        {
        }
    }
}

