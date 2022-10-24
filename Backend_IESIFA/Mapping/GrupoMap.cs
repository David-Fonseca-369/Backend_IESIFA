using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class GrupoMap : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("grupo")
                .HasKey(x => x.Id);

            //reference            
            builder.HasOne(x => x.Grado).WithMany().HasForeignKey(x => x.IdGrado);
        }
    }
}
