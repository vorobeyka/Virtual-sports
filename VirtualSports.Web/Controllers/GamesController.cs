using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.Mappings;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.DAL.Entities;
using VirtualSports.Web.Contracts;
using VirtualSports.Web.Filters;
using VirtualSports.Web.Contracts.ViewModels;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for games requests
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    public class GamesController : ControllerBase
    {
        /// <summary>
        /// Web or Mobile
        /// </summary>
        [FromHeader(Name = "X-Platform")]
        public string Platform { get; set; }

        private readonly ILogger<GamesController> _logger;
        private readonly IDatabaseRootService _dbRootService;

        public GamesController(ILogger<GamesController> logger, IDatabaseRootService dbRootService)
        {
            _logger = logger;
            _dbRootService = dbRootService;
        }

        /// <summary>
        /// Initial request for main page
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(RootView), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<ActionResult<RootView>> GetAllData(CancellationToken cancellationToken)
        {
            var platformType = MapPlatforms.MapPlatformType(Platform);
            var data = await _dbRootService.GetRootAsync(platformType, cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Play chosen game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="gameId"></param>
        /// <param name="dbUserService"></param>
        /// <returns></returns>
        [HttpGet("play/{gameId}")]
        [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Game), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Game>> PlayGame(CancellationToken cancellationToken,
            [FromRoute] string gameId, [FromServices] IDatabaseUserService dbUserService)
        {
            var platformType = MapPlatforms.MapPlatformType(Platform);
            var userLogin = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userLogin)) return BadRequest("Invalid user!");

            var isAdded = await dbUserService.AddRecentAsync(userLogin, gameId, platformType, cancellationToken);

            if (!isAdded)
            {
                return NotFound("there is no game with such id in database or game is already in recent played list");
            }
            // maybe change into just OK();
            return Ok(await _dbRootService.GetGameAsync(gameId, cancellationToken));
        }

        /// <summary>
        /// Throw dice
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="dateTime"></param>
        /// <param name="betType"></param>
        /// <param name="dbUserService"></param>
        /// <param name="diceService"></param>
        /// <returns></returns>
        [HttpPost("play/dice")]
        [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
        [ProducesResponseType(typeof(Bet), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Bet>> PlayDice(CancellationToken cancellationToken,
            [FromBody] DiceBetValidationModel diceBet,
            [FromServices] IDatabaseUserService dbUserService,
            [FromServices] IDiceService diceService)
        {
            var platformType = MapPlatforms.MapPlatformType(Platform);
            var userLogin = HttpContext.User.Identity?.Name;

            if (string.IsNullOrEmpty(userLogin)) return BadRequest("Invalid user!");

            var diceRoll = new Random().Next(1, 7);
            var result = await diceService.GetBetResultAsync(diceRoll, diceBet.BetType);
            var bet = new Bet
            {
                Id = Guid.NewGuid().ToString(),
                BetType = diceBet.BetType,
                DroppedNumber = diceRoll,
                IsBetWon = result,
                DateTime = diceBet.DateTime
            };
            
            await dbUserService.AddBetAsync(userLogin, bet, platformType, cancellationToken);
            await dbUserService.AddRecentAsync(userLogin, "original_dice_game", platformType,
                cancellationToken);

            return Ok(bet);
        }
    }
}
