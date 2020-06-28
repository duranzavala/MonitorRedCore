using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.Entities;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Delete(int id);
    }
}
