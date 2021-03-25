using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtualSports.Web.Models;

namespace VirtualSports.Web.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment,
            ILogger logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment()) return;

            var errorCode = context.Exception.GetType().Name switch
            {
                "ArgumentException" => 602,
                "AuthenticationException" => 603,
                "HttpRequestException" => 604,
                "DbUpdateException" => 605,
                _ => 606
            };
            var message = errorCode switch
            {
                602 => "Wrong arguments",
                603 => "Error occurred during authentication",
                604 => "Error with Http Request",
                605 => "Such id was used already",
                606 => "Something went wrong"
            };

            _logger.LogInformation($"Error {errorCode} occurred.\nMessage: {message}.");

            context.Result = new JsonResult(new Error(errorCode, message));
            context.ExceptionHandled = true;
        }
    }
}