using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Entities;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MONITOREDContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(MONITOREDContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}
