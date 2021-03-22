using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Services.DatabaseServices
{
    public interface IDatabaseRootService
    {
        Task AddRootAsync(Root root, CancellationToken cancellationToken);
        Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<bool> AddGamesAsync(List<Game> game, CancellationToken cancellationToken);
        Task<bool> AddCategoriesAsync(List<Category> categories, CancellationToken cancellationToken);
        Task<bool> AddTagsAsync(List<Tag> tags, CancellationToken cancellationToken);
        Task<bool> AddProviderAsync(List<Provider> providers, CancellationToken cancellationToken);
        Task<Root> GetRootAsync(CancellationToken cancellationToken);
        Task<Provider> GetProviderAsync(string id, CancellationToken cancellationToken);
        Task<Category> GetCategoryAsync(string id, CancellationToken cancellationToken);
        Task<Game> GetGameAsync(string id, CancellationToken cancellationToken);
        Task<List<Game>> GetGamesAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
