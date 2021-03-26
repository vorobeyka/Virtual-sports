﻿using AutoMapper;

namespace VirtualSports.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<CategoryRequest, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            CreateMap<GameRequest, GameDTO>();
            CreateMap<GameDTO, Game>();
            CreateMap<Game, GameDTO>();

            CreateMap<ProviderRequest, ProviderDTO>();
            CreateMap<ProviderDTO, Provider>();
            CreateMap<Provider, ProviderDTO>();

            CreateMap<TagRequest, TagDTO>();
            CreateMap<TagDTO, Tag>();
            CreateMap<Tag, TagDTO>();
        }
    }
}
