using System.Threading.Tasks;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthService
    {
        public Task<GenericResponse<string>> SignUp(UserDto user);
        public Task<GenericResponse<string>> SignIn(AuthDto authDto);
        public Task SignOut();
    }
}
