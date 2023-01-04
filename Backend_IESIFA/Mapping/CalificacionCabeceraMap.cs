
using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class CalificacionCabeceraMap : IEntityTypeConfiguration<CalificacionCabecera>
    {
        public void Configure(EntityTypeBuilder<CalificacionCabecera> builder)
        {
            builder.ToTable("calificacionCabecera")
                .HasKey(x => x.Id);

            //reference            
            builder.HasOne(x => x.Materia).WithMany().HasForeignKey(x => x.IdMateria);
            builder.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo);
            builder.HasOne(x => x.NivelEducativo).WithMany().HasForeignKey(x => x.IdNivelEducativo);
            builder.HasOne(x => x.Docente).WithMany().HasForeignKey(x => x.IdDocente);
        }
    }
}
