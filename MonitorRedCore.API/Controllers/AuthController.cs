using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.API.Controllers
{
    [Produces("application/json")]
    [Route("command/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(AuthDto authDto)
        {
            var result = await _authService.SignIn(authDto);

            var response = new ApiResponse<LoginResponse>(result);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<bool>> SignOut()
        {
            await _authService.SignOut();

            return Ok();
        }
    }
}