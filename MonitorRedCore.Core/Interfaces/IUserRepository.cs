using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IUserRepository
    {
        IList<Users> GetUsers();
        Users GetUser(string email);
        Task RegisterUser(Users user);
    }
}
