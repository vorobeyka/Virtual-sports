using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services.DatabaseServices
{
    public interface IDatabaseAdminService
    {
        Task AddRootAsync(Root root, CancellationToken cancellationToken);
        Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<bool> AddGamesAsync(List<Game> game, CancellationToken cancellationToken);
        Task<bool> AddCategoriesAsync(List<Category> categories, CancellationToken cancellationToken);
        Task<bool> AddTagsAsync(List<Tag> tags, CancellationToken cancellationToken);
        Task<bool> AddProviderAsync(List<Provider> providers, CancellationToken cancellationToken);
        Task<bool> DeleteGameAsync(string id, CancellationToken cancellationToken);
        Task<bool> DeleteCategoryAsync(string id, CancellationToken cancellationToken);
        Task<bool> DeleteProviderAsync(string id, CancellationToken cancellationToken);
        Task<bool> DeleteTagAsync(string id, CancellationToken cancellationToken);
    }
}
