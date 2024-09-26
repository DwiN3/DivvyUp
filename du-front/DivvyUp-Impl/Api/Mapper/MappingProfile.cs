using DivvyUp.Shared.Dto;
using DivvyUp.Shared.Model;
using AutoMapper;

namespace DivvyUp_Impl.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*
              // USER
               CreateMap<UserDto, UserModel>()
                   .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.userId))
                   .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                   .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password))
                   .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.username))
                   .ForMember(dest => dest.role, opt => opt.Ignore());

               CreateMap<UserModel, UserDto>()
                   .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.userId))
                   .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                   .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password))
                   .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.username));

               // RECEIPT
               CreateMap<ReceiptDto, ReceiptModel>()
                   .ForMember(dest => dest.userId, opt => opt.Ignore())
                   .ForMember(dest => dest.receiptId, opt => opt.MapFrom(src => src.receiptId))
                   .ForMember(dest => dest.receiptName, opt => opt.MapFrom(src => src.receiptName))
                   .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.date))
                   .ForMember(dest => dest.totalAmount, opt => opt.MapFrom(src => src.totalAmount))
                   .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.settled));

               CreateMap<ReceiptModel, ReceiptDto>()
                   .ForMember(dest => dest.receiptId, opt => opt.MapFrom(src => src.receiptId))
                   .ForMember(dest => dest.receiptName, opt => opt.MapFrom(src => src.receiptName))
                   .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.userId))
                   .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.date))
                   .ForMember(dest => dest.totalAmount, opt => opt.MapFrom(src => src.totalAmount))
                   .ForMember(dest => dest.settled, opt => opt.MapFrom(src => src.settled));
             */
        }
    }
}
