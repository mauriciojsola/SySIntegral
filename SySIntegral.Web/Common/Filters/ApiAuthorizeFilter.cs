using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SySIntegral.Web.Common.Filters
{
    public class ApiAuthorizeFilter : IActionFilter
    {
        private const string ClientIdKey = "X-SySIntegral-ClientId";

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // AllowAnonymous skips any authorization
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var clientIdValue =
                context.HttpContext.Request.Headers.FirstOrDefault(x => !string.IsNullOrEmpty(x.Key) && x.Key.Equals(ClientIdKey)).Value.ToString();

            if (string.IsNullOrEmpty(clientIdValue))
            {
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new BadRequestObjectResult("Unauthorized");
                //context.Result = new UnauthorizedResult();
            }
        }
    }
}
