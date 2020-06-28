using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Infraestructure.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Role__46CA8DBE7E5CE900");

            builder.Property(e => e.Id).HasColumnName("Id_role");

            builder.Property(e => e.RoleType)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
