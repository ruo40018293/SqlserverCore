using AutoMapper;
using Common.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Model.Entity;

namespace ModelDto
{
    public class SystemProfile : Profile, IProfile
    {
        public SystemProfile()
        {
            CreateMap<SystemDto, t_system>()
                .ForMember(sys=> sys.SystemName,opt=> opt.MapFrom(dto=> dto.Name));

            CreateMap<t_system, SystemDto>()
                .ForMember(dto=> dto.Name,opt=>opt.MapFrom(sys=>sys.SystemName));
        }
    }
}
