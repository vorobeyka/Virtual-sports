using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models.DatabaseModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VirtualSports.Web.Models;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseUserService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> TryAddFavouriteAsync(
            string login,
            string gameId,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            if (!user.FavouriteGameIds[platformType].Any(id => id == gameId))
            {
                return false;
            }
            user.FavouriteGameIds[platformType].Add(gameId);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TryAddRecentAsync(
            string login,
            string gameId,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var recentGames = user.RecentGameIds[platformType];

            if (recentGames.Count >= 4)
            {
                recentGames.Dequeue();
            }
            recentGames.Enqueue(gameId);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task AddBetAsync(
            string login,
            Bet bet,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            user.Bets[platformType].Add(bet);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Game>> GetRecentAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = games.Where(game => user.RecentGameIds[platformType].Any(id => id == game.Id));
            return recentGames;
        }

        public async Task<IEnumerable<Game>> GetFavouritesAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var favouriteGames = games.Where(game => user.FavouriteGameIds[platformType].Any(id => id == game.Id));
            return favouriteGames;
        }

        public async Task<IEnumerable<Bet>> GetBetsStoryAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var bets = user.Bets[platformType];
            return bets;
        }

        private async Task<User> GetUserAsync(string userLogin, CancellationToken cancellationToken)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == userLogin, cancellationToken);
            if (user == null) throw new NullReferenceException(nameof(user));
            return user;
        }
    }
}
