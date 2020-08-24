using System.Threading.Tasks;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<GenericResponse<string>> SignUp(Users user);
        Task<GenericResponse<string>> SignIn(AuthDto authDto);
        Task SignOut();
    }
}

