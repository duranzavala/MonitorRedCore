using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int id);
        IEnumerable<UserDto> GetUsers();
        Task<bool> RegisterUser(UserDto userDto);
        Task<bool> DeleteUser(int id);
    }
}
