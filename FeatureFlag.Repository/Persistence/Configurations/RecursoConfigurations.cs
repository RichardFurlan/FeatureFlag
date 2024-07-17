using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class RecursoConfigurations : IEntityTypeConfiguration<Recurso>
{
    public void Configure(EntityTypeBuilder<Recurso> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Identificacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Descricao)
            .IsRequired()
            .HasMaxLength(150);
    }
}