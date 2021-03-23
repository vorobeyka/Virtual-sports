using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models.DatabaseModels;
using System.Linq;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseUserService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBetAsync(string userLogin, Bet bet, CancellationToken cancellationToken)
        {
            if (bet == null) throw new ArgumentNullException(nameof(bet));
            var user = await GetUserAsync(userLogin, cancellationToken);

            user.Bets.Add(bet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddBetMobileAsync(string userLogin, Bet bet, CancellationToken cancellationToken)
        {
            if (bet == null) throw new ArgumentNullException(nameof(bet));
            var user = await GetUserAsync(userLogin, cancellationToken);

            user.MobileBets.Add(bet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> TryAddFavouriteAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            if (!await IsValidGameId(gameId, cancellationToken)) return false;

            user.FavouriteGameIds.Add(gameId.ToString());
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TryAddFavouriteMobileAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            if (!await IsValidGameId(gameId, cancellationToken)) return false;

            user.FavouriteGameMobileIds.Add(gameId.ToString());
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

       

        public async Task<bool> TryAddRecentAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            if (!await IsValidGameId(gameId, cancellationToken)) return false;

            var recentGames = user.RecentGameIds;
            AddRecent(recentGames, gameId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TryAddRecentMobileAsync(string userLogin, Guid gameId, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            if (!await IsValidGameId(gameId, cancellationToken)) return false;

            var recentGames = user.RecentMobileGameIds;
            AddRecent(recentGames, gameId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Game>> GetRecentAsync(string userLogin, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = games.Where(game => user.RecentGameIds.Any(id => id == game.Id));
            return recentGames;
        }

        public async Task<IEnumerable<Game>> GetRecentMobileAsync(string userLogin, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = games.Where(game => user.RecentMobileGameIds.Any(id => id == game.Id));
            return recentGames;
        }

        public async Task<IEnumerable<Game>> GetFavouritesAsync(string userLogin, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var favouriteGames = games.Where(game => user.FavouriteGameIds.Any(id => id == game.Id));
            return favouriteGames;
        }

        public async Task<IEnumerable<Game>> GetFavouritesMobileAsync(string userLogin, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userLogin, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var favouriteGames = games.Where(game => user.FavouriteGameMobileIds.Any(id => id == game.Id));
            return favouriteGames;
        }

        private async Task<User> GetUserAsync(string userLogin, CancellationToken cancellationToken)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == userLogin, cancellationToken);
            if (user == null) throw new NullReferenceException(nameof(user));
            return user;
        }

        private async Task<bool> IsValidGameId(Guid gameId, CancellationToken cancellationToken)
        {
            var isValid = await _dbContext.Games.AnyAsync(
                game => game.Id == gameId.ToString(),
                cancellationToken);
            return isValid;
        }
        private static void AddRecent(Queue<string> recentGames, Guid gameId)
        {
            if (recentGames.Count >= 4)
            {
                recentGames.Dequeue();
            }
            recentGames.Enqueue(gameId.ToString());
        }
    }
}
