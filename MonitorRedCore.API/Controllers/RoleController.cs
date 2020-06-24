using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return Ok(rolesDto);
        }

        [HttpGet("{roleType}")]
        public IActionResult GetRole(string roleType)
        {
            var role = _roleRepository.GetRole(roleType);
            var roleDto = _mapper.Map<RoleDto>(role);

            return Ok(roleDto);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRole(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.RegisterRole(role);

            return Ok(role);
        }
    }
}
