using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Contexts;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseUserService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddBetAsync(string userLogin, Bet bet, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetFavourites(string userLogin, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetRecent(string userLogin, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryAddFavouriteAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryAddRecentAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
