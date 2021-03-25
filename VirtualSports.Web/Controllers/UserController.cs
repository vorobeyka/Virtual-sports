using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Mappings;
using VirtualSports.Web.Models;
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
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity.Name;

            if (platformType == PlatformType.UnknownPlatform) return BadRequest("Unsupported platform!");

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
        [ProducesResponseType(typeof(List<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetRecent(CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity.Name;

            if (platformType == PlatformType.UnknownPlatform) return BadRequest("Unsupported platform!");

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
        public async Task<ActionResult> AddToFavourites([FromRoute] string gameId, CancellationToken cancellationToken)
        {
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity.Name;

            if (platformType == PlatformType.UnknownPlatform) return BadRequest("Unsupported platform!");

            var isAdded = await _dbUserService.TryAddFavouriteAsync(
                userLogin, gameId, platformType, cancellationToken);

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
            var platformType = MapMethods.MapPlayformType(Platform);
            var userLogin = HttpContext.User.Identity.Name;

            if (platformType == PlatformType.UnknownPlatform) return BadRequest("Unsupported platform!");

            var history = await _dbUserService.GetBetsStoryAsync(userLogin, platformType, cancellationToken);

            return Ok(history.TakeLast(100));
        }
    }
}
