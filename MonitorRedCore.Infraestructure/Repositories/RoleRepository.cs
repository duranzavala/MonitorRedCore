using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MONITOREDContext _context;

        public RoleRepository(MONITOREDContext context)
        {
            _context = context;
        }

        public IList<Role> GetRoles()
        {
            var roles = _context.Role.ToListAsync().Result;

            return roles;
        }
    }
}
