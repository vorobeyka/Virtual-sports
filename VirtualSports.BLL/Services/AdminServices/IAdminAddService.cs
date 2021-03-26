using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.AdminServices
{
    public interface IAdminAddService
    {
        Task AddGames(IEnumerable<GameDTO> gameDTOs, CancellationToken cancellationToken);
        Task AddProviders(IEnumerable<ProviderDTO> providerDTOs, CancellationToken cancellationToken);
        Task AddCategories(IEnumerable<CategoryDTO> categoryDTOs, CancellationToken cancellationToken);
        Task AddTags(IEnumerable<TagDTO> tagDTOs, CancellationToken cancellationToken);
    }
}
