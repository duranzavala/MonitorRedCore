using System;
using System.Threading.Tasks;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IAuthRepository AuthRepository { get; }

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
