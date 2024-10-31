using AutoMapper;
using DivvyUp.Web.Models;
using DivvyUp_Shared.Dto;

namespace DivvyUp.Web.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();
        }
    }
}
