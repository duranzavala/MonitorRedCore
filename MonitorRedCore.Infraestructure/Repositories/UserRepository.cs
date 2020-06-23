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

        public IList<Users> GetUsers()
        {
            var users = _context.Users.ToListAsync().Result;

            return users;
        }
    }
}
