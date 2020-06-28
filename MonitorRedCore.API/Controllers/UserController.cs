using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.QueryFilters;
using MonitorRedCore.Infraestructure.Interfaces;
using Newtonsoft.Json;

namespace MonitorRedCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUriService _uriService;

        public UserController(IUserService userService, IUriService uriService)
        {
            _userService = userService;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetUsers))]
        public IActionResult GetUsers([FromQuery] UserQueryFilter filters)
        {
            var usersDto = _userService.GetUsers(filters);

            var metadata = new Metadata
            {
                TotalCount = usersDto.TotalCount,
                PageSize = usersDto.PageSize,
                CurrentPage = usersDto.CurrentPage,
                TotalPage = usersDto.TotalPage,
                HasNextPage = usersDto.HasNextPage,
                HasPreviousPage = usersDto.HasPreviousPage,
                NextPageUrl = _uriService.GetUsersPaginationUri(filters, Url.RouteUrl(nameof(GetUsers))).ToString(),
                PreviousPageUrl = _uriService.GetUsersPaginationUri(filters, Url.RouteUrl(nameof(GetUsers))).ToString()
            };

            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto)
            {
                Meta = metadata
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