using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class RecursoConsumidorConfigurations : IEntityTypeConfiguration<RecursoConsumidor>
{
    public void Configure(EntityTypeBuilder<RecursoConsumidor> builder)
    {
        builder.HasKey(rc => rc.Id);
    }
}