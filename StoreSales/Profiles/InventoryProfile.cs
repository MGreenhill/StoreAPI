using AutoMapper;

namespace StoreSales.API.Profiles
{
    public class InventoryProfile: Profile
    {
        public InventoryProfile()
        {
            CreateMap<Entities.Inventory, Models.InventoryDto>();
        }
    }
}
