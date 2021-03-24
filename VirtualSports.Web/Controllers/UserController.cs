using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models.DatabaseModels;
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetFavourites(CancellationToken cancellationToken)
        {
            IEnumerable<Game> favourites;
            switch (Platform)
            {
                case "Mobile":
                    favourites = await _dbUserService.GetFavouritesMobileAsync(HttpContext.User.Identity.Name,
                        cancellationToken);
                    break;
                case "Web":
                    favourites = await _dbUserService.GetFavouritesAsync(HttpContext.User.Identity.Name,
                       cancellationToken);
                    break;
                default: return BadRequest("Unsupported platform!");
            }
            return favourites == null ? NotFound() : Ok(favourites);
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetRecent(CancellationToken cancellationToken)
        {
            IEnumerable<Game> recent;
            switch (Platform)
            {
                case "Mobile":
                    recent = await _dbUserService.GetRecentMobileAsync(HttpContext.User.Identity.Name,
                        cancellationToken);
                    break;
                case "Web":
                    recent = await _dbUserService.GetRecentAsync(HttpContext.User.Identity.Name,
                       cancellationToken);
                    break;
                default: return BadRequest("Unsupported platform!");
            }
            return recent == null ? NotFound() : Ok(recent);
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddToFavourites([FromRoute] Guid gameId,
            CancellationToken cancellationToken)
        {
            bool isAdded;
            switch (Platform)
            {
                case "Mobile":
                    isAdded = await _dbUserService.TryAddFavouriteMobileAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken);
                    break;
                case "Web":
                    isAdded = await _dbUserService.TryAddFavouriteAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken);
                    break;
                default: return BadRequest("Unsupported platform!");
            }

            return isAdded 
                ? Ok()
                : NotFound("there is no game with such id in database");
        }

        /// <summary>
        /// Get user's bet history
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history")]
        [ProducesResponseType(typeof(List<Bet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<ActionResult<IEnumerable<Bet>>> GetBetHistory(CancellationToken cancellationToken)
        {
            IEnumerable<Bet> history;
            switch (Platform)
            {
                case "Mobile":
                    history = await _dbUserService.GetBetsStoryMobileAsync(HttpContext.User.Identity.Name,
                        cancellationToken);
                    break;
                case "Web":
                    history = await _dbUserService.GetBetsStoryAsync(HttpContext.User.Identity.Name,
                        cancellationToken);
                    break;
                default: return BadRequest("Unsupported platform!");
            }
            return Ok(history);
        }
    }
}
