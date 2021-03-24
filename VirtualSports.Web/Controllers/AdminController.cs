using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services.DatabaseServices;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for postman
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDatabaseRootService _dbRootService;

        public AdminController(ILogger<AdminController> logger, IDatabaseRootService dbRootService)
        {
            _logger = logger;
            _dbRootService = dbRootService;
        }
        
        [HttpPost]
        [Route("add/game")]
        public async Task<ActionResult> AddGame([FromBody] Game game, CancellationToken cancellationToken)
        {
            if (game == null) return BadRequest();
            return await _dbRootService.AddGameAsync(game, cancellationToken)
                ? Ok()
                : Conflict("Game with such id already exists");
        }

        [HttpPost]
        [Route("add/games")]
        public async Task<ActionResult> AddGames([FromBody] List<Game> games, CancellationToken cancellationToken)
        {
            if (games == null) return BadRequest();
            return await _dbRootService.AddGamesAsync(games, cancellationToken)
                ? Ok()
                : Conflict("Game or games with such id already exists");
        }

        [HttpPost]
        [Route("add/categories")]
        public async Task<ActionResult> AddCategory([FromBody] List<Category> categories, CancellationToken cancellationToken)
        {
            if (categories == null) return BadRequest();
            return await _dbRootService.AddCategoriesAsync(categories, cancellationToken)
                ? Ok()
                : Conflict("Category with such id already exists");
        }

        [HttpPost]
        [Route("add/providers")]
        public async Task<ActionResult> AddProvider([FromBody] List<Provider> providers, CancellationToken cancellationToken)
        {
            if (providers == null) return BadRequest();
            return await _dbRootService.AddProviderAsync(providers, cancellationToken)
                ? Ok()
                : Conflict("Provider with such id already exists");
        }

        [HttpPost]
        [Route("add/tags")]
        public async Task<ActionResult> Addtags([FromBody] List<Tag> tags, CancellationToken cancellationToken)
        {
            if (tags == null) return BadRequest();
            return await _dbRootService.AddTagsAsync(tags, cancellationToken)
                ? Ok()
                : Conflict("Tag with such id already exists");
        }
    }
}
