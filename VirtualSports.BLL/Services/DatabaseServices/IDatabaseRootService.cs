using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task<RootDTO> GetRootAsync(PlatformType platformType, CancellationToken cancellationToken);
        //Task<Provider> GetProviderAsync(string id, CancellationToken cancellationToken);
        //Task<Category> GetCategoryAsync(string id, CancellationToken cancellationToken);
        Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
