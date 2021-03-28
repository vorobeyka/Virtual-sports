using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VirtualSports.DAL.Entities;
using VirtualSports.Lib.Models;
using VirtualSports.BLL.DTO;
using AutoMapper;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IUserRepository _userRepository;

        public UserService(
            IMapper mapper,
            IRepository<Game> gameRepository,
            IUserRepository userRepository)
        {
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
            await _userRepository.AddGameToFavouriteAsync(login, game, cancellationToken);
        }

        public async Task AddRecentAsync(
            string login,
            string gameId,
            string platformType,
            CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetAsync(gameId, cancellationToken) ?? throw new NullReferenceException();
            await _userRepository.AddGameToRecentAsync(login, platformType, game, cancellationToken);
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
            CancellationToken cancellationToken)
        {
            await _userRepository.DeleteFromFavouriteAsync(login, gameId, cancellationToken);
        }
    }
}
