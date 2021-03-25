using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Filters;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services.DatabaseServices;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for postman
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(ValidateAdminHeaderFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDatabaseAdminService _dbAdminService;

        [FromHeader(Name = "X-Admin")]
        public string Admin { get; set; }

        public AdminController(
            IDatabaseAdminService dbAdminService,
            ILogger<AdminController> logger)
        {
            _logger = logger;
            _dbAdminService = dbAdminService;
        }
        
        [HttpPost]
        [Route("add/game")]
        public async Task<ActionResult> AddGame(
            [FromBody] Game game,
            CancellationToken cancellationToken)
        {
            if (game == null) return BadRequest();

            return await _dbAdminService.AddGameAsync(game, cancellationToken)
                ? Ok()
                : Conflict("Game with such id already exists.");
        }

        [HttpPost]
        [Route("add/games")]
        public async Task<ActionResult> AddGames(
            [FromBody] List<Game> games,
            CancellationToken cancellationToken)
        {
            if (games == null) return BadRequest();

            return await _dbAdminService.AddGamesAsync(games, cancellationToken)
                ? Ok()
                : Conflict("Game with one of the ids already exists.");
        }

        [HttpPost]
        [Route("add/categories")]
        public async Task<ActionResult> AddCategory(
            [FromBody] List<Category> categories,
            CancellationToken cancellationToken)
        {
            if (categories == null) return BadRequest();

            return await _dbAdminService.AddCategoriesAsync(categories, cancellationToken)
                ? Ok()
                : Conflict("Category with one of the ids already exists.");
        }

        [HttpPost]
        [Route("add/providers")]
        public async Task<ActionResult> AddProvider(
            [FromBody] List<Provider> providers,
            CancellationToken cancellationToken)
        {
            if (providers == null) return BadRequest();

            return await _dbAdminService.AddProviderAsync(providers, cancellationToken)
                ? Ok()
                : Conflict("Provider with one of the ids already exists.");
        }

        [HttpPost]
        [Route("add/tags")]
        public async Task<ActionResult> Addtags(
            [FromBody] List<Tag> tags,
            CancellationToken cancellationToken)
        {
            if (tags == null) return BadRequest();

            return await _dbAdminService.AddTagsAsync(tags, cancellationToken)
                ? Ok()
                : Conflict("Tag with one of the ids already exists.");
        }

        [HttpDelete]
        [Route("delete/game/{id}")]
        public async Task<ActionResult> DeleteGame(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            return await _dbAdminService.DeleteGameAsync(id, cancellationToken)
                ? Ok()
                : Conflict("Game with such id doesn't exists.");
        }

        [HttpDelete]
        [Route("delete/category/{id}")]
        public async Task<ActionResult> DeleteCategory(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            return await _dbAdminService.DeleteCategoryAsync(id, cancellationToken)
                ? Ok()
                : Conflict("Category with such id doesn't exists.");
        }

        [HttpDelete]
        [Route("delete/provider/{id}")]
        public async Task<ActionResult> DeleteProvider(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            return await _dbAdminService.DeleteProviderAsync(id, cancellationToken)
                ? Ok()
                : Conflict("Provider with such id doesn't exists.");
        }

        [HttpDelete]
        [Route("delete/tag/{id}")]
        public async Task<ActionResult> DeleteTag(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            return await _dbAdminService.DeleteTagAsync(id, cancellationToken)
                ? Ok()
                : Conflict("Tag with such id doesn't exists.");
        }
    }
}
