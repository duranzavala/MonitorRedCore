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

        public Role GetRole(string roleType)
        {
            var role = _context.Role.FirstOrDefaultAsync(x => x.RoleType == roleType).Result;

            return role;
        }

        public IList<Role> GetRoles()
        {
            var roles = _context.Role.ToListAsync().Result;

            return roles;
        }

        public async Task RegisterRole(Role role)
        {
            try
            {
                _context.Role.Add(role);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                System.Console.Write("Trono alv: ", ex.Message.ToString());
            }
        }
    }
}
