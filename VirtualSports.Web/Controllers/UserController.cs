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
        private readonly ILogger<UserController> _logger;
        private readonly IDatabaseUserService _dbUserService;

        public UserController(ILogger<UserController> logger, IDatabaseUserService dbUserService)
        {
            _logger = logger;
            _dbUserService = dbUserService;
        }

        [HttpGet]
        [Route("favourites")]
        public async Task<ActionResult> GetFavourites(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetFavourites(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet]
        [Route("recent")]
        public async Task<ActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var data = await _dbUserService.GetRecent(HttpContext.User.Identity.Name, cancellationToken);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet]
        [Route("favourite/{id:Guid}")]
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
