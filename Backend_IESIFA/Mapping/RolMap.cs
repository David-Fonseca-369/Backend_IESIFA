using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("rol")
                .HasKey(x => x.Id);
        }
    }
}
