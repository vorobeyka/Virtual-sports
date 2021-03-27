using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
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
        private readonly IRepository<Tag> _tagProvider;

        public AdminUpdateService(
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

        public async Task UpdateGame(GameDTO gameDTO, CancellationToken cancellationToken = default)
        {
            var game = _mapper.Map<Game>(gameDTO);
            await _gameRepository.UpdateAsync(game, cancellationToken);
        }

        public async Task UpdateCategory(CategoryDTO categoryDTO, CancellationToken cancellationToken = default)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.UpdateAsync(category, cancellationToken);
        }

        public async Task UpdateProvider(ProviderDTO providerDTO, CancellationToken cancellationToken = default)
        {
            var provider = _mapper.Map<Provider>(providerDTO);
            await _providerRepository.UpdateAsync(provider, cancellationToken);
        }

        public async Task UpdateTag(TagDTO tagDTO, CancellationToken cancellationToken = default)
        {
            var tag = _mapper.Map<Tag>(tagDTO);
            await _tagProvider.UpdateAsync(tag, cancellationToken);
        }
    }
}
