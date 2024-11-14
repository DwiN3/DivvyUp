using AutoMapper;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                ;

            CreateMap<Receipt, ReceiptDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                ;

            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.ReceiptsCount, opt => opt.MapFrom(src => src.ReceiptsCount))
                .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.ProductsCount))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.UnpaidAmount, opt => opt.MapFrom(src => src.UnpaidAmount))
                .ForMember(dest => dest.LoanBalance, opt => opt.MapFrom(src => src.LoanBalance))
                .ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.UserAccount))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.Surname))
                ;

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

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.AdditionalPrice, opt => opt.MapFrom(src => src.AdditionalPrice))
                .ForMember(dest => dest.Divisible, opt => opt.MapFrom(src => src.Divisible))
                .ForMember(dest => dest.MaxQuantity, opt => opt.MapFrom(src => src.MaxQuantity))
                .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.AvailableQuantity))
                .ForMember(dest => dest.CompensationPrice, opt => opt.MapFrom(src => src.CompensationPrice))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.Persons, opt => opt.Ignore())
                ;

            CreateMap<PersonProduct, PersonProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.PartOfPrice, opt => opt.MapFrom(src => src.PartOfPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Compensation, opt => opt.MapFrom(src => src.Compensation))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                ;
            ;
        }
    }
}
