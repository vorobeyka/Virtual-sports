using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.AdminServices;
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
        private readonly IAdminAddService _adminAddService;
        private readonly IAdminUpdateService _adminUpdateService;
        private readonly IAdminDeleteService _adminDeleteService;
        private readonly IMapper _mapper;

        [FromHeader(Name = "X-Auth")]
        public string Auth { get; set; }

        public AdminController(
            IAdminAddService adminAddService,
            IAdminUpdateService adminUpdateService,
            IAdminDeleteService adminDeleteService,
            ILogger<AdminController> logger,
            IMapper mapper)
        {
            _adminAddService = adminAddService;
            _adminUpdateService = adminUpdateService;
            _adminDeleteService = adminDeleteService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add/games")]
        public async Task<IActionResult> AddGames(
            [FromBody] IEnumerable<GameRequest> games,
            CancellationToken cancellationToken)
        {
            var gamesDTO = _mapper.Map<IEnumerable<GameDTO>>(games);
            await _adminAddService.AddGames(gamesDTO, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/categories")]
        public async Task<IActionResult> AddCategories(
            [FromBody] IEnumerable<CategoryRequest> categories,
            CancellationToken cancellationToken)
        {
            var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            await _adminAddService.AddCategories(categoriesDTO, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/providers")]
        public async Task<IActionResult> AddProviders(
            [FromBody] IEnumerable<ProviderRequest> providers,
            CancellationToken cancellationToken)
        {
            var providersDTO = _mapper.Map<IEnumerable<ProviderDTO>>(providers);
            await _adminAddService.AddProviders(providersDTO, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("add/tags")]
        public async Task<IActionResult> AddTags(
            [FromBody] IEnumerable<TagRequest> tags,
            CancellationToken cancellationToken)
        {
            var tagsDTO = _mapper.Map<IEnumerable<TagDTO>>(tags);
            await _adminAddService.AddTags(tagsDTO, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("update/game")]
        public async Task<IActionResult> UpdateGame(
            [FromBody] GameRequest game,
            CancellationToken cancellationToken)
        {
            var gameDTO = _mapper.Map<GameDTO>(game);
            await _adminUpdateService.UpdateGame(gameDTO, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("update/provider")]
        public async Task<IActionResult> UpdateProvider(
            [FromBody] ProviderRequest provider,
            CancellationToken cancellationToken)
        {
            var providerDTO = _mapper.Map<ProviderDTO>(provider);
            await _adminUpdateService.UpdateProvider(providerDTO, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("update/category")]
        public async Task<IActionResult> UpdateCategory(
            [FromBody] CategoryRequest category,
            CancellationToken cancellationToken)
        {
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            await _adminUpdateService.UpdateCategory(categoryDTO, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("update/tag")]
        public async Task<IActionResult> UpdateTag(
            [FromBody] TagRequest tag,
            CancellationToken cancellationToken)
        {
            var tagDTO = _mapper.Map<TagDTO>(tag);
            await _adminUpdateService.UpdateTag(tagDTO, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/game/{id}")]
        public async Task<IActionResult> DeleteGame(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteGame(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/category/{id}")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteCategory(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/provider/{id}")]
        public async Task<IActionResult> DeleteProvider(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteProvider(id, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/tag/{id}")]
        public async Task<IActionResult> DeleteTag(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteTag(id, cancellationToken);
            return Ok();
        }
    }
}
