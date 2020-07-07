using System;
using System.Threading.Tasks;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> SignUp(Users user);
        Task<string> SignIn(AuthDto authDto);
        Task SignOut();
    }
}
