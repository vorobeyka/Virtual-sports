using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Web.Contracts.ViewModels;
using VirtualSports.Web.Filters;
using VirtualSports.Web.Services;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for authorization.
    /// </summary>
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDatabaseAuthService _dbAuthService;
        private readonly ISessionStorage _sessionStorage;
        
        /// <summary>
        /// Constructor with DI.
        /// </summary>
        public AuthController(
            ILogger<AuthController> logger,
            IDatabaseAuthService dbAuthService,
            ISessionStorage sessionStorage)
        {
            _logger = logger;
            _dbAuthService = dbAuthService;
            _sessionStorage = sessionStorage;
        }

        /// <summary>
        /// Registration.
        /// </summary>
        /// <returns>Action result</returns>
        /// <response code="200">Returns token.</response>
        /// <response code="400">Invalid model state.</response>
        /// <response code="409">When login is used.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Conflict)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] Account user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token =  await _dbAuthService
                .RegisterUserAsync(user.Login, user.Password, cancellationToken);

            if (token == null) return Conflict("Login has been used already.");

            return Ok(token);
        }

        /// <summary>
        /// LogIn.
        /// </summary>
        /// <returns>Action result.</returns>
        /// <response code="200">Returns token.</response>
        /// <response code="400">Invalid model state.</response>
        /// <response code="404">When username or password is wrong.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] Account user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _dbAuthService
                .LoginUserAsync(user.Login, user.Password, cancellationToken);

            if (token == null) return NotFound("Wrong username or password.");

            return Ok(token);
        }

        /// <summary>
        /// LogOut.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPut("logout")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            if (Request == null || !Request.Headers.TryGetValue("Authorization", out var authHeader))
                return Unauthorized();

            var token = authHeader.ToString().Split(' ')[1];
            _sessionStorage.Add(token);
            await  _dbAuthService.ExpireToken(token, cancellationToken);

            return Ok();
        }
    }
}   