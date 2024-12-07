using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class LoanToLoanDtoProfile : Profile
    {
        public LoanToLoanDtoProfile()
        {
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Lent, opt => opt.MapFrom(src => src.Lent))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                //.ForMember(dest => dest.person, opt => opt.Ignore())
                ;
        }
    }
}