using AutoMapper;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Domain.Entities.AuthEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Mapping
{
    public class AuthMapping : Profile
    {
        public AuthMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(desc => desc.Roles, opt => opt.MapFrom(src => src.roles));

            CreateMap<ApplicationUserDto, ApplicationUser>()
                .ForMember(dest => dest.roles, opt => opt.Ignore());

            CreateMap<ApplicationUser, ApplicationUserWithRolesDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ApplicationRoleDto, ApplicationRole>().ReverseMap();
        }
    }
}
