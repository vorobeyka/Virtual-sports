using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.DAL.Entities;
using VirtualSports.Lib.Models;

namespace VirtualSports.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddGameToFavouriteAsync(string login, Game game, CancellationToken cancellationToken);
        Task AddGameToRecentAsync(string login, string platform, Game game, CancellationToken cancellationToken);
        Task AddBetAsync(string login, Bet bet, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetFavouritesAsync(string login, string platform, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetRecentAsync(string login, string platform, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetRecommendedAsync(string login, string platform, CancellationToken cancellationToken);
        Task<IEnumerable<Bet>> GetBetsStoryAsync(string login, CancellationToken cancellationToken);
        Task DeleteFromFavouriteAsync(string login, Game game, string platform, CancellationToken cancellationToken);
    }
}
