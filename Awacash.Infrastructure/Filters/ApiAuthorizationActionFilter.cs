using System;
using Awacash.Domain.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Awacash.Infrastructure.Filters
{
    public class ApiAuthorizationActionFilter : IActionFilter
    {
        protected AppSettings _appSettings { get; set; }

        public ApiAuthorizationActionFilter(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var appId = GetHeaderValueAs<string>(context, "AppId");
            var appKey = GetHeaderValueAs<string>(context, "AppKey");

            if (appId != _appSettings.ApiId || appKey != _appSettings.ApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid request!");
            }
        }

        private T GetHeaderValueAs<T>(ActionExecutingContext context, string headerName)
        {
            StringValues values;

            if (context?.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}

