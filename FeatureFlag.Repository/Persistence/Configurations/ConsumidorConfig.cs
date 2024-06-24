using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class ConsumidorConfig : IEntityTypeConfiguration<Consumidor>
{
    public void Configure(EntityTypeBuilder<Consumidor> builder)
    {
        throw new NotImplementedException();
    }
}