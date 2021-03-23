using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models.DatabaseModels;
using VirtualSports.Web.Services.DatabaseServices;
namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Web or Mobile
        /// </summary>
        [FromHeader(Name = "X-Platform")]
        public string Platform { get; set; }

        private readonly ILogger<UserController> _logger;
        private readonly IDatabaseUserService _dbUserService;

        public UserController(ILogger<UserController> logger, IDatabaseUserService dbUserService)
        {
            _logger = logger;
            _dbUserService = dbUserService;
        }

        /// <summary>
        /// Get user's favourites games
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("favourites")]
        [ProducesResponseType(typeof(List<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetFavourites(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetFavouritesAsync(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        /// <summary>
        /// Get user's recent games
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recent")]
        [ProducesResponseType(typeof(List<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetRecentAsync(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        /// <summary>
        /// Add favourite game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("favourite/{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> AddToFavourites(CancellationToken cancellationToken,
            [FromRoute] Guid gameId)
        {
            return await _dbUserService.TryAddFavouriteAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken)
                 ? Ok()
                 : NotFound();
        }

        /// <summary>
        /// Get user's bet history
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history")]
        [ProducesResponseType(typeof(List<Bet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBetHistory(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
