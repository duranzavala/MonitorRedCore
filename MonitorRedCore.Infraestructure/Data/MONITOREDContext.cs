using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data.Configuration;

namespace MonitorRedCore.Infraestructure.Data
{
    public partial class MONITOREDContext : DbContext
    {
        public MONITOREDContext()
        {
        }

        public MONITOREDContext(DbContextOptions<MONITOREDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
