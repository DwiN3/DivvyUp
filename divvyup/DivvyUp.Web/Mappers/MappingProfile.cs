using AutoMapper;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Receipt, ReceiptDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.totalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.Settled))
                ;

            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.receiptsCount, opt => opt.MapFrom(src => src.ReceiptsCount))
                .ForMember(dest => dest.productsCount, opt => opt.MapFrom(src => src.ProductsCount))
                .ForMember(dest => dest.totalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.unpaidAmount, opt => opt.MapFrom(src => src.UnpaidAmount))
                .ForMember(dest => dest.loanBalance, opt => opt.MapFrom(src => src.LoanBalance))
                .ForMember(dest => dest.userAccount, opt => opt.MapFrom(src => src.UserAccount))
                .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.Name + " " + src.Surname))
                ;

            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.personId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.lent, opt => opt.MapFrom(src => src.Lent))
                .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.person, opt => opt.MapFrom(src => src.Person))
                //.ForMember(dest => dest.person, opt => opt.Ignore())
                ;

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.receiptId, opt => opt.MapFrom(src => src.ReceiptId))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.divisible, opt => opt.MapFrom(src => src.Divisible))
                .ForMember(dest => dest.maxQuantity, opt => opt.MapFrom(src => src.MaxQuantity))
                .ForMember(dest => dest.compensationPrice, opt => opt.MapFrom(src => src.CompensationPrice))
                .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.persons, opt => opt.Ignore())
                ;

            CreateMap<PersonProduct, PersonProductDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.personId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.partOfPrice, opt => opt.MapFrom(src => src.PartOfPrice))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.compensation, opt => opt.MapFrom(src => src.Compensation))
                .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.Settled))
                .ForMember(dest => dest.person, opt => opt.MapFrom(src => src.Person))
                ;
            ;
        }
    }
}
