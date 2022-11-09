using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
                .HasKey(x => x.Id);

            //reference
            builder.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.IdRol);
        }
    }
}
