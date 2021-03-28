using System;
using System.Collections;
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
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class DatabaseUserService : IUserService
    {
        private readonly DatabaseManagerContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IUserRepository _userRepository;

        public DatabaseUserService(
            DatabaseManagerContext dbContext,
            IMapper mapper,
            IRepository<Game> gameRepository,
            IRepository<Provider> providerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagRepository,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public async Task AddFavouriteAsync(
            string login,
            string gameId,
            string platformType,
            CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetAsync(gameId, cancellationToken) ?? throw new NullReferenceException();
            var user = await _userRepository.GetAsync(login, cancellationToken);

            user.FavouriteGames.Add(game);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRecentAsync(
            string login,
            string gameId,
            string platformType,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(login, cancellationToken);
            var recentGames = user.RecentGames[platformType];
            var game = await _gameRepository.GetAsync(gameId, cancellationToken) ?? throw new NullReferenceException();
            var existedGame = recentGames.FirstOrDefault(g => g.Id == gameId);

            if (existedGame != null)
            {
                recentGames.Remove(existedGame);
            }
            else if (recentGames.Count >= 4)
            {
                recentGames.Remove(recentGames.ElementAt(0));
            }
            recentGames.Add(game);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddBetAsync(
            string login,
            Bet bet,
            CancellationToken cancellationToken)
        {
            await _userRepository.AddBetAsync(login, bet, cancellationToken);
        }

        public async Task<IEnumerable<GameDTO>> GetRecentAsync(
            string login,
            string platformType,
            CancellationToken cancellationToken)
        {
            //var user = await GetUserAsync(login, cancellationToken);
            var recentGames = await _userRepository.GetRecentAsync(login, platformType, cancellationToken);
            var recentGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(recentGames);
            return recentGamesDTO.Reverse();
        }

        public async Task<IEnumerable<GameDTO>> GetFavouritesAsync(
            string login,
            string platformType,
            CancellationToken cancellationToken)
        {
            var favouriteGames = await _userRepository.GetFavouritesAsync(login, platformType, cancellationToken);
            var favouritePlatformGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(favouriteGames);
            
            return favouritePlatformGamesDTO;
        }

        public async Task<IEnumerable<GameDTO>> GetRecommendedAsync(
            string login,
            string platformType,
            CancellationToken cancellationToken)
        {
            var recommendedGames = await _userRepository.GetRecommendedAsync(login, platformType, cancellationToken);
            var recommendedGamesDTO = _mapper.Map<IEnumerable<GameDTO>>(recommendedGames);

            return recommendedGamesDTO;
        }

        public async Task<IEnumerable<Bet>> GetBetsStoryAsync(
            string login,
            CancellationToken cancellationToken)
        {
            return await _userRepository.GetBetsStoryAsync(login, cancellationToken);
        }

        public async Task DeleteFavouriteAsync(
            string login,
            string gameId,
            string platformType,
            CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetAsync(login, cancellationToken);
            await _userRepository.DeleteFromFavouriteAsync(login, game, platformType, cancellationToken);
            /*var user = await GetUserAsync(login, cancellationToken);
            user.FavouriteGameIds.Remove(user.FavouriteGameIds.FirstOrDefault(g => g.Id == gameId));
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);*/
        }
/*
        private async Task<User> GetUserAsync(string userLogin, CancellationToken cancellationToken)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == userLogin, cancellationToken);
            if (user == null) throw new NullReferenceException(nameof(user));
            return user;
        }*/
    }
}
