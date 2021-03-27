using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task<RootDTO> GetRootAsync(string platformType, CancellationToken cancellationToken);
        /*Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken);*/

        public Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken);

        //Task<IEnumerable<GameDTO>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
