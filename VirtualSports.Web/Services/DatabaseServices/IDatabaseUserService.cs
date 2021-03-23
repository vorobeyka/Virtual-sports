using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public interface IDatabaseUserService
    {
        Task<bool> TryAddFavouriteAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<bool> TryAddRecentAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<bool> TryAddFavouriteMobileAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<bool> TryAddRecentMobileAsync(string userLogin, Guid gameId, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetRecentAsync(string userLogin, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetRecentMobileAsync(string userLogin, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetFavouritesAsync(string userLogin, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetFavouritesMobileAsync(string userLogin, CancellationToken cancellationToken);
        Task AddBetAsync(string userLogin, Bet bet, CancellationToken cancellationToken);
        Task AddBetMobileAsync(string userLogin, Bet bet, CancellationToken cancellationToken);
        Task<IEnumerable<Bet>> GetBetsStoryAsync(string userLogin, CancellationToken cancellationToken);
        Task<IEnumerable<Bet>> GetBetsStoryMobileAsync(string userLogin, CancellationToken cancellationToken);
    }
}
