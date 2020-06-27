using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetRole(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetById(id);

            if (role == null)
            {
                throw new Exception($"There isn't a role with ID: {id}");
            }

            var roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            var roles =_unitOfWork.RoleRepository.GetAll();
            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return rolesDto;
        }

        public async Task<bool> RegisterRole(RoleDto roleDto)
        {
            var roles = _unitOfWork.RoleRepository.GetAll();
            var roleExist = roles.FirstOrDefault(role => role.RoleType == roleDto.RoleType);

            if (roleExist != null)
            {
                throw new Exception("The role already exist");
            }

            var role = _mapper.Map<Role>(roleDto);
            await _unitOfWork.RoleRepository.Add(role);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRole(int id)
        {
            var roleExist = await _unitOfWork.RoleRepository.GetById(id);

            if (roleExist == null)
            {
                throw new Exception("The role doesn't exist");
            }

            await _unitOfWork.RoleRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
