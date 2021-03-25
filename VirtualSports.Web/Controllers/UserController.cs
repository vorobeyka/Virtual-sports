using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Filters;
using VirtualSports.Web.Mappings;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services.DatabaseServices;
using System.Collections;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
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
        [ProducesResponseType(typeof(IEnumerable<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetFavourites(CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;
            var favouriteGames = await _dbUserService.GetFavouritesAsync(userLogin, platformType, cancellationToken);

            return favouriteGames == null ? NotFound() : Ok(favouriteGames);
        }

        /// <summary>
        /// Get user's recent games
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recent")]
        [ProducesResponseType(typeof(IEnumerable<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable>> GetRecent(CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;
            var recentGames = await _dbUserService.GetRecentAsync(userLogin, platformType, cancellationToken);

            return recentGames == null ? NotFound() : Ok(recentGames);
        }

        /// <summary>
        /// Add favourite game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("favourite/{gameId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddToFavourites([FromRoute] string gameId, CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;
            var isAdded = await _dbUserService.TryAddFavouriteAsync(
                userLogin, gameId, platformType, cancellationToken);

            return isAdded 
                ? Ok()
                : NotFound("there is no game with such id in database");
        }

        [HttpDelete]
        [Route("favourite/{gameId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteFromFavourites([FromRoute] string gameId, CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;
            var isDeleted = await _dbUserService.TryAddFavouriteAsync(
                userLogin, gameId, platformType, cancellationToken);

            return isDeleted
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
        [ProducesResponseType(typeof(IEnumerable<Bet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBetHistory(CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;
            var history = await _dbUserService.GetBetsStoryAsync(userLogin, platformType, cancellationToken);

            return Ok(history.TakeLast(100));
        }
    }
}
