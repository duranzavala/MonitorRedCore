using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Exceptions;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Core.QueryFilters;

namespace MonitorRedCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PaginationOptions _paginationOptions;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paginationOptions = options.Value;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                throw new BusinessException($"There isn't a user with ID: {id}");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public PagedList<UserDto> GetUsers(UserQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var users = _unitOfWork.UserRepository.GetAll();

            if (filters.FirstName != null)
            {
                users = users.Where(x => x.FirstName.ToLower() == filters.FirstName.ToLower());
            }
            if (filters.Role != null)
            {
                var role = _unitOfWork.RoleRepository.GetAll().FirstOrDefault(x => x.RoleType.ToLower() == filters.Role.ToLower());
                users = users.Where(x => x.Role == role.Id);
            }
            if (filters.Email != null)
            {
                users = users.Where(x => x.Email.ToLower() == filters.Email.ToLower());
            }

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            var pagedUsers = PagedList<UserDto>.Create(usersDto, filters.PageNumber, filters.PageSize);
            

            return pagedUsers;
        }

        public async Task<GenericResponse<string>> SignUp(UserDto userDto)
        {
            var userExist = _unitOfWork.UserRepository.GetUserByEmail(userDto.Email);

            if (userExist != null)
            {
                throw new BusinessException("The user already exist");
            }

            var user = _mapper.Map<Users>(userDto);
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            var cognitoResult = await _unitOfWork.AuthRepository.SignUp(user);

            return cognitoResult;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var userExist = await _unitOfWork.UserRepository.GetById(id);

            if (userExist == null)
            {
                throw new BusinessException("The user doesn't exist");
            }

            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
