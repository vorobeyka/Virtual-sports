using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.DAL.Models;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseUserService
    {
        Task<bool> TryAddFavouriteAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
        Task<bool> TryAddRecentAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
        Task AddBetAsync(string login, Bet bet, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetRecentAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetFavouritesAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetRecommendedAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<Bet>> GetBetsStoryAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<bool> DeleteFavouriteAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
    }
}
