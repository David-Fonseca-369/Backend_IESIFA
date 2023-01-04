
using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class CalificacionDetallePreparatoriaMap : IEntityTypeConfiguration<CalificacionDetallePreparatoria>
    {
        public void Configure(EntityTypeBuilder<CalificacionDetallePreparatoria> builder)
        {
            builder.ToTable("calificacionDetallePreparatoria")
                .HasKey(x => x.Id);

            //reference            
            builder.HasOne(x => x.CalificacionCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
            builder.HasOne(x => x.Alumno).WithMany().HasForeignKey(x => x.IdAlumno);
        }
    }
}
