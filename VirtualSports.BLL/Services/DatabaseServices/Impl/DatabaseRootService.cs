using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.DAL.Contexts;
using VirtualSports.Lib.Models;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    public class DatabaseRootService : IDatabaseRootService
    {
        private readonly DatabaseManagerContext _dbContext;
        private readonly IMapper _mapper;

        public DatabaseRootService(DatabaseManagerContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RootDTO> GetRootAsync(string platformType, CancellationToken cancellationToken)
        {
            var games = (await _dbContext.Games.ToListAsync(cancellationToken))
                .Where(game => game.PlatformTypes.Contains(platformType)).ToList();
            var categories = (await _dbContext.Categories.ToListAsync(cancellationToken))
                .Where(category => category.PlatformTypes.Contains(platformType)).ToList();
            var providers = (await _dbContext.Providers.ToListAsync(cancellationToken))
                .Where(provider => provider.PlatformTypes.Contains(platformType)).ToList();
            var tags = (await _dbContext.Tags.ToListAsync(cancellationToken)).ToList();

            var root = new RootDTO
            {
                Games = _mapper.Map<List<GameDTO>>(games),
                Categories = _mapper.Map<List<CategoryDTO>>(categories),
                Providers = _mapper.Map<List<ProviderDTO>>(providers),
                Tags = _mapper.Map<List<TagDTO>>(tags)
            };
            return root;
        }

        /*public async Task<ProviderDTO> GetProviderAsync(string id, CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return provider;
        }


        public async Task<CategoryDTO> GetCategoryAsync(string id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return category;
        }
        public async Task<IEnumerable<GameDTO>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken)
        {
            var games = await _dbContext.Games
                .Where(game => ids.Any(id => id == game.Id))
                .ToListAsync(cancellationToken);
            var gamesDTO = _mapper.Map<IEnumerable<GameDTO>>(games);
            return gamesDTO;
        }

        */

        public async Task<GameDTO> GetGameAsync(string id, CancellationToken cancellationToken)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            return _mapper.Map<GameDTO>(game);
        }
    }
}
