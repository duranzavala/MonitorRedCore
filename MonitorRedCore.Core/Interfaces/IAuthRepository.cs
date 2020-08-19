using System;
using System.Threading.Tasks;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> SignUp(Users user);
        Task<LoginResponse> SignIn(AuthDto authDto);
        Task SignOut();
    }
}
