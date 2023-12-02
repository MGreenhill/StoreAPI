using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using StoreSales.API.Models;

namespace StoreSales.API.Profiles
{
    public class InventoryProfile: Profile
    {
        public InventoryProfile()
        {
            CreateMap<Entities.Inventory, Models.InventoryDto>();
            CreateMap<Models.InventoryCreateDto, Entities.Inventory>();
            CreateMap<Models.InventoryUpdateDto, Entities.Inventory>();
            CreateMap<JsonPatchDocument<InventoryUpdateDto>, JsonPatchDocument>();
        }
    }
}
