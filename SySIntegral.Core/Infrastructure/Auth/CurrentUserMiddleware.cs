using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public class CurrentUserMiddleware : IMiddleware
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
