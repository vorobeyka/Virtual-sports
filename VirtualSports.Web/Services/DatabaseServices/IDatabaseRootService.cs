using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task<Root> GetRootAsync(CancellationToken cancellationToken);
        Task<Provider> GetProviderAsync(string id, CancellationToken cancellationToken);
        Task<Category> GetCategoryAsync(string id, CancellationToken cancellationToken);
        Task<Game> GetGameAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
