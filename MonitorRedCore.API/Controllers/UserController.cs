using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.QueryFilters;
using Newtonsoft.Json;

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
        public IActionResult GetUsers([FromQuery] UserQueryFilter filters)
        {
            var usersDto = _userService.GetUsers(filters);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);

            var metadata = new
            {
                usersDto.TotalCount,
                usersDto.PageSize,
                usersDto.CurrentPage,
                usersDto.TotalPage,
                usersDto.HasNextPage,
                usersDto.HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userDto = await _userService.GetUser(id);
            var response = new ApiResponse<UserDto>(userDto);

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
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RegisterUserAsync(int id)
        {
            var result = await _userService.DeleteUser(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}