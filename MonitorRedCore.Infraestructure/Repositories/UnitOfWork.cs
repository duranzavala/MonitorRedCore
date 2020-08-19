using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MONITOREDContext _context;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;
        private readonly IAwsService _awsService;

        private readonly IRepository<Role> _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public UnitOfWork(
            MONITOREDContext context,
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool pool,
            IAwsService awsService)
        {
            _context = context;
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _pool = pool;
            _awsService = awsService;
        }

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public IRepository<Role> RoleRepository => _roleRepository ?? new BaseRepository<Role>(_context);

        public IAuthRepository AuthRepository => _authRepository ?? new AuthRepository(_userManager, _signInManager, _pool, _awsService);

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
