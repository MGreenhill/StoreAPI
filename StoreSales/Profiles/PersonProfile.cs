using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace StoreSales.API.Profiles
{
    public class PersonProfile: Profile
    {
        public PersonProfile() {
            CreateMap<Entities.Person, Models.PersonDto>();
            CreateMap<Entities.Person, Models.PersonWithoutTransactionsDto>();
            CreateMap<Models.PersonCreateDto, Entities.Person>();
            CreateMap<Models.PersonUpdateDto, Entities.Person>();
            CreateMap<JsonPatchDocument<Models.PersonUpdateDto>, JsonPatchDocument>();
        }
    }
}
