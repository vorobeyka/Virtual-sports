using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VirtualSports.Web.Services;

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
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers[HeaderNames.Authorization].FirstOrDefault();
            if (authHeader == null) return Task.FromResult(AuthenticateResult.NoResult());
            var token = authHeader.Split(' ')[1];

            if (_storage.Contains(token)) return Task.FromResult(AuthenticateResult.NoResult());
            var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var login = securityToken.Claims.ToList()[0].Value;
            try
            {
                var identity = new ClaimsIdentity(
                    new[] {new Claim(ClaimTypes.Name, login)},
                    Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        Scheme.Name)));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Auth error");
                return Task.FromResult(AuthenticateResult.Fail(e));
            }
            
        }
    }
}