using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VirtualSports.Web.Contracts;

namespace VirtualSports.Web.Middleware
{
    /// <summary>
    /// Custom middleware for logging request and response data
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private static readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager =
            new RecyclableMemoryStreamManager();
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Logging Request and Response bodies
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var login = string.IsNullOrEmpty(context.Request.Headers["Authorization"])
                ? "Unauthorized"
                : context.User.Identity.Name;

            var requestPath = context.Request.Path;
            var platform = "Not mentioned";
            if (context.Request.Headers.ContainsKey("X-Platform"))
            {
                context.Request.Headers.TryGetValue("X-Platform", out var temp);
                platform = temp.ToString();
            }

            var requestBody = "";
            var requestMethod = context.Request.Method;

            var controllerName = context
            .GetEndpoint()
            .Metadata
            .GetMetadata<ControllerActionDescriptor>().ControllerName;

            if (context.Request.Method.ToUpper() != "GET")
            {
                requestBody = await ObtainRequestBody(context.Request);
                if (string.IsNullOrEmpty(requestBody))
                {
                    requestBody = "Empty";
                }
                else if (controllerName.ToLower().Contains("auth"))
                {
                    login = (await JsonSerializer
                        .DeserializeAsync<Account>(context.Request.Body, _options))
                        .Login;
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    requestBody = "Hidden";
                }
            }

            var originalBody = context.Response.Body;
            await using var tempResponseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = tempResponseBody;

            await _next(context);

            var responseBody = await ObtainResponseBody(context);
            if (string.IsNullOrEmpty(responseBody))
            {
                responseBody = "Empty";
            }
            else if (controllerName.ToLower().Contains("auth"))
            {
                responseBody = "Hidden";
            }
            _logger.LogInformation($"User:           {login}");
            _logger.LogInformation($"Platform:       {platform}");
            _logger.LogInformation($"Request method: {requestMethod}");
            _logger.LogInformation($"Request path:   {requestPath}");
            _logger.LogInformation("-".PadLeft(40, '-'));
            _logger.LogInformation($"Request body:   {requestBody}");
            _logger.LogInformation("-".PadLeft(40, '-'));
            _logger.LogInformation($"Response body:  {responseBody}");
            await tempResponseBody.CopyToAsync(originalBody);
            _logger.LogInformation("=".PadLeft(40, '='));
        }

        /// <summary>
        /// Asynchronously reads request body
        /// </summary>
        /// <param name="request">http request</param>
        /// <returns></returns>
        private static async Task<string> ObtainRequestBody(HttpRequest request)
        {
            if (request.Body == null || string.IsNullOrEmpty(request.ContentType)) return string.Empty;
            request.EnableBuffering();
            var encoding = GetEncodingFromContentType(request.ContentType);
            string bodyStr;
            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            request.Body.Seek(0, SeekOrigin.Begin);
            return bodyStr;
        }

        /// <summary>
        /// Asynchronously reads response body
        /// </summary>
        /// <param name="context">http context</param>
        /// <returns></returns>
        private static async Task<string> ObtainResponseBody(HttpContext context)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var encoding = GetEncodingFromContentType(response.ContentType);
            using (var reader = new StreamReader(response.Body, encoding,
                detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true))
            {
                var text = await reader.ReadToEndAsync().ConfigureAwait(false);
                response.Body.Seek(0, SeekOrigin.Begin);
                return text;
            }
        }
        private static Encoding GetEncodingFromContentType(string contentTypeStr)
        {
            if (string.IsNullOrEmpty(contentTypeStr))
            {
                return Encoding.UTF8;
            }
            ContentType contentType;
            try
            {
                contentType = new ContentType(contentTypeStr);
            }
            catch (FormatException)
            {
                return Encoding.UTF8;
            }
            if (string.IsNullOrEmpty(contentType.CharSet))
            {
                return Encoding.UTF8;
            }
            return Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
    }
}
