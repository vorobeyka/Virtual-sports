using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
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
            if (!user.FavouriteGameIds.Any(id => id == gameId))
            {
                return false;
            }
            user.FavouriteGameIds.Add(gameId);
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
            user.Bets.Add(bet);
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
            var favouriteGames = games.Where(game => user.FavouriteGameIds.Any(id => id == game.Id));
            var favouritePlatformGames = favouriteGames.Where(game => game.PlatformTypes.Contains(platformType));
            return favouritePlatformGames;
        }

        public async Task<IEnumerable<Game>> GetRecommendedAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = games.Where(game => user.RecentGameIds[platformType].Any(id => id == game.Id));
            var recommendedGames = games.Where(game =>
                recentGames.Any(recent => 
                recent.Categories.Any(category => 
                game.Categories.Any(c => c == category))));
            return recommendedGames;
        }


        public async Task<IEnumerable<Bet>> GetBetsStoryAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var bets = user.Bets;
            return bets;
        }

        public async Task<bool> DeleteFavouriteAsync(
            string login,
            string gameId,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var favouriteGameId = user.FavouriteGameIds.FirstOrDefault(id => id == gameId);

            if (favouriteGameId == null) return false;

            user.FavouriteGameIds.Remove(favouriteGameId);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
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
