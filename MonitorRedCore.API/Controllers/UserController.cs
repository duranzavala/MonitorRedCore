using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var usersDto = _userService.GetUsers();

            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userDto = await _userService.GetUser(id);
            ApiResponse<UserDto> response = new ApiResponse<UserDto>(userDto);

            if (response.Data != null)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            var result = await _userService.RegisterUser(userDto);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RegisterUserAsync(int id)
        {
            var result = await _userService.DeleteUser(id);

            return Ok(result);
        }
    }
}