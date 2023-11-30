using Awacash.Application.Common.Interfaces.Authentication;
using AwaCash.Application.Common.Exceptions;
using Awacash.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ICurrentUser _currentUser;

        public ExceptionMiddleware(
            ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                string errorId = Guid.NewGuid().ToString();
                LogContext.PushProperty("ErrorId", errorId);
                LogContext.PushProperty("StackTrace", exception.StackTrace);
                var errorResult = new ErrorResult
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                    Exception = exception.Message.Trim(),
                    ErrorId = errorId,
                    SupportMessage = "Unsupported message"
                };
                errorResult.Messages!.Add(exception.Message);
                var response = context.Response;
                response.ContentType = "application/json";
                if (exception is not CustomException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }


                switch (exception)
                {
                    case CustomException e:
                        response.StatusCode = errorResult.StatusCode = (int)e.StatusCode;
                        if (e.ErrorMessage is not null)
                        {
                            errorResult.Messages = new List<string> { e.ErrorMessage };
                            await response.WriteAsync(JsonConvert.SerializeObject(ResponseModel<string>.Failure(e.ErrorMessage, e.ErrorCode)));
                            return;
                        }

                        break;

                    case KeyNotFoundException:
                        response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case UnauthorizedAccessException:
                        response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    //case ForbiddenException:
                    //    response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.Forbidden;
                    //    break;

                    default:
                        response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
                await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
            }
        }
    }
}
