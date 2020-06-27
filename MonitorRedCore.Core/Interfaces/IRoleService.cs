using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetRole(int id);
        IEnumerable<RoleDto> GetRoles();
        Task<bool> RegisterRole(RoleDto roleDto);
        Task<bool> DeleteRole(int id);
    }
}
