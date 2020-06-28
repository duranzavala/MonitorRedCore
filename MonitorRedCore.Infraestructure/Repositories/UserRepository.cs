using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        public UserRepository(MONITOREDContext context) : base(context) { }

        public Users GetUserByEmail(string email)
        {
            var user = _entities.Where(x => x.Email == email).FirstOrDefault();
            return user;
        }

        public async Task<IEnumerable<Users>> GetUsersByRole(int roleId)
        {
            return await _entities.Where(x => x.Role == roleId).ToListAsync();
        }
    }
}
