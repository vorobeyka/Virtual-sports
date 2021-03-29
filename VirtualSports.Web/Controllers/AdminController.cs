using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.Web.Contracts.AdminRequests;
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

        /// <summary>
        /// Add games.
        /// </summary>
        /// <param name="games">Games</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add categories.
        /// </summary>
        /// <param name="categories">Categories.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add providers.
        /// </summary>
        /// <param name="providers">Providers.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add tags.
        /// </summary>
        /// <param name="tags">tags</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update game.
        /// </summary>
        /// <param name="game">Chosen game.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update tag.
        /// </summary>
        /// <param name="tag">Chosen tag.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete game by id.
        /// </summary>
        /// <param name="id">Game id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/game/{id}")]
        public async Task<IActionResult> DeleteGame(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteGame(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete category by id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/category/{id}")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteCategory(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete provider by id.
        /// </summary>
        /// <param name="id">Provider id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/provider/{id}")]
        public async Task<IActionResult> DeleteProvider(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            await _adminDeleteService.DeleteProvider(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete tag by id.
        /// </summary>
        /// <param name="id">Tag id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
