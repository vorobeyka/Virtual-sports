using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace VirtualSports.Web.Filters
{
    public class ValidatePlatformHeaderFilter : ActionFilterAttribute
    {
        private static readonly string _headerName = "X-Platform";

        private static IEnumerable<string> Platforms()
        {
            yield return "web-desktop";
            yield return "web-mobile";
            yield return "ios";
            yield return "android";
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var platform = request.Headers[_headerName].FirstOrDefault().ToLower();

            foreach (var i in Platforms())
            {
                if (i == platform)
                {
                    return;
                }
            }

            context.Result = new BadRequestObjectResult("Unsupported platform!");
        }
    }
}
