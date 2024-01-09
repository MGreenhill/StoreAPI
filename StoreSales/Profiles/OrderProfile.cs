using AutoMapper;

namespace StoreSales.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Entities.Order, Models.OrderDto>();
            CreateMap<Models.OrderCreateDto, Entities.Order>();
            CreateMap<Models.OrderUpdateDto, Entities.Order>();
            CreateMap<Models.OrderPutDto, Entities.Order>();
        }
    }
}
