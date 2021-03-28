using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.Mappings;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Lib.Models;
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
        private readonly IMapper _mapper;
        private readonly IRootService _dbRootService;

        public GamesController(
            ILogger<GamesController> logger,
            IMapper mapper,
            IRootService dbRootService)
        {
            _logger = logger;
            _mapper = mapper;
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
            var data = await _dbRootService.GetRootAsync(Platform, cancellationToken);

            return Ok(_mapper.Map<RootView>(data));
        }

        /// <summary>
        /// Play chosen game.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="gameId"></param>
        /// <param name="dbUserService"></param>
        /// <returns></returns>
        [HttpGet("play/{gameId}")]
        [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GameView), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GameView>> PlayGame(
            CancellationToken cancellationToken,
            [FromRoute] string gameId,
            [FromServices] IUserService dbUserService)
        {
            var userLogin = HttpContext.User.Identity?.Name;

            if (string.IsNullOrEmpty(userLogin)) return BadRequest("Invalid user!");

            await dbUserService.AddRecentAsync(userLogin, gameId, Platform, cancellationToken);
            var game = await _dbRootService.GetGameAsync(gameId, cancellationToken);
            return Ok(_mapper.Map<GameView>(game));
        }

        /// <summary>
        /// Throw dice.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="diceBet"></param>
        /// <param name="dbUserService"></param>
        /// <param name="diceService"></param>
        /// <returns></returns>
        [HttpPost("play/dice")]
        [TypeFilter(typeof(ValidatePlatformHeaderFilter))]
        [ProducesResponseType(typeof(Bet), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Bet>> PlayDice(
            CancellationToken cancellationToken,
            [FromBody] DiceBetValidationModel diceBet,
            [FromServices] IUserService dbUserService,
            [FromServices] IDiceService diceService)
        {
            var userLogin = HttpContext.User.Identity?.Name;

            if (string.IsNullOrEmpty(userLogin)) return BadRequest("Invalid user!");

            var diceRoll = new Random().Next(1, 7);
            var result = await diceService.GetBetResultAsync(diceRoll, (BetType)diceBet.BetType);
            var bet = new Bet
            {
                Id = Guid.NewGuid().ToString(),
                BetType = (BetType)diceBet.BetType,
                DroppedNumber = diceRoll,
                IsBetWon = result,
                DateTime = diceBet.DateTime
            };
            
            await dbUserService.AddBetAsync(userLogin, bet, cancellationToken);
            await dbUserService.AddRecentAsync(userLogin, "original_dice_game", Platform,
                cancellationToken);

            return Ok(bet);
        }
    }
}
