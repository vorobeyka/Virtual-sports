using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using VirtualSports.BE.Models;
using VirtualSports.BE.Options;
using VirtualSports.BE.Services;

namespace VirtualSports.BE.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// 
        /// </summary>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType((int) HttpStatusCode.Conflict)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] User user)
        {
            var canRegister =  await _authService.RegisterAsync(user);
            if (!canRegister) return Conflict("Login has been used already.");
            return await LoginAsync(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var identity = await GetIdentityAsync(user);

            if (identity == null) return NotFound("Wrong username or password.");

            var now = DateTime.UtcNow;
            var jwtToken =
                new JwtSecurityToken(JwtOptions.Issuer, JwtOptions.Audience, notBefore: now,
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(encodedJwt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok("Hello, world");
        }

        private async Task<ClaimsIdentity?> GetIdentityAsync(User user)
        {
            var canLogin = await _authService.FindAsync(user);
            if (!canLogin) return null;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Password)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}