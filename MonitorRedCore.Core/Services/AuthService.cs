using System.Threading.Tasks;
using AutoMapper;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> SignUp(UserDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);

            await _unitOfWork.AuthRepository.SignUp(user);

            return true;
        }

        public async Task<bool> SignIn(UserDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);

            await _unitOfWork.AuthRepository.SignIn(user);

            return true;
        }

        public async Task SignOut()
        {
            await _unitOfWork.AuthRepository.SignOut();
        }
    }
}
