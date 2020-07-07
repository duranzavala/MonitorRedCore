using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MonitorRedCore.Core.CustomEntities;
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

        public async Task<string> SignIn(AuthDto authDto)
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
