using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MONITOREDContext _context;

        public UserRepository(MONITOREDContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }
    }
}
