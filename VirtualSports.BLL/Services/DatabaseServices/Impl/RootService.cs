using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Repositories.Interfaces;
using VirtualSports.Lib.Models;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class RootService : DatabaseServices.RootService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;

        public RootService(
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

        public async Task<RootDTO> GetRootAsync(string platformType, CancellationToken cancellationToken)
        {
            var games = (await _gameRepository.GetAllAsync(cancellationToken))
                .Where(game => game.PlatformTypes.Contains(platformType));
            var providers = (await _providerRepository.GetAllAsync(cancellationToken))
                .Where(provider => provider.PlatformTypes.Contains(platformType));
            var categories = (await _categoryRepository.GetAllAsync(cancellationToken))
                .Where(category => category.PlatformTypes.Contains(platformType));
            var tags = await _tagRepository.GetAllAsync(cancellationToken);

            var root = new RootDTO
            {
                Games = _mapper.Map<List<GameDTO>>(games),
                Categories = _mapper.Map<List<CategoryDTO>>(categories),
                Providers = _mapper.Map<List<ProviderDTO>>(providers),
                Tags = _mapper.Map<List<TagDTO>>(tags)
            };
            return root;
        }

        public async Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<GameDTO>(game);
        }
    }
}
