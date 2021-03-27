using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetAsync<U>(U id, CancellationToken cancellationToken);
        Task AddAsync(T item, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken);
        Task UpdateAsync(T item, CancellationToken cancellationToken);
        Task DeleteAsync(T item, CancellationToken cancellationToken);
    }
}
