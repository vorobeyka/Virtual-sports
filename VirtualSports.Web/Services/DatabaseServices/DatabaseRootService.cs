using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Contexts;
using VirtualSports.BE.Models;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Services.DatabaseServices
{
    public class DatabaseRootService : IDatabaseRootService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseRootService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddCategoriesAsync(List<Category> categories, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(categories, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken)
        {
            await _dbContext.Games.AddAsync(game, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddGamesAsync(List<Game> games, CancellationToken cancellationToken)
        {
            await _dbContext.Games.AddRangeAsync(games, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddProviderAsync(List<Provider> providers, CancellationToken cancellationToken)
        {
            await _dbContext.Providers.AddRangeAsync(providers, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddTagsAsync(List<Tag> tags, CancellationToken cancellationToken)
        {
            await _dbContext.Tags.AddRangeAsync(tags, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task AddRootAsync(Root root, CancellationToken cancellationToken)
        {
            await _dbContext.Providers.AddRangeAsync(root.Providers, cancellationToken);
            await _dbContext.Categories.AddRangeAsync(root.Categories, cancellationToken);
            await _dbContext.Tags.AddRangeAsync(root.Tags, cancellationToken);
            await _dbContext.Games.AddRangeAsync(root.Games, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Root> GetRootAsync(CancellationToken cancellationToken)
        {
            var root = new Root
            {
                Games = await _dbContext.Games.ToListAsync(cancellationToken),
                Categories = await _dbContext.Categories.ToListAsync(cancellationToken),
                Providers = await _dbContext.Providers.ToListAsync(cancellationToken),
                Tags = await _dbContext.Tags.ToListAsync(cancellationToken)
            };
            return root;
        }

        public async Task<Provider> GetProviderAsync(string id, CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return provider;
        }

        public async Task<Category> GetCategoryAsync(string id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return category;
        }

        public async Task<Game> GetGameAsync(string id, CancellationToken cancellationToken)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);
            return game;
        }

        public Task<List<Game>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
