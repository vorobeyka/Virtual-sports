using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseAdminService
    {
        Task AddRangeAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken);
        Task AddAsync<T>(T item, CancellationToken cancellationToken);
        Task UpdateAsync<T>(T item, CancellationToken cancellationToken);
        Task DeleteAsync<TEntity>(string id, CancellationToken cancellationToken);
    }
}
