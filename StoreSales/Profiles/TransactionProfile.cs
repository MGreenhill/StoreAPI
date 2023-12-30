using AutoMapper;

namespace StoreSales.API.Profiles
{
    public class TransactionProfile: Profile
    {
        public TransactionProfile()
        {
            CreateMap<Entities.Transaction, Models.TransactionWithOrdersDto>();
            CreateMap<Entities.Transaction, Models.TransactionDto>();
            CreateMap<Models.TransactionCreateDto, Entities.Transaction>();
        }
    }
}
