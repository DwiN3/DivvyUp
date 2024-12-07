using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class UserToUserDtoProfile : Profile
    {
        public UserToUserDtoProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        }
    }
}