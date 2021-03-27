using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Services.AdminServices
{
    public interface IAdminDeleteService
    {
        Task DeleteGame(string id, CancellationToken cancellationToken);
        Task DeleteProvider(string id, CancellationToken cancellationToken);
        Task DeleteCategory(string id, CancellationToken cancellationToken);
        Task DeleteTag(string id, CancellationToken cancellationToken);
    }
}
