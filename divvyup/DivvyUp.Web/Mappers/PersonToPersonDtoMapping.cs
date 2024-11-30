using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class PersonToPersonDtoMapping : Profile
    {
        public PersonToPersonDtoMapping()
        {
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.ReceiptsCount, opt => opt.MapFrom(src => src.ReceiptsCount))
                .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.ProductsCount))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.UnpaidAmount, opt => opt.MapFrom(src => src.UnpaidAmount))
                .ForMember(dest => dest.CompensationAmount, opt => opt.MapFrom(src => src.CompensationAmount))
                .ForMember(dest => dest.LoanBalance, opt => opt.MapFrom(src => src.LoanBalance))
                .ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.UserAccount))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.Surname))
                ;
        }
    }
}