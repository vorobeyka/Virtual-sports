using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Mappings;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.DAL.Entities;
using VirtualSports.DAL.Models;
using VirtualSports.Lib.Mappings;
using VirtualSports.Lib.Models;

namespace VirtualSports.BLL.Services.AdminServices.Impl
{
    public class AdminAddService : IAdminAddService
    {
        private readonly IDatabaseAdminService _databaseAdminService;
        private readonly IMapper _mapper;

        public AdminAddService(IDatabaseAdminService databaseAdminService, IMapper mapper)
        {
            _databaseAdminService = databaseAdminService;
            _mapper = mapper;
        }

        public async Task AddCategories(
            IEnumerable<CategoryDTO> categoryDTOs,
            CancellationToken cancellationToken)
        {
            var categoriesPlatformTypes = categoryDTOs
                .Select(category =>
                    MapMethods.MapPlatformTypes(category.PlatformTypes).ToList()).ToArray();
            CheckInvalidPlatforms(categoriesPlatformTypes);

            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categoryDTOs);
            var categories = _mapper.Map<List<Category>>(categoryDTOs);

            for (int i = 0; i < categories.Count(); i++)
            {
                categories[i].PlatformTypes = categoriesPlatformTypes[i];
            }

            await _databaseAdminService.AddRangeAsync<Category>(categories, cancellationToken);
        }

        public async Task AddGames(IEnumerable<GameRequest> gameRequests, CancellationToken cancellationToken)
        {
            var gamesPlatformTypes = gameRequests
                .Select(game =>
                    MapMethods.MapPlatformTypes(game.PlatformTypes)).ToArray();
            CheckInvalidPlatforms(gamesPlatformTypes);

            var gameDTOs = _mapper.Map<IEnumerable<GameDTO>>(gameRequests);
            var games = _mapper.Map<IEnumerable<Game>>(gameDTOs);
            var platforms = games.Select(game => game.PlatformTypes).ToArray();
            SetPlatforms(gamesPlatformTypes, platforms);

            await _databaseAdminService.AddRangeAsync<Game>(games, cancellationToken);
        }

        public async Task AddProviders(IEnumerable<ProviderRequest> providerRequests, CancellationToken cancellationToken)
        {
            var providersPlatformTypes = providerRequests
                .Select(provider =>
                    MapMethods.MapPlatformTypes(provider.PlatformTypes)).ToArray();
            CheckInvalidPlatforms(providersPlatformTypes);

            var providerDTOs = _mapper.Map<IEnumerable<ProviderDTO>>(providerRequests);
            var providers = _mapper.Map<IEnumerable<Provider>>(providerDTOs);
            var platforms = providers.Select(game => game.PlatformTypes).ToArray();
            SetPlatforms(providersPlatformTypes, platforms);

            await _databaseAdminService.AddRangeAsync<Provider>(providers, cancellationToken);
        }

        public async Task AddTags(IEnumerable<TagRequest> tagRequests, CancellationToken cancellationToken)
        {
            var tagsDTOs = _mapper.Map<IEnumerable<TagDTO>>(tagRequests);
            var tags = _mapper.Map<IEnumerable<Tag>>(tagsDTOs);

            await _databaseAdminService.AddRangeAsync<Tag>(tags, cancellationToken);
        }

        private static void CheckInvalidPlatforms(IEnumerable<IEnumerable<PlatformType>> entitiesPlatformTypes)
        {
            var isAnyInvalidPlatforms = entitiesPlatformTypes
                .Any(types =>
                    types.Any(type => type == PlatformType.UnknownPlatform));

            if (isAnyInvalidPlatforms)
            {
                // TODO: Change to custom invalid Admin exception
                throw new Exception("Invalid platform type.");
            }
        }

        private static void SetPlatforms(IEnumerable<PlatformType>[] source, IEnumerable<PlatformType>[] dest)
        {
            if (source.Length != dest.Length)
            {
                throw new Exception("Unexpected exception.");
            }
            for (int i = 0; i < source.Length; i++)
            {
                dest[i] = source[i];
            }
        }
    }
}
