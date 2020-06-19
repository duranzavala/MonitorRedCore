using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Models;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MONITOREDContext _context;

        public RoleRepository(MONITOREDContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            var roles = await _context.Role.ToListAsync();

            return roles;
        }
    }
}
