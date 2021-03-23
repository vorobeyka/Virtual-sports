using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Services.DatabaseServices;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for games requests
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        /// <summary>
        /// Web or Mobile
        /// </summary>
        [FromHeader(Name ="X-Platform")]
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
        [ProducesResponseType(typeof(Root), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<ActionResult<Root>> GetAllData(CancellationToken cancellationToken)
        {
            var data = await _dbRootService.GetRootAsync(cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Play chosen game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="gameId"></param>
        /// <param name="dbUserService"></param>
        /// <returns></returns>
        [HttpGet("play/{gameId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Game), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Game>> PlayGame(CancellationToken cancellationToken,
            [FromRoute] Guid gameId, [FromServices] IDatabaseUserService dbUserService)
        {
            bool isAdded;
            switch (Platform)
            {
                case "Mobile":
                    isAdded = await dbUserService.TryAddRecentAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken);
                    break;
                case "Web":
                    isAdded = await dbUserService.TryAddRecentAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken);
                    break;
                default: return BadRequest("Unsupported platform!");
            }
            if(!isAdded)
            {
                return NotFound("there is no game with such id in database");
            }
            // maybe change into just OK();
            return Ok(await _dbRootService.GetGameAsync(gameId.ToString(), cancellationToken));
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
        [HttpGet("play/dice")]
        [ProducesResponseType(typeof(Bet), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bet>> PlayDice(CancellationToken cancellationToken,
            [FromQuery] string dateTime, 
            [FromQuery] BetType betType,
            [FromServices] IDatabaseUserService dbUserService,
            [FromServices] IDiceService diceService)
        {
            if (string.IsNullOrEmpty(dateTime)) return BadRequest();

            var diceRoll = new Random().Next(7);
            var result = await diceService.GetBetResultAsync(diceRoll, betType);
            var bet = new Bet
            {
                Id = Guid.NewGuid().ToString(),
                BetType = betType,
                DroppedNumber = diceRoll,
                IsBetWon = result,
                DateTime = dateTime
            };
            await dbUserService.AddBetAsync(HttpContext.User.Identity.Name, bet, cancellationToken);
            return Ok(bet);
        }
    }
}
