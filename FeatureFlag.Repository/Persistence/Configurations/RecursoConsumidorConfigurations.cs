using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class RecursoConsumidorConfigurations : IEntityTypeConfiguration<RecursoConsumidor>
{
    public void Configure(EntityTypeBuilder<RecursoConsumidor> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.HasOne<Consumidor>()
            .WithMany(c => c.RecursoConsumidores)
            .HasForeignKey(rc => rc.CodigoConsumidor);
        
        builder.HasOne<Recurso>()
            .WithMany(r => r.RecursoConsumidores)
            .HasForeignKey(rc => rc.CodigoRecurso);
    }
}