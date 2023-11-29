using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using StoreSales.API.Entities;
using StoreSales.API.Models;

namespace StoreSales.API.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemCreateDto, Item>();
            CreateMap<ItemUpdateDto, Item>();
            CreateMap<JsonPatchDocument<ItemUpdateDto>, JsonPatchDocument>();
        }
    }
}
