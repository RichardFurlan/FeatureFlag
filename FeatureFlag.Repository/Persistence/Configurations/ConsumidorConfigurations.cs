using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class ConsumidorConfigurations : IEntityTypeConfiguration<Consumidor>
{
    public void Configure(EntityTypeBuilder<Consumidor> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Identificacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(150);
    }
}