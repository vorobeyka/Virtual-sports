using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseManagerContext DbContext;

        public Repository(DatabaseManagerContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken)
        {
            await DbContext.AddAsync(item, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken)
        {
            await DbContext.AddRangeAsync(items, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T item, CancellationToken cancellationToken)
        {
            DbContext.Remove(item);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync<U>(U id, CancellationToken cancellationToken)
        {
            object[] key = new object[1] { id };
            return await DbContext.FindAsync<T>(key, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(T item, CancellationToken cancellationToken)
        {
            DbContext.Update(item);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
