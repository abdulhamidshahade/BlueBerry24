using AutoMapper;
using Berryfy.Application.Dtos.AuthDtos;
using Berryfy.Domain.Entities.AuthEntities;

namespace Berryfy.Application.Mapping
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
