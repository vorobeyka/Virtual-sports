using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class DatabaseRootService : IDatabaseRootService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseRootService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Root> GetRootAsync(PlatformType platformType, CancellationToken cancellationToken)
        {
            var games = (await _dbContext.Games.ToListAsync(cancellationToken))
                .Where(game => game.PlatformTypes.Contains(platformType)).ToList();
            var categories = (await _dbContext.Categories.ToListAsync(cancellationToken))
                .Where(category => category.PlatformTypes.Contains(platformType)).ToList();
            var providers = (await _dbContext.Providers.ToListAsync(cancellationToken))
                .Where(provider => provider.PlatformTypes.Contains(platformType)).ToList();

            var root = new Root
            {
                Games = games,
                Categories = categories,
                Providers = providers,
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

        public async Task<IEnumerable<Game>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken)
        {
            var games = await _dbContext.Games
                .Where(game => ids.Any(id => id == game.Id))
                .ToListAsync(cancellationToken);
            return games;
        }
    }
}
