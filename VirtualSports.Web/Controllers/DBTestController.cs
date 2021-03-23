using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models;
using VirtualSports.BE.Models.DatabaseModels;
using VirtualSports.BE.Services.DatabaseServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VirtualSports.BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBTestController : ControllerBase
    {
        IDatabaseRootService _databaseRootService;

        public DBTestController(IDatabaseRootService databaseRootService)
        {
            _databaseRootService = databaseRootService;
        }

        // GET: api/<DBTestController>
        [HttpGet]
        public async Task<Root> GetRoot(CancellationToken cancellationToken)
        {
            var root = await _databaseRootService.GetRootAsync(cancellationToken);
            return root;
        }

        [HttpGet]
        [Route("GetGame")]
        public async Task<Game> GetGame([FromQuery]string id, CancellationToken cancellationToken)
        {
            var game = await _databaseRootService.GetGameAsync(id, cancellationToken);
            return game;
        }

        [HttpGet]
        [Route("GetProvider")]
        public async Task<Provider> GetProvider([FromQuery] string id, CancellationToken cancellationToken)
        {
            var provider = await _databaseRootService.GetProviderAsync(id, cancellationToken);
            return provider;
        }

        [HttpGet]
        [Route("GetCategory")]
        public async Task<Category> GetCategory([FromQuery] string id, CancellationToken cancellation)
        {
            var category = await _databaseRootService.GetCategoryAsync(id, cancellation);
            return category;
        }


        // POST api/<DBTestController>
        [HttpPost]
        [Route("UpdateRoot")]
        public async Task<ActionResult> Post([FromBody] Root root, CancellationToken cancellationToken)
        {
            await _databaseRootService.AddRootAsync(root, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("AddGame")]
        public async Task<ActionResult<Game>> Post([FromBody] Game game, CancellationToken cancellationToken)
        {
            await _databaseRootService.AddGameAsync(game, cancellationToken);

            return Ok(game);
        }

        [HttpPost]
        [Route("AddGames")]
        public async Task<ActionResult> Post(
            [FromBody] List<Game> games,
            CancellationToken cancellationToken)
        {
            await _databaseRootService.AddGamesAsync(games, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("AddProviders")]
        public async Task<ActionResult> Post(
            [FromBody] List<Provider> providers,
            CancellationToken cancellationToken)
        {
            await _databaseRootService.AddProviderAsync(providers, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("AddCategories")]
        public async Task<IActionResult> Post(
            [FromBody] List<Category> categories,
            CancellationToken cancellationToken)
        {
            await _databaseRootService.AddCategoriesAsync(categories, cancellationToken);
            return Ok();
        }

        
    }
}
