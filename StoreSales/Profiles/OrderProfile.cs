using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using StoreSales.API.Models;

namespace StoreSales.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Entities.Order, Models.OrderDto>();
            CreateMap<Models.OrderDto, Entities.Order>();
            CreateMap<Models.OrderCreateDto, Entities.Order>();
            CreateMap<Models.OrderUpdateDto, Entities.Order>();
            CreateMap<Models.OrderPutDto, Entities.Order>();
            CreateMap<JsonPatchDocument<OrderUpdateDto>, JsonPatchDocument>();
        }
    }
}
