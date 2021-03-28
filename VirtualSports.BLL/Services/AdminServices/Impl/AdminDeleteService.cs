using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminDeleteService : IAdminDeleteService
    {
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;

        public AdminDeleteService(
            IRepository<Game> gameRepository,
            IRepository<Provider> providerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagProvider)
        {
            _gameRepository = gameRepository;
            _providerRepository = providerRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagProvider;
        }

        public async Task DeleteCategory(string id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(id, cancellationToken);
            await _categoryRepository.DeleteAsync(category, cancellationToken);
        }

        public async Task DeleteGame(string id, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetAsync(id, cancellationToken);
            await _gameRepository.DeleteAsync(game, cancellationToken);
        }

        public async Task DeleteProvider(string id, CancellationToken cancellationToken)
        {
            var provider = await _providerRepository.GetAsync(id, cancellationToken);
            await _providerRepository.DeleteAsync(provider, cancellationToken);
        }

        public async Task DeleteTag(string id, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetAsync(id, cancellationToken);
            await _tagRepository.DeleteAsync(tag, cancellationToken);
        }
    }
}
