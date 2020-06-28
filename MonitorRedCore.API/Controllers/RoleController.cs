using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieve all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoles()
        {
            var rolesDto = _roleService.GetRoles();

            return Ok(rolesDto);
        }

        /// <summary>
        /// Retrieve specific role
        /// </summary>
        /// <param name="id">Role id to retrieve it</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var roleDto = await _roleService.GetRole(id);
            ApiResponse<RoleDto> response = new ApiResponse<RoleDto>(roleDto);

            if (response.Data != null)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Register a role
        /// </summary>
        /// <param name="roleDto">Model with the required information to register a role</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterRole(RoleDto roleDto)
        {
            var result = await _roleService.RegisterRole(roleDto);

            return Ok(result);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="id">Role id to delete it</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRole(id);

            return Ok(result);
        }
    }
}
