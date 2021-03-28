using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task<RootDTO> GetRootAsync(string platformType, CancellationToken cancellationToken);
        public Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken);
    }
}
