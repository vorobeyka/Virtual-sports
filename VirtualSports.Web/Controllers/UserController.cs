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
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        [FromHeader(Name = "X-Platform")]
        public string Platform { get; set; }

        private readonly ILogger<UserController> _logger;
        private readonly IDatabaseUserService _dbUserService;

        public UserController(ILogger<UserController> logger, IDatabaseUserService dbUserService)
        {
            _logger = logger;
            _dbUserService = dbUserService;
        }

        [HttpGet]
        [Route("favourites")]
        [ProducesResponseType(typeof(List<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetFavourites(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetFavouritesAsync(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet]
        [Route("recent")]
        [ProducesResponseType(typeof(List<Game>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetRecentAsync(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet]
        [Route("favourite/{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> AddToFavourites(CancellationToken cancellationToken,
            [FromRoute] Guid gameId)
        {
            return await _dbUserService.TryAddFavouriteAsync(HttpContext.User.Identity.Name, gameId,
                cancellationToken)
                 ? Ok()
                 : NotFound();
        }

        [HttpGet]
        [Route("history")]
        [ProducesResponseType(typeof(List<Bet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBetHistory(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
