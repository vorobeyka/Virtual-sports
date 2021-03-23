using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public interface IDatabaseUserService
    {
        Task<bool> TryAddFavouriteAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<bool> TryAddRecentAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<List<string>> GetRecent(string userLogin, CancellationToken cancellationToken);
        Task<List<string>> GetFavourites(string userLogin, CancellationToken cancellationToken);
        Task AddBetAsync(string userLogin, Bet bet, CancellationToken cancellationToken);
    }
}
