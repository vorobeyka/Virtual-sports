using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
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

        public async Task AddAsync<TEntity>(TEntity item, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken cancellationToken)
        {
            foreach (var i in items)
            {
                await _dbContext.AddAsync(i, cancellationToken);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync<T>(T item, CancellationToken cancellationToken)
        {
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync<TEntity>(string id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.FindAsync(typeof(TEntity), id);
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
