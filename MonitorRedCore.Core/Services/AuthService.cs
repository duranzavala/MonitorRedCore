using System.Threading.Tasks;
using AutoMapper;
using MonitorRedCore.API.Responses;
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

        public async Task<GenericResponse<string>> SignUp(UserDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);

            var result = await _unitOfWork.AuthRepository.SignUp(user);

            return result;
        }

        public async Task<GenericResponse<string>> SignIn(AuthDto authDto)
        {
            var result = await _unitOfWork.AuthRepository.SignIn(authDto);

            return result;
        }

        public async Task SignOut()
        {
            await _unitOfWork.AuthRepository.SignOut();
        }
    }
}
