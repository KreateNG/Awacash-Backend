using Awacash.Application.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Middlewares
{
    public class CurrentUserMiddleware: IMiddleware
    {
        private readonly ICurrentUserInitializer _currentUserInitializer;

        public CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) =>
            _currentUserInitializer = currentUserInitializer;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _currentUserInitializer.SetCurrentUser(context.User);

            await next(context);
        }
    }
}
