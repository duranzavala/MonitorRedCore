using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn(UserDto userDto)
        {
            var result = await _authService.SignIn(userDto);
            var response = new ApiResponse<bool>(result);

            if (response.Data)
            {
                return Ok(response);
            }

            return BadRequest(result);
        }

        [HttpGet("SignOut")]
        public async Task<ActionResult<bool>> SignOut(UserDto user)
        {
            await _authService.SignOut();

            return Ok();
        }
    }
}