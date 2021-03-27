using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using VirtualSports.Lib.Models;

namespace VirtualSports.Web.Filters
{
    public class ValidatePlatformHeaderFilter : ActionFilterAttribute
    {
        private static readonly string _headerName = "X-Platform";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var platform = request.Headers[_headerName].FirstOrDefault().ToLower();

            if (AppTools.Platforms.Any(pl => pl == platform))
            {
                return;
            }

            context.Result = new BadRequestObjectResult("Unsupported platform!");
        }
    }
}
