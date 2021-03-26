using AutoMapper;
using VirtualSports.BLL.DTO;
using VirtualSports.DAL.Entities;
using VirtualSports.Web.Contracts.AdminContracts;
using VirtualSports.Web.Contracts.ViewModels;

namespace VirtualSports.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryRequest, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, CategoryView>();

            CreateMap<GameRequest, GameDTO>();
            CreateMap<GameDTO, Game>();
            CreateMap<Game, GameDTO>();
            CreateMap<GameDTO, GameView>();

            CreateMap<ProviderRequest, ProviderDTO>();
            CreateMap<ProviderDTO, Provider>();
            CreateMap<Provider, ProviderDTO>();
            CreateMap<ProviderDTO, ProviderView>();

            CreateMap<TagRequest, TagDTO>();
            CreateMap<TagDTO, Tag>();
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, TagView>();
        }
    }
}
