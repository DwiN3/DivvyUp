using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class ProductToProductDtoProfile : Profile
    {
        public ProductToProductDtoProfile()
        {
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
        }
    }
}