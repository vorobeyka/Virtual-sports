using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using VirtualSports.Web.Contracts.ViewModels;

namespace VirtualSports.Web.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(
            ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {

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
                606 => context.Exception.Message
            };

            _logger.LogError($"Error {errorCode} occurred.\nMessage: {message}.");
            _logger.LogError(context.Exception.ToString());

            context.Result = new JsonResult(new Error(errorCode, message)) { StatusCode = 400 };
            context.ExceptionHandled = true;
        }
    }
}