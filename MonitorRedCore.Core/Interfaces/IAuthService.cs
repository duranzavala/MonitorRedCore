using System.Threading.Tasks;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> SignUp(UserDto user);
        public Task<LoginResponse> SignIn(AuthDto authDto);
        public Task SignOut();
    }
}
