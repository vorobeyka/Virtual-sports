using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using VirtualSports.BE.Models;
using VirtualSports.BE.Options;
using VirtualSports.BE.Services;
using VirtualSports.BE.Services.DatabaseServices;

namespace VirtualSports.BE.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IDatabaseAuthService _dbUserService;

        /// <summary>
        /// 
        /// </summary>
        public AuthController(
            IAuthService authService,
            IDatabaseAuthService dbUserService)
        {
            _authService = authService;
            _dbUserService = dbUserService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType((int) HttpStatusCode.Conflict)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] User user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var canRegister =  await _dbUserService.RegisterUserAsync(user.Login, user.Password, cancellationToken);
            if (!canRegister) return Conflict("Login has been used already.");

            var token = await GetJwtTokenAsync(user);
            
            return Ok(token);
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
        public async Task<IActionResult> LoginAsync([FromBody] User user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var canLogin = await _dbUserService.LoginUserAsync(user.Login, user.Password, cancellationToken);
            if (!canLogin) return NotFound("Wrong username or password.");

            var token = await GetJwtTokenAsync(user);

            return Ok(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader)) return Unauthorized();
            var token = authHeader.ToString().Split(' ')[1];

            // TODO : ADD TO DB and MAKE SESSION STORAGE IN MEMORY.
            return Ok(token);

        }

        private async Task<string> GetJwtTokenAsync(User user)
        {
            var identity = await GetIdentityAsync(user);

            var now = DateTime.UtcNow;
            var jwtToken =
                new JwtSecurityToken(JwtOptions.Issuer, JwtOptions.Audience, notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(JwtOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}