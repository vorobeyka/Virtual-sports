using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace VirtualSports.Web.Filters
{
    public class ValidateAdminHeaderFilter : ActionFilterAttribute
    {
        private static readonly string _headerName = "X-Auth";
        private static readonly string _headerValue = "admin";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var admin = request.Headers[_headerName].FirstOrDefault();

            if (admin != _headerValue)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
