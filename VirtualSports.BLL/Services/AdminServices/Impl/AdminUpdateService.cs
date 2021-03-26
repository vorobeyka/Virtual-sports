using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminUpdateService : IAdminUpdateService
    {
        public Task UpdateGame(GameDTO gameDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategory(CategoryDTO categoryDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProvider(ProviderDTO providerDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTag(TagDTO tagDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
