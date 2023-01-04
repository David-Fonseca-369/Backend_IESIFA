using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class PeriodoMap : IEntityTypeConfiguration<Periodo>
    {
        public void Configure(EntityTypeBuilder<Periodo> builder)
        {

            builder.ToTable("periodo")
                .HasKey(x => x.Id);


            //reference            
            builder.HasOne(x => x.NivelEducativo).WithMany().HasForeignKey(x => x.IdNivelEducativo);

        }
    }
}
