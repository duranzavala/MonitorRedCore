using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            if (user == null)
            {
                throw new Exception($"There isn't a user with ID: {id}");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }

        public async Task<bool> RegisterUser(UserDto userDto)
        {
            var userExist = _unitOfWork.UserRepository.GetUserByEmail(userDto.Email);

            if (userExist != null)
            {
                throw new Exception("The user already exist");
            }

            var user = _mapper.Map<Users>(userDto);
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var userExist = await _unitOfWork.UserRepository.GetById(id);

            if (userExist == null)
            {
                throw new Exception("The user doesn't exist");
            }

            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
