using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.DTO;
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
        private readonly IRepository<Tag> _tagProvider;

        public AdminAddService(
            IMapper mapper,
            IRepository<Game> gameRepository,
            IRepository<Provider> providerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagProvider)
        {
            _mapper = mapper;
            _gameRepository = gameRepository;
            _providerRepository = providerRepository;
            _categoryRepository = categoryRepository;
            _tagProvider = tagProvider;
        }

        public async Task AddGames(IEnumerable<GameDTO> gamesDTO, CancellationToken cancellationToken)
        {
            /*var categoriesInGames = gamesDTO.Select(g => g.Categories);
            var providersInGames = gamesDTO.Select(g => g.Provider);
            var tagsInGames = gamesDTO.Select(g => g.Tags);
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            var providers = await _providerRepository.GetAllAsync(cancellationToken);
            var tags = await _tagProvider.GetAllAsync(cancellationToken);*/

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
            await _tagProvider.AddRangeAsync(tags, cancellationToken);
        }
    }
}
