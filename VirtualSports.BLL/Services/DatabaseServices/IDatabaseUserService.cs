using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.Lib.Models;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    public interface IDatabaseUserService
    {
        Task AddFavouriteAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
        Task AddRecentAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
        Task AddBetAsync(string login, Bet bet, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetRecentAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetFavouritesAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetRecommendedAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task<IEnumerable<Bet>> GetBetsStoryAsync(string login, PlatformType platformType, CancellationToken cancellationToken);
        Task DeleteFavouriteAsync(string login, string gameId, PlatformType platformType, CancellationToken cancellationToken);
    }
}
