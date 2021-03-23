using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services;
using VirtualSports.Web.Services.DatabaseServices;

namespace VirtualSports.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly IDatabaseRootService _dbRootService;

        public GamesController(ILogger<GamesController> logger, IDatabaseRootService dbRootService)
        {
            _logger = logger;
            _dbRootService = dbRootService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Root>> GetAllData(CancellationToken cancellationToken)
        {
            var data = await _dbRootService.GetRootAsync(cancellationToken);
            return Ok(data);
        }

        [HttpGet("play/{gameId:Guid}")]
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
