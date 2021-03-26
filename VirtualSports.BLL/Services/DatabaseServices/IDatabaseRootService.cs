using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.DAL.Models;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task<RootDTO> GetRootAsync(PlatformType platformType, CancellationToken cancellationToken);
        Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
