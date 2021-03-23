using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models;
using VirtualSports.BE.Models.DatabaseModels;
using VirtualSports.BE.Services;
using VirtualSports.BE.Services.DatabaseServices;
using VirtualSports.Web.Services.DatabaseServices;

namespace VirtualSports.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        [FromHeader(Name ="X-Platform")]
        public string Platform { get; set; }

        private readonly ILogger<GamesController> _logger;
        private readonly IDatabaseRootService _dbRootService;

        public GamesController(ILogger<GamesController> logger, IDatabaseRootService dbRootService)
        {
            _logger = logger;
            _dbRootService = dbRootService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Root), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<ActionResult<Root>> GetAllData(CancellationToken cancellationToken)
        {
            var data = await _dbRootService.GetRootAsync(cancellationToken);
            return Ok(data);
        }

        [HttpGet("play/{gameId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Game), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Game>> PlayGame(CancellationToken cancellationToken,
            [FromRoute] Guid gameId, [FromServices] IDatabaseUserService dbUserService)
        {

            if(!(await dbUserService.TryAddRecentAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken)))
            {
                return NotFound("there is no game with such id in database");
            }
            return Ok(await _dbRootService.GetGameAsync(gameId.ToString(), cancellationToken));
        }

        [HttpGet("play/dice")]
        [ProducesResponseType(typeof(Bet), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Game>> PlayDice(CancellationToken cancellationToken,
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
