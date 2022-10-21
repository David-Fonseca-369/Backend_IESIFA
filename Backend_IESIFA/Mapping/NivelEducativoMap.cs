using Backend_IESIFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_IESIFA.Mapping
{
    public class NivelEducativoMap : IEntityTypeConfiguration<NivelEducativo>
    {
        public void Configure(EntityTypeBuilder<NivelEducativo> builder)
        {
            builder.ToTable("nivelEducativo")
                .HasKey(x => x.Id);
        }
    }
}
