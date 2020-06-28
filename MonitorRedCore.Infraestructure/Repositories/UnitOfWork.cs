using System.Threading.Tasks;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MONITOREDContext _context;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(MONITOREDContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public IRepository<Role> RoleRepository => _roleRepository ?? new BaseRepository<Role>(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
