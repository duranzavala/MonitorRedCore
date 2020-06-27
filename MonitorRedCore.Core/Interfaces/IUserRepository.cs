using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IUserRepository : IRepository<Users>
    {
        Users GetUserByEmail(string email);
        Task<IEnumerable<Users>> GetUsersByRole(int roleId);
    }
}
