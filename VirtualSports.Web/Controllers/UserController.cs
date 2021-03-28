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
using AutoMapper;

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
        private readonly IUserService _dbUserService;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<UserController> logger,
            IUserService dbUserService,
            IMapper mapper)
        {
            _logger = logger;
            _dbUserService = dbUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user's favourites games
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("favourites")]
        [ProducesResponseType(typeof(IEnumerable<GameView>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GameView>>> GetFavourites(CancellationToken cancellationToken)
        {
            var userLogin = HttpContext.User.Identity?.Name;
            var favouriteGamesDTO = await _dbUserService.GetFavouritesAsync(userLogin, Platform, cancellationToken);
            var favouriteGames = _mapper.Map<IEnumerable<GameView>>(favouriteGamesDTO);

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
            var recentGamesDTO = await _dbUserService.GetRecentAsync(userLogin, Platform, cancellationToken);
            var recentGames = _mapper.Map<IEnumerable<GameView>>(recentGamesDTO);

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
            var recommendedGamesDTO = await _dbUserService.GetRecommendedAsync(userLogin, Platform, cancellationToken);
            var recommendedGames = _mapper.Map<IEnumerable<GameView>>(recommendedGamesDTO);

            return Ok(recommendedGames);
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
                userLogin, gameId, cancellationToken);

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
            var history = await _dbUserService.GetBetsStoryAsync(userLogin, cancellationToken);

            return Ok(history.Take(100).ToList() ?? new List<Bet>());
        }
    }
}
