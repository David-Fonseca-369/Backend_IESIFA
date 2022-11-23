
using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class AlumnoMap : IEntityTypeConfiguration<Alumno>
    {
        public void Configure(EntityTypeBuilder<Alumno> builder)
        {
            builder.ToTable("alumno")
                .HasKey(x => x.Id);

            //reference            
            builder.HasOne(x => x.Grupo).WithMany().HasForeignKey(x => x.IdGrupo);
            builder.HasOne(x => x.Genero).WithMany().HasForeignKey(x => x.IdGenero);
        }
    }
}
