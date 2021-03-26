using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Entities;
using VirtualSports.Lib.Models;
using VirtualSports.BLL.DTO;
using AutoMapper;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly DatabaseManagerContext _dbContext;
        private readonly IMapper _mapper;

        public DatabaseUserService(DatabaseManagerContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddFavouriteAsync(
            string login,
            string gameId,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            user.FavouriteGameIds.Add(gameId);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRecentAsync(
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

        public async Task<IEnumerable<GameDTO>> GetRecentAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var recentGames = games.Where(game => user.RecentGameIds[platformType].Any(id => id == game.Id));
            var recentGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(recentGames);
            return recentGamesDTO;
        }

        public async Task<IEnumerable<GameDTO>> GetFavouritesAsync(
            string login,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var games = await _dbContext.Games.ToListAsync(cancellationToken);
            var favouriteGames = games.Where(game => user.FavouriteGameIds.Any(id => id == game.Id));
            var favouritePlatformGames = favouriteGames.Where(game => game.PlatformTypes.Contains(platformType));
            var favouritePlatformGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(favouritePlatformGames);
            return favouritePlatformGamesDTO;
        }

        public async Task<IEnumerable<GameDTO>> GetRecommendedAsync(
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
            var recommendedGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(recommendedGames);
            return recommendedGamesDTO;
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

        public async Task DeleteFavouriteAsync(
            string login,
            string gameId,
            PlatformType platformType,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(login, cancellationToken);
            var favouriteGameId = user.FavouriteGameIds.FirstOrDefault(id => id == gameId);

            user.FavouriteGameIds.Remove(favouriteGameId);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
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
