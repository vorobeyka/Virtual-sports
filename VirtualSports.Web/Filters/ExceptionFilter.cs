using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace VirtualSports.Web.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment()) return;

            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionMessage = context.Exception.Message;
            var content = new ContentResult
            {
                Content = $"In {actionName} exception occurred: {exceptionMessage}"
            };
            content.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = content;
            context.ExceptionHandled = true;
        }
    }
}