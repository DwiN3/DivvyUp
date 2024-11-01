using AutoMapper;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<Receipt, ReceiptDto>();
        }
    }
}
