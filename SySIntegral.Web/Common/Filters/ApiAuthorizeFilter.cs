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
        private const string DeviceIdKey = "X-SySIntegral-DeviceId";

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Allow Anonymous skips all authorization
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var deviceIdValue =
                context.HttpContext.Request.Headers.FirstOrDefault(x => !string.IsNullOrEmpty(x.Key) && x.Key.Equals(DeviceIdKey)).Value.ToString();

            if (string.IsNullOrEmpty(deviceIdValue))
            {
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new BadRequestObjectResult("Unauthorized");
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
