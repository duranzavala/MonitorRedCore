using AutoMapper;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Infraestructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Users, UserDto>();
            CreateMap<UserDto, Users>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }
}
