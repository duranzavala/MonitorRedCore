using System.Threading.Tasks;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.QueryFilters;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int id);
        PagedList<UserDto> GetUsers(UserQueryFilter filters);
        Task<GenericResponse<string>> SignUp(UserDto userDto);
        Task<bool> DeleteUser(int id);
    }
}
