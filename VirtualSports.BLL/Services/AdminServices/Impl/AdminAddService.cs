using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Exceptions;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminAddService : IAdminAddService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;

        public AdminAddService(
            IMapper mapper,
            IRepository<Game> gameRepository,
            IRepository<Provider> providerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagProvider,
            ILogger<AdminAddService> logger)
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _providerRepository = providerRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagProvider;
        }

        public async Task AddGames(IEnumerable<GameDTO> gamesDTO, CancellationToken cancellationToken)
        {
            await CheckGamesProperties(gamesDTO, cancellationToken);

            var games = _mapper.Map<IEnumerable<Game>>(gamesDTO);
            await _gameRepository.AddRangeAsync(games, cancellationToken);
        }

        public async Task AddCategories(IEnumerable<CategoryDTO> categoriesDTO, CancellationToken cancellationToken)
        {
            var categories = _mapper.Map<IEnumerable<Category>>(categoriesDTO);
            await _categoryRepository.AddRangeAsync(categories, cancellationToken);
        }

        public async Task AddProviders(IEnumerable<ProviderDTO> providersDTO, CancellationToken cancellationToken)
        {
            var providers = _mapper.Map<IEnumerable<Provider>>(providersDTO);
            await _providerRepository.AddRangeAsync(providers, cancellationToken);
        }

        public async Task AddTags(IEnumerable<TagDTO> tagsDTO, CancellationToken cancellationToken)
        {
            var tags = _mapper.Map<IEnumerable<Tag>>(tagsDTO);
            await _tagRepository.AddRangeAsync(tags, cancellationToken);
        }

        private async Task CheckGamesProperties(IEnumerable<GameDTO> gamesDTO, CancellationToken cancellationToken)
        {
            var categories = (await _categoryRepository.GetAllAsync(cancellationToken))
                .Select(c => c.Id);
            var providers = (await _providerRepository.GetAllAsync(cancellationToken))
                .Select(p => p.Id);
            var tags = (await _tagRepository.GetAllAsync(cancellationToken))
                .Select(t => t.Id);

            foreach (var game in gamesDTO)
            {
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
}
