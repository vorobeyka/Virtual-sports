using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.DAL.Entities;
using VirtualSports.Web.Contracts.AdminContracts;
using VirtualSports.Web.Filters;

namespace VirtualSports.Web.Controllers
{
    /// <summary>
    /// Controller for postman
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(ValidateAdminHeaderFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDatabaseAdminService _dbAdminService;
        private readonly IAdminAddService _adminAddService;
        private readonly IMapper _mapper;

        [FromHeader(Name = "X-Auth")]
        public string Auth { get; set; }

        public AdminController(
            IAdminAddService adminAddService,
            IDatabaseAdminService dbAdminService,
            ILogger<AdminController> logger,
            IMapper mapper)
        {
            _adminAddService = adminAddService;
            _dbAdminService = dbAdminService;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("add/game")]
        public async Task<IActionResult> AddGame(
            [FromBody] GameRequest game,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.AddAsync(game, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/games")]
        public async Task<IActionResult> AddGames(
            [FromBody] IEnumerable<GameRequest> games,
            CancellationToken cancellationToken)
        {
            await _adminAddService.AddGames(
                _mapper.Map<IEnumerable<GameDTO>>(games), cancellationToken);
            
            //await _dbAdminService.AddRangeAsync(games, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/categories")]
        public async Task<IActionResult> AddCategories(
            [FromBody] IEnumerable<CategoryRequest> categories,
            CancellationToken cancellationToken)
        {
            await _adminAddService.AddCategories(
                _mapper.Map<IEnumerable<CategoryDTO>>(categories),
                cancellationToken);

            //await _dbAdminService.AddRangeAsync(categories, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/providers")]
        public async Task<IActionResult> AddProviders(
            [FromBody] IEnumerable<ProviderRequest> providers,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.AddRangeAsync(providers, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/tags")]
        public async Task<IActionResult> AddTags(
            [FromBody] IEnumerable<TagRequest> tags,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.AddRangeAsync(tags, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("update/game")]
        public async Task<IActionResult> UpdateGame(
            [FromBody] GameRequest game,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.UpdateAsync(game, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("update/provider")]
        public async Task<IActionResult> UpdateProvider(
            [FromBody] ProviderRequest provider,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.UpdateAsync(provider, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("update/category")]
        public async Task<IActionResult> UpdateCategory(
            [FromBody] CategoryRequest category,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.UpdateAsync(category, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("update/tag")]
        public async Task<IActionResult> UpdateTag(
            [FromBody] TagRequest tag,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.UpdateAsync(tag, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/game/{id}")]
        public async Task<IActionResult> DeleteGame(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.DeleteAsync<Game>(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/category/{id}")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.DeleteAsync<Category>(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/provider/{id}")]
        public async Task<IActionResult> DeleteProvider(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.DeleteAsync<Provider>(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/tag/{id}")]
        public async Task<IActionResult> DeleteTag(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _dbAdminService.DeleteAsync<Tag>(id, cancellationToken);
            return Ok();
        }
    }
}
