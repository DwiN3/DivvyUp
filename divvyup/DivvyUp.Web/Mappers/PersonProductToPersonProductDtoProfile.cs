using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class PersonProductToPersonProductDtoProfile : Profile
    {
        public PersonProductToPersonProductDtoProfile()
        {
            CreateMap<PersonProduct, PersonProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.PartOfPrice, opt => opt.MapFrom(src => src.PartOfPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Compensation, opt => opt.MapFrom(src => src.Compensation))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                ;
        }
    }
}