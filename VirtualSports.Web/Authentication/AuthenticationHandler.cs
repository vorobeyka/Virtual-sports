using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VirtualSports.BE.Services;

namespace VirtualSports.Web.Authentication
{
    /// <inheritdoc />
    public class AuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private readonly ISessionStorage _storage;
        /// <inheritdoc />
        public AuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISessionStorage sessionStorage)
            : base(options, logger, encoder, clock)
        {
            _storage = sessionStorage;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers[HeaderNames.Authorization].FirstOrDefault();
            if (authHeader == null) return AuthenticateResult.NoResult();
            var token = authHeader.Split(' ')[1];

            if (_storage.Contains(token)) return AuthenticateResult.NoResult();

            try
            {
                var identity = new ClaimsIdentity(
                    new[] {new Claim(ClaimTypes.NameIdentifier, "token")},
                    Scheme.Name);
                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        Scheme.Name));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Auth error");
                return AuthenticateResult.Fail(e);
            }
            
        }
    }
}