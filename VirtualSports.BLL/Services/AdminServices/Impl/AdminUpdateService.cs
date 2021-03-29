using AutoMapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Exceptions;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminUpdateService : IAdminUpdateService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;

        public AdminUpdateService(
            IMapper mapper,
            IRepository<Game> gameRepository,
            IRepository<Provider> providerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagRepository)
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _providerRepository = providerRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task UpdateGame(GameDTO gameDTO, CancellationToken cancellationToken = default)
        {
            await CheckGamesProperties(gameDTO, cancellationToken);
            var game = _mapper.Map<Game>(gameDTO);
            await _gameRepository.UpdateAsync(game, cancellationToken);
        }

        public async Task UpdateTag(TagDTO tagDTO, CancellationToken cancellationToken = default)
        {
            var tag = _mapper.Map<Tag>(tagDTO);
            await _tagRepository.UpdateAsync(tag, cancellationToken);
        }

        private async Task CheckGamesProperties(GameDTO game, CancellationToken cancellationToken)
        {
            var categories = (await _categoryRepository.GetAllAsync(cancellationToken))
                .Select(c => c.Id);
            var providers = (await _providerRepository.GetAllAsync(cancellationToken))
                .Select(p => p.Id);
            var tags = (await _tagRepository.GetAllAsync(cancellationToken))
                .Select(t => t.Id);

            if (!providers.Any(p => p == game.Provider)) throw new NotExistedProviderException(game.Provider);
            var isOk = (await _providerRepository.GetAsync(game.Provider, cancellationToken))
                .PlatformTypes.Any(p => game.PlatformTypes.Contains(p));
            if (!isOk) throw new InvalidProviderPlatformException(game.Provider);

            var notExistedCategory = game.Categories.FirstOrDefault(cg => categories.All(c => c != cg));
            if (notExistedCategory != null)
            {
                throw new NotExistedCategoryException(notExistedCategory);
            }
            var invalidCategoryPlatform = (await _categoryRepository.GetAllAsync(cancellationToken))
                .Where(c => game.Categories.Contains(c.Id))
                .FirstOrDefault(c => c.PlatformTypes.All(p => game.PlatformTypes.All(pt => pt != p)));
            if (invalidCategoryPlatform != null)
            {
                throw new InvalidCategoryPlatformException(invalidCategoryPlatform.Id);
            }

            var notExistedTag = game.Tags.FirstOrDefault(tg => tags.All(t => t != tg));
            if (notExistedTag != null) throw new NotExistedTagException(notExistedTag);
        }
    }
}
