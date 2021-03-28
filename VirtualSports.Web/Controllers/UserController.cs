using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Lib.Models;
using VirtualSports.Web.Contracts.ViewModels;
using VirtualSports.Web.Filters;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
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
        [AllowAnonymous]
        [Route("favourites")]
        [ProducesResponseType(typeof(IEnumerable<GameView>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GameView>>> GetFavourites(CancellationToken cancellationToken)
        {
            var userLogin = HttpContext.User.Identity?.Name;
            var favouriteGames = await _dbUserService.GetFavouritesAsync(userLogin, Platform, cancellationToken);

            return favouriteGames == null ? NotFound() : Ok(favouriteGames);
        }

        /// <summary>
        /// Get user's recent games
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recent")]
        [ProducesResponseType(typeof(IEnumerable<GameView>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GameView>>> GetRecent(CancellationToken cancellationToken)
        {
            var userLogin = HttpContext.User.Identity?.Name;
            var recentGames = await _dbUserService.GetRecentAsync(userLogin, Platform, cancellationToken);

            return recentGames == null ? NotFound() : Ok(recentGames.Take(4));
        }

        [HttpGet]
        [Route("recommended")]
        [ProducesResponseType(typeof(IEnumerable<GameView>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GameView>>> GetRecommended(CancellationToken cancellationToken)
        {
            var userLogin = HttpContext.User.Identity?.Name;
            var recomendedGames = await _dbUserService.GetRecommendedAsync(userLogin, Platform, cancellationToken);
            return Ok(recomendedGames);
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
            var userLogin = HttpContext.User.Identity?.Name;
            await _dbUserService.AddFavouriteAsync(
                userLogin, gameId, Platform, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Route("favourite/{gameId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteFromFavourites([FromRoute] string gameId, CancellationToken cancellationToken)
        {
            var userLogin = HttpContext.User.Identity?.Name;
            await _dbUserService.DeleteFavouriteAsync(
                userLogin, gameId, Platform, cancellationToken);

            return Ok();
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
            var userLogin = HttpContext.User.Identity?.Name;
            var history = await _dbUserService.GetBetsStoryAsync(userLogin, Platform, cancellationToken);

            return Ok(history.Take(100).ToList() ?? new List<Bet>());
        }
    }
}
