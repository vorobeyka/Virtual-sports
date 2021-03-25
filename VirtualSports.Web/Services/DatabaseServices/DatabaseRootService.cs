using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public class DatabaseRootService : IDatabaseRootService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseRootService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<IEnumerable<Game>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken)
        {
            var games = await _dbContext.Games
                .Where(game => ids.Any(id => id == game.Id))
                .ToListAsync(cancellationToken);
            return games;
        }
    }
}
