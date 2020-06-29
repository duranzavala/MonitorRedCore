using System;
using System.Threading.Tasks;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> SignUp(Users user);
        Task<bool> SignIn(Users user);
        Task SignOut();
    }
}
