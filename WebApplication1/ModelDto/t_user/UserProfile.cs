using AutoMapper;
using Common.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Model.Entity;

namespace ModelDto
{
    public class UserProfile : Profile, IProfile
    {
        public UserProfile()
        {
            CreateMap<UserDto, t_user>()
                .ForMember(user => user.UserName, opt => opt.MapFrom(dto => dto.Name)); 

            CreateMap<t_user, UserDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(user => user.UserName)); 
        }
    }
}
