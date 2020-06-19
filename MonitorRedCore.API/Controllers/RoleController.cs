using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var users = _roleRepository.GetRoles();
            return Ok(null);
        }
    }
}
