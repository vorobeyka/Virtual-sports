using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Repositories.Interfaces;
using VirtualSports.Lib.Models;

namespace VirtualSports.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DatabaseManagerContext _dbContext;

        public UserRepository(DatabaseManagerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBetAsync(string login, Bet bet, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            user.Bets.Add(bet);
            await UpdateAsync(user, cancellationToken);
        }

        public async Task AddGameToFavouriteAsync(string login, Game game, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            user.FavouriteGames.Add(game);
            await UpdateAsync(user, cancellationToken);
        }

        public async Task AddGameToRecentAsync(string login, string platform, Game game, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            user.RecentGames[platform].Add(game);
            await UpdateAsync(user, cancellationToken);
        }

        public async Task DeleteFromFavouriteAsync(string login, Game game, string platform, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            user.FavouriteGames.Remove(game);
            await UpdateAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<Bet>> GetBetsStoryAsync(string login, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            return user.Bets;
        }

        public async Task<IEnumerable<Game>> GetFavouritesAsync(string login, string platform, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            var favouriteGames = user.FavouriteGames.Where(game => game.PlatformTypes.Contains(platform));
            return favouriteGames;
        }

        public async Task<IEnumerable<Game>> GetRecentAsync(string login, string platform, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            var recentGames = user.RecentGames[platform];
            return recentGames;
        }
        
        public async Task<IEnumerable<Game>> GetRecommendedAsync(string login, string platform, CancellationToken cancellationToken)
        {
            var user = await GetAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = user.RecentGames[platform];
            var recommendedGames = games.Where(game =>
                game.PlatformTypes.Contains(platform)
                && recentGames.Any(recent =>
                recent.Categories.Any(category =>
                game.Categories.Contains(category))));
            
            return recommendedGames;
        }
    }
}
