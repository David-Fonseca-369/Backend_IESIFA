using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class MateriaMap : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> builder)
        {
            builder.ToTable("materia")
                .HasKey(x => x.Id);

            //reference            
            builder.HasOne(x => x.Grupo).WithMany().HasForeignKey(x => x.IdGrupo);
            builder.HasOne(x => x.Docente).WithMany().HasForeignKey(x => x.IdDocente);
        }
    }
}
