using AutoMapper;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Infraestructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
