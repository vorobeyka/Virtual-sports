﻿using System;
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
        private readonly IDatabaseAuthService _dbAuthService;
        private readonly ISessionStorage _sessionStorage;

        /// <summary>
        /// 
        /// </summary>
        public AuthController(
            IAuthService authService,
            IDatabaseAuthService dbAuthService,
            ISessionStorage sessionStorage)
        {
            _authService = authService;
            _dbAuthService = dbAuthService;
            _sessionStorage = sessionStorage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType((int) HttpStatusCode.Conflict)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] Account user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token =  await _dbAuthService.RegisterUserAsync(user, cancellationToken);
            if (token == null) return Conflict("Login has been used already.");

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
        public async Task<IActionResult> LoginAsync([FromBody] Account user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _dbAuthService.LoginUserAsync(user, cancellationToken);
            if (token == null) return NotFound("Wrong username or password.");

            return Ok(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("logout")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Authorize]
        public IActionResult Logout(CancellationToken cancellationToken)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader)) return Unauthorized();
            var token = authHeader.ToString().Split(' ')[1];

            _sessionStorage.Add(token);
            _dbAuthService.ExpireToken(token, cancellationToken);
            return Ok();
        }

        
    }
}