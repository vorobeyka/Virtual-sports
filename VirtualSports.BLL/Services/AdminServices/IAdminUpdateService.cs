using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.AdminServices
{
    public interface IAdminUpdateService
    {
        Task UpdateGame(GameDTO gameDTO, CancellationToken cancellationToken = default);
        Task UpdateProvider(ProviderDTO providerDTO, CancellationToken cancellationToken = default);
        Task UpdateCategory(CategoryDTO categoryDTO, CancellationToken cancellationToken = default);
        Task UpdateTag(TagDTO tagDTO, CancellationToken cancellationToken = default);
    }
}
