using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public class DatabaseAdminService : IDatabaseAdminService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseAdminService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRootAsync(Root root, CancellationToken cancellationToken)
        {
            await _dbContext.Providers.AddRangeAsync(root.Providers, cancellationToken);
            await _dbContext.Categories.AddRangeAsync(root.Categories, cancellationToken);
            await _dbContext.Tags.AddRangeAsync(root.Tags, cancellationToken);
            await _dbContext.Games.AddRangeAsync(root.Games, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AddCategoriesAsync(List<Category> categories, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Categories.AddRangeAsync(categories, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception) { return false; }
            return true;
        }

        public async Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Games.AddAsync(game, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception) { return false; }
            return true;
        }

        public async Task<bool> AddGamesAsync(List<Game> games, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Games.AddRangeAsync(games, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            { return false; }
            return true;
        }

        public async Task<bool> AddProviderAsync(List<Provider> providers, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Providers.AddRangeAsync(providers, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception) { return false; }
            return true;
        }

        public async Task<bool> AddTagsAsync(List<Tag> tags, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Tags.AddRangeAsync(tags, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception) { return false; }
            return true;
        }

        public async Task<bool> DeleteGameAsync(string id, CancellationToken cancellationToken)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (game == null) return false;

            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (category == null) return false;

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteProviderAsync(string id, CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (provider == null) return false;

            _dbContext.Providers.Remove(provider);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteTagAsync(string id, CancellationToken cancellationToken)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (tag == null) return false;

            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
