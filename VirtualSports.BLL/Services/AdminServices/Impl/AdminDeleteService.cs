using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminDeleteService : IAdminDeleteService
    {
        public Task DeleteCategory(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGame(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProvider(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTag(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
