using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class GradoMap : IEntityTypeConfiguration<Grado>
    {
        public void Configure(EntityTypeBuilder<Grado> builder)
        {
            builder.ToTable("grado")
                .HasKey(x => x.Id);

            //reference
            builder.HasOne(x => x.NivelEducativo).WithMany().HasForeignKey(x => x.IdNivelEducativo);
        }
    }
}
