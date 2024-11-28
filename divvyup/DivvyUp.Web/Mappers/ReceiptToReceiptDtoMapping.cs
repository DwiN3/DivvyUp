﻿using AutoMapper;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Mappers
{
    public class ReceiptToReceiptDtoMapping : Profile
    {
        public ReceiptToReceiptDtoMapping()
        {
            CreateMap<Receipt, ReceiptDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Settled, opt => opt.MapFrom(src => src.Settled))
                ;
        }
    }
}
