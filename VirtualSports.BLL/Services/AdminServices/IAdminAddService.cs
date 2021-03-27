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
        Task AddGames(IEnumerable<GameDTO> gamesDTO, CancellationToken cancellationToken);
        Task AddProviders(IEnumerable<ProviderDTO> providersDTO, CancellationToken cancellationToken);
        Task AddCategories(IEnumerable<CategoryDTO> categoriesDTO, CancellationToken cancellationToken);
        Task AddTags(IEnumerable<TagDTO> tagsDTO, CancellationToken cancellationToken);
    }
}
